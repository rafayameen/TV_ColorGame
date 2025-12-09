/*
TimerUI.cs
Attach to a Timer UI GameObject with a Text / TMP and optional Image fill for progress.
Assign in inspector:

* timerText (TMP) to display seconds left
* timerFill (Image) as an optional radial/horizontal fill to show visual progress
  */
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public Text timerText;
    public Image timerFill;


private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnTimerUpdated += HandleTimerUpdated;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnTimerUpdated -= HandleTimerUpdated;
    }

    private void HandleTimerUpdated(float current, float max)
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(current).ToString();
        }
        if (timerFill != null && max > 0f)
        {
            timerFill.fillAmount = Mathf.Clamp01(current / max);
        }
    }


}
