using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistTaskItem : TaskUIItem
{
    public Text headerText;

    //public ChecklistItem_TextScript checkListTextPrefab;

    public TaskType type = TaskType.None;
    public int taskID = 0;
  
    public void Initialize(TaskType type, int id,  string textVal)
    {
        this.type = type;
        this.taskID = id;

        if (taskText)
            taskText.text = textVal;
    }

    public override void SetText(string text)
    {
        base.SetText(text);
    }
}
