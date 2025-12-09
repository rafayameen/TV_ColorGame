using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIHandler : MonoBehaviour
{
    public Text taskHeaderText; // Header for task list
    public RectTransform content; // Parent transform for task items
    public SimpleTaskUIItem simpleItemPrefab; // Prefab for individual task items
    public ChecklistTaskItem checklistItemPrefab; // Prefab for checklist task items

    private List<TutorialTask> tutorialTasks = new List<TutorialTask>(); // List of tutorial tasks
    private int currentTaskIndex = 0; // Index of the current task

    string headerTextString = "";
    public GameObject taskParentUI; // Parent UI object for task display

    int completedTaskIndex = 0;

    public Sprite taskCompleteSprite;
    private void OnEnable()
    {
        // Subscribe to the event
        EventManager.onAddSimpleTask += AddSimpleTask;
        EventManager.onAddCheckListTask += AddChecklistTask;
        EventManager.tutorialTaskComplete += CompleteCurrentTask;
        EventManager.OnTaskProgressUpdatedEvent += UpdateChecklistTaskProgress;

        EventManager.onResetTaskUI_CompletedTasks += ResetCompleteTasksCount;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        EventManager.onAddSimpleTask -= AddSimpleTask;
        EventManager.onAddCheckListTask -= AddChecklistTask;
        EventManager.tutorialTaskComplete -= CompleteCurrentTask;
        EventManager.OnTaskProgressUpdatedEvent -= UpdateChecklistTaskProgress;

        EventManager.onResetTaskUI_CompletedTasks -= ResetCompleteTasksCount;

    }

    private void ResetCompleteTasksCount()
    {
        completedTaskIndex = 0;

        //TutorialManager.instance.HandleInitialTask();
    }

    private int GetCompletedTaskIndex()
    {
        return completedTaskIndex;
    }



    int currentCheckListIndex = 0;

    private void UpdateChecklistTaskProgress(TaskType taskType, int increment)
    {
        if (tutorialTasks.Count == 0 || currentCheckListIndex >= tutorialTasks.Count)
            return;

        var currentTask = tutorialTasks[currentCheckListIndex];

        if (currentTask.tutorialTaskType != TutorialTaskType.Checklist)
            return;

        foreach (var checklistTask in currentTask.checkListTasks)
        {
            if (checklistTask.taskType == taskType)
            {
                checklistTask.currentProgress += increment;

                foreach (Transform child in content)
                {
                    var checklistItem = child.GetComponent<ChecklistTaskItem>();

                    if (checklistItem && checklistItem.type == taskType && checklistItem.taskID == checklistTask.taskID)
                    {
                        checklistItem.SetText(currentTask.TutorialText + " " + checklistTask.currentProgress + "/" + checklistTask.requiredProgress);
                        checklistItem.SetPercentage((float)checklistTask.currentProgress / checklistTask.requiredProgress);
                    }
                }
            }
        }

        // Check if all checklist tasks in the current task are completed
        if (currentTask.checkListTasks.All(t => t.currentProgress >= t.requiredProgress))
        {
            CompleteChecklistTask(taskType);
            //currentCheckListIndex++; // Move to the next checklist task
        }
    }


    private void CompleteChecklistTask(TaskType taskType)
    {
        completedTaskIndex++;

        var completedTask = tutorialTasks.FirstOrDefault(t => t.checkListTasks.Any(ct => ct.taskType == taskType));

        if (completedTask == null)
            return;

        foreach (Transform child in content)
        {
            var checklistItem = child.GetComponent<ChecklistTaskItem>();
            if (checklistItem && checklistItem.type == taskType)
            {
                checklistItem.MarkCompleted(taskCompleteSprite);
                Destroy(child.gameObject);
                break;
            }
        }

        tutorialTasks.Remove(completedTask);

        EventManager.DoFireSubStateComplete(completedTaskIndex);

        if (tutorialTasks.Count > 0)
        {
            currentTaskIndex = Mathf.Clamp(currentTaskIndex, 0, tutorialTasks.Count - 1);
            UpdateTaskHeader();
        }
        else
        {
            taskHeaderText.text = "All Tasks Completed!";
            EventManager.DoFireAllTasksUI_TaskComplete();
            currentTaskIndex = 0;
        }

        //EventManager.DoFireSubStateComplete(completedTaskIndex);
        UpdateTaskParentVisibility();
    }




    public void AddSimpleTask(string mainHeaderText, string subTaskText)
    {
        // Add a new simple task
        AddTask(new SimpleTask(mainHeaderText, subTaskText));

        headerTextString = mainHeaderText;
        // Update UI visibility and header
        UpdateTaskHeader();
        UpdateTaskParentVisibility();
    }

    public void AddTask(SimpleTask task)
    {
        tutorialTasks.Add(new TutorialTask
        {
            tutorialTaskType = TutorialTaskType.Simple,
            mainTaskHeaderText = task.MainTaskName,
            TutorialText = task.SubTaskName
        });

        // Instantiate a new UI item for the simple task
        SimpleTaskUIItem taskItem = Instantiate(simpleItemPrefab, content);
        taskItem.SetText(task.SubTaskName);

        // Update UI visibility
        UpdateTaskParentVisibility();
    }

    public void AddChecklistTask(string headerString, TutorialTask checklistTask)
    {
        headerTextString = headerString;

        UpdateTaskHeader();
        tutorialTasks.Add(checklistTask);

        // Instantiate a new UI item for the checklist task
        ChecklistTaskItem checklistItem = Instantiate(checklistItemPrefab, content);
        //checklistItem.headerText.text = checklistTask.mainTaskHeaderText;

        // Add checklist items dynamically
        foreach (var taskType in checklistTask.checkListTasks)
        {
            //ChecklistItem_TextScript checklistText = Instantiate(checklistItem.checkListTextPrefab, checklistItem.transform);
            checklistItem.Initialize(taskType.taskType,taskType.taskID, checklistTask.TutorialText + " " + taskType.currentProgress + "/" + taskType.requiredProgress);

        }

        // Update UI visibility
        UpdateTaskParentVisibility();
    }

    public void CompleteCurrentTask()
    {
        completedTaskIndex++;

        int index = completedTaskIndex;

        if (tutorialTasks.Count == 0 || currentTaskIndex >= tutorialTasks.Count)
        {
            Debug.LogWarning("No tasks to complete!");
            return;
        }

        // Remove the current task from the list and content
        Transform taskToRemove = content.GetChild(currentTaskIndex);

        SimpleTaskUIItem st = taskToRemove.GetComponent<SimpleTaskUIItem>();

        if (st)
            st.MarkCompleted(taskCompleteSprite);

        //Destroy(taskToRemove.gameObject);

        tutorialTasks.RemoveAt(currentTaskIndex);

        EventManager.DoFireSubStateComplete(index);
        // Update the header to the next task or clear it if no tasks are left
        if (tutorialTasks.Count > 0)
        {

            currentTaskIndex = Mathf.Clamp(currentTaskIndex, 0, tutorialTasks.Count - 1);
            UpdateTaskHeader();
        }
        else
        {


            HelperFunctions.DestroyAllChildren(content);

            taskHeaderText.text = "All Tasks Completed!";

            completedTaskIndex = 0;

            EventManager.DoFireAllTasksUI_TaskComplete();

        }

        // Update UI visibility
        UpdateTaskParentVisibility();
    }

    private void UpdateTaskHeader()
    {

        taskHeaderText.text = headerTextString;


    }

    private void UpdateTaskParentVisibility()
    {
        // Enable the taskParentUI if there are tasks; otherwise, disable it
        taskParentUI.SetActive(tutorialTasks.Count > 0);
    }
}


[System.Serializable]
public class SimpleTask
{
    public string MainTaskName; // Name of the main task
    public string SubTaskName; // Name of the subtask

    public SimpleTask(string mainTaskName, string subTaskName)
    {
        MainTaskName = mainTaskName;
        SubTaskName = subTaskName;
    }
}
