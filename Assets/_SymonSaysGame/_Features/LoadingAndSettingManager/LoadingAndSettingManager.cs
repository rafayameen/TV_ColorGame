using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingAndSettingManager : MonoBehaviour
{
    public static LoadingAndSettingManager Instance;

    public string[] tipsTextStrings;
    public GameObject loadingScreen;
    public Text loadingText;
    public GameObject settingsScreen;

    public Image loadingFill;
    public Text loadingPercentageText;

    private Coroutine tipsCoroutine;
    private Coroutine loadingCoroutine;
    private string m_sceneToLoad = "";

    public Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Application.targetFrameRate = 30;
        settingsScreen.GetComponent<SettingsPanel>().AdjustQuality();
    }


    private void OnEnable()
    {
        EventManager.showSettings += ShowSettings;

        //  EventManager.loadScene += LoadGivenScene;
    }

    private void OnDisable()
    {
        EventManager.showSettings -= ShowSettings;
        //  EventManager.loadScene -= LoadGivenScene;
    }

    public void SetCam(Camera cam)
    {
        if (canvas)
            canvas.worldCamera = cam;
    }

    void ShowSettings()
    {
        EnableSettings(true);
    }

    void LoadGivenScene(string sceneName)
    {
        EnableLoading(true, sceneName);
    }

    // Enable or disable the settings screen
    public void EnableSettings(bool enable)
    {
        if (settingsScreen != null)
        {
            settingsScreen.SetActive(enable);
        }
    }

    // Enable the loading screen and optionally load a scene
    public void EnableLoading(bool loadScene = false, string sceneToLoad = "")
    {
        Debug.Log($"Async Loading Enabled For Scene : {sceneToLoad} loadScene: {loadScene}");

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);

            // Stop any existing coroutines before starting new ones
            if (tipsCoroutine != null) StopCoroutine(tipsCoroutine);
            tipsCoroutine = StartCoroutine(DisplayTips());

            if (loadScene && !string.IsNullOrEmpty(sceneToLoad))
            {
                m_sceneToLoad = sceneToLoad;

                if (loadingCoroutine != null) StopCoroutine(loadingCoroutine);
                loadingCoroutine = StartCoroutine(LoadSceneAsync(m_sceneToLoad));
            }
        }
    }

    // Disable the loading screen
    public void DisableLoading()
    {
        Debug.Log("Async Loading Disabled!");

        canLoadScene = false;


        loadingCoroutine = null;
        tipsCoroutine = null;


        StopAllCoroutines();

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);

        }
    }

    // Coroutine to display tips one by one
    private IEnumerator DisplayTips()
    {
        while (true)
        {
            foreach (var tip in tipsTextStrings)
            {
                if (loadingText != null)
                {
                    loadingText.text = tip;
                }
                yield return new WaitForSeconds(Random.Range(2.5f, 5f));
            }
        }
    }



    bool canLoadScene = false;

    // Coroutine to load a scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log($"Loading Scene Async: {sceneName}");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;
        loadingFill.fillAmount = 0;

        canLoadScene = true;

        while (!operation.isDone)
        {
            if (loadingFill)
                loadingFill.DOFillAmount(operation.progress, Random.Range(0.3f, 0.5f));

            if (loadingPercentageText)
                loadingPercentageText.text = $"{operation.progress * 100:0}%";

            if (operation.progress >= 0.9f && canLoadScene == true)
            {
                operation.allowSceneActivation = true;

                Debug.Log($"Loading Scene Complete: {sceneName}");
            }

            yield return null;
        }

        DisableLoading();
    }

    private float fakeProgress = 0f;
    private Tween progressTween;

    public void StartFakeProgress(float min = 5, float max = 7)
    {
        fakeProgress = 0f;
        progressTween?.Kill(); // Stop any previous animation

        float duration = Random.Range(min, max);
        progressTween = DOTween.To(() => fakeProgress, SetFakeProgress, 1f, duration);
    }

    void SetFakeProgress(float progress)
    {
        progress = Mathf.Clamp01(progress); // Ensure progress stays between 0 and 1

        if (loadingFill)
            loadingFill.fillAmount = progress;

        if (loadingPercentageText)
            loadingPercentageText.text = $"{progress * 100:0}%";
    }
}
