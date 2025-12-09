
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GameState
{
    Tutorial, None
}

public enum TaskType
{
    Collect_Ores,
    None
}

public enum TutorialTaskType
{
    Simple,
    Checklist
}

[System.Serializable]
public class TaskScript
{
    public GameState state = GameState.None;
    public TutorialState tutorialState = TutorialState.None;

    [TextArea]
    public string mainTaskHeaderText; // Main heading for the task
    public List<TutorialTask> tutorialTasks = new List<TutorialTask>(); // List of tutorial tasks

    public void EnableObjects(bool enable)
    {
        foreach (var task in tutorialTasks)
        {
            task.EnableObjects(enable);
        }
    }

    public void EnableObjects(int index, bool enable)
    {
        if (index > tutorialTasks.Count)
            return;


        tutorialTasks[index].EnableObjects(enable);

    }
}

[System.Serializable]
public class CheckListTask
{
    public TaskType taskType = TaskType.None;
    public int taskID = 0;

    public int currentProgress = 0;
    public int requiredProgress = 4;
}

[System.Serializable]
public class TutorialTask
{
    public TutorialTaskType tutorialTaskType = TutorialTaskType.Simple; // Type of the tutorial task


    public List<CheckListTask> checkListTasks = new List<CheckListTask>(); // List of task types for the tutorial


    [TextArea]
    public string TutorialText; // Tutorial text description
    public string mainTaskHeaderText; // Header text for the main task

    public List<GameObject> objectsToEnableList; // Objects to enable/disable during this task

    public bool useButtonForNextTask = false; // Whether to use a button to complete the task
    public Button taskCompleteButton; // Button for task completion if required

    public UnityEvent onStartEvent;
    public UnityEvent onCompleteEvent;
    public void EnableObjects(bool enable)
    {
        if (enable)
            onStartEvent?.Invoke();
        else
            onCompleteEvent?.Invoke();

        foreach (var item in objectsToEnableList)
        {
            if (item)
                item.SetActive(enable);
        }
    }
}

