using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistItem_TextScript : MonoBehaviour
{
    public TaskType type = TaskType.None;

    public Text text;

    public void Initialize(TaskType type, string textVal)
    {
        this.type = type;
        if (text)
            text.text = textVal;
    }

    public void SetText(string newText)
    {
        if (text)
            text.text = newText;
    }
}
