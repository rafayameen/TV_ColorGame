using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TaskUIItem : MonoBehaviour
{
    public Text taskText;
    public Text percentageText;

    public Image fillBar;
    public Image checkBoxImage;

    public virtual void SetText(string text)
    {
        taskText.text = text;
    }

    public void SetPercentage(float percentageValue)
    { 
        fillBar.DOFillAmount(percentageValue,0.5f);
        percentageText.text = (int)(percentageValue*100) +"%";
    }

    public void MarkCompleted(Sprite completeSprite)
    {
        if (checkBoxImage)
            checkBoxImage.sprite = completeSprite;

        Destroy(gameObject, 0.1f);
    }
}
