using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Sprite onSprite, offSprite;
    public Button soundBtn, musicBtn, vibrationBtn;
    public Slider sensitivitySlider;

    public Button closeButton;
    public Button privacyPolicyButton;

    // Add input fields for min and max sensitivity values
    public InputField minValueInputField;
    public InputField maxValueInputField;

    public Dropdown qualityDropdown;

    private bool isSoundOn, isMusicOn, isVibrationOn;

    private void OnEnable()
    {
        EventManager.DoFireHideNativeAds(true);
    }

    private void OnDisable()
    {
        EventManager.DoFireHideNativeAds(false);
    }

    private void Start()
    {
        // Load settings using HelperFunctions
        isSoundOn = HelperFunctions.GetSound();
        isMusicOn = HelperFunctions.GetMusic();
        isVibrationOn = HelperFunctions.GetVibration();
        float sensitivity = HelperFunctions.GetSensitivity();

        AdjustQuality();


        // Set button sprites
        UpdateButtonSprite(soundBtn, isSoundOn);
        UpdateButtonSprite(musicBtn, isMusicOn);
        UpdateButtonSprite(vibrationBtn, isVibrationOn);

        // Set slider value
        sensitivitySlider.value = sensitivity;

        // Add listeners
        soundBtn.onClick.AddListener(() => ToggleSetting(ref isSoundOn, HelperFunctions.SetSound, soundBtn));
        musicBtn.onClick.AddListener(() => ToggleSetting(ref isMusicOn, HelperFunctions.SetMusic, musicBtn));
        vibrationBtn.onClick.AddListener(() => ToggleSetting(ref isVibrationOn, HelperFunctions.SetVibration, vibrationBtn));

        closeButton.onClick.AddListener(() =>
        {
            EventManager.DoFireShowInterstial();
            EventManager.DoFireLogAnalyticsEvent("settings_closed");

            EventManager.DoFireSettingsUpdated();
            gameObject.SetActive(false);
        });

        sensitivitySlider.onValueChanged.AddListener(value =>
        {
            HelperFunctions.SetSensitivity(value);
        });

        // Set listeners for InputFields to update slider min/max values
        minValueInputField.onValueChanged.AddListener(UpdateSliderMinValue);
        maxValueInputField.onValueChanged.AddListener(UpdateSliderMaxValue);

        // Initialize InputFields with current min and max values of the slider
        minValueInputField.text = sensitivitySlider.minValue.ToString();
        maxValueInputField.text = sensitivitySlider.maxValue.ToString();

        // Quality dropdown listener
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        privacyPolicyButton.onClick.AddListener(()=> 
        {
            Application.OpenURL(GameConstants.LINK_PRIVACYPOLICY);
        });
    }

    public void AdjustQuality()
    {
        int savedQuality = PlayerPrefs.GetInt("QualityLevel", DetectDevicePerformance()); // Load saved or auto-detected quality

        // Set dropdown value
        qualityDropdown.value = savedQuality;
        QualitySettings.SetQualityLevel(savedQuality);
    }

    private void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityLevel", index);
        PlayerPrefs.Save();
    }

    private int DetectDevicePerformance()
    {
        int ramSize = SystemInfo.systemMemorySize;  // RAM in MB
        int processorCount = SystemInfo.processorCount;  // CPU cores
        int processorFrequency = SystemInfo.processorFrequency;  // CPU speed in MHz
        string gpuName = SystemInfo.graphicsDeviceName;  // GPU Model
        int vram = SystemInfo.graphicsMemorySize;  // VRAM in MB

        Debug.Log($"RAM: {ramSize}MB, CPU Cores: {processorCount}, CPU Speed: {processorFrequency}MHz, GPU: {gpuName}, VRAM: {vram}MB");

        // Determine quality based on multiple factors
        if (ramSize <= 4096 || processorFrequency < 1500 || vram < 512)
            return 0; // Low quality

        if (ramSize <= 6144 || processorFrequency < 2200 || vram < 2048)
            return 1; // Medium quality

        return 2; // Ultra quality
    }


    private void ToggleSetting(ref bool setting, System.Action<bool> saveAction, Button btn)
    {
        setting = !setting;
        saveAction(setting); // Save using HelperFunctions
        UpdateButtonSprite(btn, setting);

        if (saveAction == HelperFunctions.SetMusic)
        { 
            AudioManager.instance?.ToggleMusic(setting);

            AudioManager.instance?.PlayStopMusic(setting);
        }
        else if (saveAction == HelperFunctions.SetSound)
            AudioManager.instance?.ToggleSound(setting);
    }

    private void UpdateButtonSprite(Button btn, bool isOn)
    {
        btn.image.sprite = isOn ? onSprite : offSprite;
    }

    // Update slider minValue
    private void UpdateSliderMinValue(string value)
    {
        if (float.TryParse(value, out float newMinValue))
        {
            sensitivitySlider.minValue = newMinValue;
        }
    }

    // Update slider maxValue
    private void UpdateSliderMaxValue(string value)
    {
        if (float.TryParse(value, out float newMaxValue))
        {
            sensitivitySlider.maxValue = newMaxValue;
        }
    }
}
