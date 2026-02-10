using LiquidVolumeFX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class FlaskScript : MonoBehaviour
{
    [Header("References")]
    public LiquidVolume liquidVolume;
    public TMP_Text colorText;

    [Header("Particles")]
    public ParticleSystem bubbleParticles;

    [Header("Color Data")]
    public ColorId colorId;
    public Color flaskColor;

    [Header("Click Settings")]
    public bool interactable = true;

    public SpriteRenderer outlineImage;

    /// <summary>
    /// Called by manager to initialize flask visuals
    /// </summary>
    public void SetColor(ColorId id, Color color, string displayName)
    {
        colorId = id;
        flaskColor = color;

        // Liquid visuals
        if (liquidVolume != null)
        {
            liquidVolume.liquidColor1 = color;
            liquidVolume.emissionColor = color;
        }

        if (outlineImage)
            outlineImage.color = color;

        // Particle color
        if (bubbleParticles != null)
        {
            var main = bubbleParticles.main;
            main.startColor = color;
        }

        // Text
        if (colorText != null)
            colorText.text = displayName;
    }

    public void EnableOutline(bool enable)
    {
        if (outlineImage)
            outlineImage.gameObject.SetActive(enable);
    }

    public void Click()
    {
        
        if (!interactable) return;

        Debug.Log($"Flask clicked: {colorId}");

        EventManager.OnFlaskClicked?.Invoke(this);
        EnableOutline(true);
    }


    /// <summary>
    /// 3D click handling
    /// </summary>
    private void OnMouseDown()
    {
        if (!interactable) return;

        Debug.Log($"Flask clicked: {colorId}");

        EventManager.OnFlaskClicked?.Invoke(this);

        EnableOutline(true);
    }

    public void SetInteractable(bool value)
    {
        interactable = value;

        if (!value)
            EnableOutline(false);
    }

    public void Highlight(bool on)
    {
        // Hook for glow / scale / outline
        // Example:
        // transform.localScale = on ? Vector3.one * 1.05f : Vector3.one;
    }
}
