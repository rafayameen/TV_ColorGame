using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Views
{
    SellOresPanel, UpgradesPanel, MainMenu, ModeSelection, RemoveAdsPanel, PausePanel, TreasureRewardPanel
}

public class ViewManager : MonoBehaviour
{
    private static ViewManager s_instance;

    [SerializeField] private View _startingView;
    [SerializeField] private View[] _views;

    private View _currentView;
    private readonly Stack<Views> _history = new Stack<Views>();

    public Text progressText;
    public GameObject storeTestButton;

    public void EnableStoreBtn()
    {
        if (storeTestButton)
            storeTestButton.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.onSetProgress += UpdateProgress;
    }

    private void OnDisable()
    {
        EventManager.onSetProgress -= UpdateProgress;
        
    }

    private void UpdateProgress(string progress)
    {
        if (progressText)
            progressText.text = progress;
    }

    public static void Show(Views viewEnum, ViewContext context = null, bool remember = true, bool hideCurrent = false)
    {
        // Find the view corresponding to the Views enum
        View viewToShow = System.Array.Find(s_instance._views, v => v.ViewType == viewEnum);

        if (viewToShow != null)
        {
            // If there's a current view and we need to remember the history, push it onto the stack
            if (s_instance._currentView != null)
            {
                if (remember)
                {
                    s_instance._history.Push(s_instance._currentView.ViewType);
                }

                // Hide the current view only if hideCurrent is true
                if (hideCurrent)
                {
                    s_instance._currentView.Hide();
                }
            }

            // Pass the context data to the view, if available
            viewToShow.Show(context);

            viewToShow.ProcessContext(context);

            // Update the current view
            s_instance._currentView = viewToShow;
        }
        else
        {
            Debug.LogError($"View {viewEnum} not found in the ViewManager.");
        }
    }


    //public static void Show(Views viewEnum, bool remember = true, bool hideCurrent = false)
    //{
    //    // Find the view corresponding to the Views enum
    //    View viewToShow = System.Array.Find(s_instance._views, v => v.ViewType == viewEnum);

    //    if (viewToShow != null)
    //    {
    //        if (s_instance._currentView != null)
    //        {
    //            if (remember)
    //            {
    //                s_instance._history.Push(s_instance._currentView.ViewType);
    //            }

    //            // Hide the current view only if hideCurrent is true
    //            if (hideCurrent)
    //            {
    //                s_instance._currentView.Hide();
    //            }
    //        }

    //        viewToShow.Show();
    //        s_instance._currentView = viewToShow;
    //    }
    //    else
    //    {
    //        Debug.LogError($"View {viewEnum} not found in the ViewManager.");
    //    }
    //}

    public static void HideViewByID(Views viewEnum)
    {
        // Find the view corresponding to the Views enum
        View viewToHide = System.Array.Find(s_instance._views, v => v.ViewType == viewEnum);

        if (viewToHide != null)
        {
            if (viewToHide == s_instance._currentView)
            {
                // If the view to hide is the current view, set _currentView to null
                s_instance._currentView = null;
            }

            viewToHide.Hide();
        }
        else
        {
            Debug.LogError($"View {viewEnum} not found in the ViewManager.");
        }
    }


    public static void ShowLast()
    {
        if (s_instance._history.Count != 0)
        {
            Views lastViewEnum = s_instance._history.Pop();
            Show(lastViewEnum, null, false);
        }
    }

    private void Awake() => s_instance = this;

    private void Start()
    {
        foreach (var view in _views)
        {
            view.Initialize();
            // view.Hide();
        }

        if (_startingView != null)
        {
            Show(_startingView.ViewType, null, false);
        }
    }
}

public class ViewContext
{
    public object CustomData { get; set; }

    public ViewContext(object customData)
    {
        CustomData = customData;
    }
}
