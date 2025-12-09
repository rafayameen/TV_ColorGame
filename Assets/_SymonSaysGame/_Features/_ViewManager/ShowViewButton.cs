using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShowViewButton : MonoBehaviour
{
    public Views viewToShow = Views.RemoveAdsPanel;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {

            EventManager.DoFireLogAnalyticsEvent($"Sh_{viewToShow}_Cl");

            AudioManager.instance?.Play(SoundType.BUTTONCLICK);
            ViewManager.Show(viewToShow);

            if(viewToShow == Views.PausePanel)
                EventManager.DoFireShowInterstial();
        });
    }

}
