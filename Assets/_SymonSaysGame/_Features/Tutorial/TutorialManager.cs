using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState
{
    MOVE_AND_ROTATE,
    GO_TO_DIGGING_AREA,
    START_DIGGING_A_HOLE,
    COLLECT_ORES,
    GO_TO_SELL_SHOP,
    UPGRADE_SHOVEL,
    RETURN_TO_YARD,
    DIG_TO_TUNNEL,
    FIND_AND_OPEN_TREASURECHEST,
    None
}


public class TutorialManager : MonoBehaviour
{
    public bool testing = true;
    public TutorialState testState = TutorialState.None;

    public static TutorialManager instance;

    private const string TutorialStateKey = "TutorialState";
    private const string TutorialCompleteKey = "TutorialComplete";

    public float cooldownTimeBetweenTasks = 2;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        // Load the saved state on start
        LoadTutorialState();
    }

    private TutorialState currentState;
    public Action OnTutorialComplete;

    public TaskScript[] tasks;

    internal TutorialState GetCurrentState()
    {
        return currentState;
    }

    private void OnEnable()
    {
        //GameManager.OnGameStateChanged += HandleGameState;
        EventManager.OnSubStateComplete += OnTaskCompleted;
        EventManager.onAllTasksUI_TaskComplete += OnStateComplete;
    }


    private void OnDisable()
    {
        //GameManager.OnGameStateChanged -= HandleGameState;
        EventManager.OnSubStateComplete -= OnTaskCompleted;
        EventManager.onAllTasksUI_TaskComplete -= OnStateComplete;
    }

    public void OnTaskCompleted(int taskIndex)
    {
        Debug.Log($"Task at index {taskIndex} completed!");

        //if (taskIndex < currentTask.tutorialTasks.Count)
        EnableNextSubTask(taskIndex);
        //else
        //    OnAllTasksCompleted();
    }

    public void OnAllTasksCompleted()
    {
        Debug.Log("All tutorial tasks are complete!");
        MarkTutorialComplete();
        //OnStateComplete();
    }



    private void EnableInitialSubTask()
    {


        //var nextTask = initialTask.tutorialTasks[0];
        //Debug.Log($"Next task enabled: {nextTask.mainTaskHeaderText}");

        //nextTask.EnableObjects(true);

        //if (nextTask.useButtonForNextTask)
        //{
        //    if (nextTask.taskCompleteButton)
        //    {
        //        Debug.LogError($"Listener Added To : {nextTask.taskCompleteButton} for task : {nextTask.mainTaskHeaderText}");

        //        nextTask.taskCompleteButton.onClick.AddListener(SubStateTaskComplete);
        //    }
        //}
        // Logic to visually or functionally enable the next task



    }

    private void EnableNextSubTask(int currentTaskIndex)
    {
        int totalTasks = currentTask.tutorialTasks.Count;

        if (currentTaskIndex <= totalTasks)
        {
            if (currentTaskIndex > 0)
            {
                var prevTask = currentTask.tutorialTasks[currentTaskIndex - 1];

                prevTask.EnableObjects(false);

                if (prevTask.useButtonForNextTask)
                {
                    if (prevTask.taskCompleteButton)
                        prevTask.taskCompleteButton.onClick.RemoveListener(SubStateTaskComplete);
                }
            }
        }

        if (currentTaskIndex < totalTasks)
        {

            var nextTask = currentTask.tutorialTasks[currentTaskIndex];
            Debug.Log($"Next task enabled: {nextTask.mainTaskHeaderText}");

            nextTask.EnableObjects(true);

            if (nextTask.useButtonForNextTask)
            {
                if (nextTask.taskCompleteButton)
                {
                    //Debug.LogError($"Listener Added To : {nextTask.taskCompleteButton} for task : {nextTask.mainTaskHeaderText}");

                    nextTask.taskCompleteButton.onClick.AddListener(SubStateTaskComplete);
                }

            }
            // Logic to visually or functionally enable the next task
        }
        else
        {
            Debug.Log("No more subtasks to enable.");
        }


    }

    private void SubStateTaskComplete()
    {

        EventManager.DoFireTaskUI_TaskComplete();

    }

    private int currentSubTaskIndex = 0;

    private void HandleSubStateComplete()
    {
        if (currentTask == null || currentSubTaskIndex >= currentTask.tutorialTasks.Count)
        {
            Debug.LogWarning("No more subtasks to complete.");
            return;
        }

        // Disable the current subtask objects
        TutorialTask currentSubTask = currentTask.tutorialTasks[currentSubTaskIndex];
        currentSubTask.EnableObjects(false);

        // Increment the subtask index
        currentSubTaskIndex++;

        // Enable the next subtask objects, if any
        if (currentSubTaskIndex < currentTask.tutorialTasks.Count)
        {
            TutorialTask nextSubTask = currentTask.tutorialTasks[currentSubTaskIndex];
            nextSubTask.EnableObjects(true);

            // Display updated tutorial text
            SetTutorialText(currentTask.mainTaskHeaderText, nextSubTask.TutorialText);
        }
        else
        {
            Debug.Log("All subtasks completed for this task.");
            OnStateComplete();
        }
    }


    int completedTaskIndex = 0;

    //private void HandleGameState(GameState state)
    //{
    //    if (state == GameState.Tutorial)
    //    {
    //        StartTutorial();
    //    }
    //}


    private void Start()
    {
        StartTutorial();
    }

    public void StartTutorial()
    {
        if (PlayerPrefs.GetInt(TutorialCompleteKey, 0) == 1)
        {
            Debug.Log("Tutorial already completed. Skipping...");
            SetTutorialState(TutorialState.None);
            return;
        }

        if (testing)
        {
            SetTutorialState(testState);
        }
        else
        {
            SetTutorialState(currentState == TutorialState.None ? TutorialState.MOVE_AND_ROTATE : currentState);
        }

        //HandleInitialTask();
    }

    //TaskScript initialTask;
    //public void HandleInitialTask()
    //{
    //    initialTask = GetTaskForState(currentState);

    //    if (initialTask != null)
    //        //handle initial state task
    //        EnableInitialSubTask();
    //}

    public void SetTutorialState(TutorialState newState)
    {
        if (newState == TutorialState.None)
            return;

        currentState = newState;
        SaveTutorialState(); // Save the current state
        EventManager.DoFireResetTaskUI_CompletedTasksCounter();


        DOVirtual.DelayedCall(cooldownTimeBetweenTasks, () =>
        {
            HandleState(currentState);
        });
    }

    TaskScript currentTask;

    private void HandleState(TutorialState state)
    {

        //switch (state)
        //{
        //    //case TutorialState.WelcomePanel:
        //    //    ViewManager.Show(Views.WelcomePanel);
        //    //    break;
        //    case TutorialState.MoveRotateTutorial:
        //        ViewManager.Show(Views.MoveRotateTutorial);
        //        break;
        //    default:
        //        break;
        //}


        if (currentTask != null)
        {
            // Disable all objects of the previous task
            foreach (var tutorialTask in currentTask.tutorialTasks)
            {
                tutorialTask.EnableObjects(false);
            }
        }

        EventManager.DoFireLogAnalyticsEvent($"Tut_{state}_St");

        // Get the corresponding task for the current state
        currentTask = GetTaskForState(state);

        Debug.Log($"getting task for state {state}");

        if (currentTask != null)
        {

            // Enable objects for all tutorial tasks within the current task
            // currentTask.EnableObjects(0, true);

            var nextTask = currentTask.tutorialTasks[0];

         //   Debug.Log($"Next task enabled: {nextTask.mainTaskHeaderText}");

            nextTask.EnableObjects(true);

            if (nextTask.useButtonForNextTask)
            {
                if (nextTask.taskCompleteButton)
                {
                    //Debug.LogError($"Listener Added To : {nextTask.taskCompleteButton} for task : {nextTask.mainTaskHeaderText}");

                    nextTask.taskCompleteButton.onClick.AddListener(SubStateTaskComplete);

                    nextTask.onCompleteEvent.AddListener(() =>
                    {
                        nextTask.taskCompleteButton.onClick.RemoveListener(SubStateTaskComplete);

                    });
                }
            }

          //  Debug.Log("Adding tasks");

            // Display tutorial text for each tutorial task within the current task
            foreach (var tutorialTask in currentTask.tutorialTasks)
            {
                if (tutorialTask.tutorialTaskType == TutorialTaskType.Simple)
                    SetTutorialText(currentTask.mainTaskHeaderText, tutorialTask.TutorialText);
                else
                    EventManager.DoFireAddCheckListTask(currentTask.mainTaskHeaderText, tutorialTask);
            }

            //HandleInitialTask();
        }

    }


    public void OnStateComplete()
    {
        Debug.Log($"Main State Complete: {currentState}");

        EventManager.DoFireLogAnalyticsEvent($"Tut_{currentState}_Cm");
        // Disable objects for the current task
        TaskScript currentTask = GetTaskForState(currentState);

        if (currentTask != null)
        {
            currentTask.EnableObjects(completedTaskIndex, false);


        }

        if (currentState == TutorialState.DIG_TO_TUNNEL)
        {
            SetTutorialState(TutorialState.None);
            return;
        }


        int currentStateIndex = (int)currentState;

        currentStateIndex++;


        TutorialState newState = (TutorialState)currentStateIndex;


        SetTutorialState(newState);

    }


    private void MarkTutorialComplete()
    {
        currentState = TutorialState.None;

        EventManager.DoFireLogAnalyticsEvent($"Tutorial_Complete");

        PlayerPrefs.SetInt(TutorialCompleteKey, 1);
        PlayerPrefs.Save();
        Debug.Log("Tutorial completed and saved.");
        OnTutorialComplete?.Invoke();
    }

    private void SaveTutorialState()
    {
        PlayerPrefs.SetInt(TutorialStateKey, (int)currentState);
        PlayerPrefs.Save();
        Debug.Log($"Tutorial state {currentState} saved.");
    }


    private void LoadTutorialState()
    {
        if (PlayerPrefs.HasKey(TutorialStateKey))
        {
            currentState = (TutorialState)PlayerPrefs.GetInt(TutorialStateKey);
            Debug.Log($"Loaded tutorial state: {currentState}");
        }
        else
        {
            if (testing)
                currentState = testState;
            else
                currentState = TutorialState.None;
        }
    }

    private TaskScript GetTaskForState(TutorialState state)
    {
        foreach (TaskScript task in tasks)
        {
            if (task != null && task.tutorialState == state)
            {
                return task;
            }
        }
        return null;
    }

    //public delegate void OnUpdateTutorialText(string txt);
    //public static event OnUpdateTutorialText onUpdateInstructionTextEvent;

    private void SetTutorialText(string headerText, string subTaskText)
    {
        Debug.Log(" headerText: " + headerText + " subTaskText: " + subTaskText);


        if (subTaskText != "")
            EventManager.DoFireAddSimpleTask(headerText, subTaskText);

        //  HelperFunctions.EnableInstruction(tutorialText);
    }


    public void UpdatePlayerPosition(Transform newPos)
    {
        EventManager.DoFireUpdatePlayerPosition(newPos);
    }

    
    public void UpdatePlayerPositionDisableMovement(Transform newPos)
    {
        EventManager.DoFireUpdatePlayerPositionDisableMovement(newPos);
    }


    public void EnablePlayerMovement()
    {
        EventManager.DoFireEnablePlayerMovement();
    }

    public void PurchaseShovelUpgrade()
    {
        EventManager.DoFirePurchaseShovelUpgrade();
    }

    public void EnableFPSPanel(bool enable)
    {
        EventManager.DoFireEnableFPSPanel(enable);
    }
 
}
