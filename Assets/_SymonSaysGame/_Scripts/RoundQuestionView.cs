/*
RoundQuestionView.cs
Attach to the Question UI panel. Provide:

* debugStripParent + prefabs to render the debug shown-colors
* questionText (TMP)
* answerButtons: array of Buttons (one per color). The script maps each button to a color index in ColorId's order.
* labelText components under each button if you want to show/hide falsified labels (optional)
  */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundQuestionView : MonoBehaviour
{
    [Header("References")]
    public Text questionText;
    public Button[] answerButtons; // length should match number of colors (5)
    public Text[] answerButtonLabelTexts; // optional label components per button if you want to show/hide or falsify text
    public Transform debugStripParent;
    public GameObject debugColorPrefab;
    public Text debugSequenceText; // optional


private Question currentQuestion;
    private List<ColorId> currentDebugSequence = new List<ColorId>();
    private HashSet<ColorId> currentSelections = new HashSet<ColorId>();

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnQuestionReady += HandleQuestionReady;
            GameManager.Instance.OnSequenceShowComplete += HandleSequenceComplete;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnQuestionReady -= HandleQuestionReady;
            GameManager.Instance.OnSequenceShowComplete -= HandleSequenceComplete;
        }
    }

    private void Start()
    {
        // wire button callbacks
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int idx = i; // local copy
            answerButtons[i].onClick.AddListener(() => OnAnswerButtonClicked(idx));
        }

        
    }

   

    private void HandleSequenceComplete(List<ColorId> seq)
    {
        currentDebugSequence = new List<ColorId>(seq);
        UpdateDebugStrip(seq);
    }

    private void HandleQuestionReady(Question q)
    {
        currentQuestion = q;
        currentSelections.Clear();
        // Show debug sequence text optionally
        if (debugSequenceText != null)
        {
            string dbg = "Debug: ";
            if (currentDebugSequence.Count > 0)
            {
                for (int i = 0; i < currentDebugSequence.Count; i++)
                {
                    dbg += $"[{GameManager.Instance.GetNameForColor(currentDebugSequence[i])}] ";
                }
            }
            debugSequenceText.text = dbg;
        }

        foreach (var item in answerButtons)
        {
            if (item)
            {
                item.gameObject.SetActive(false);
                item.GetComponent<Outline>().enabled = false;
            }
        }
        // Show prompt
        if (questionText != null) questionText.text = q.prompt;

        int currentSeqLength = GameManager.Instance.colorSprites.Length; //total colors

        // Reset visuals of buttons
        for (int i = 0; i < currentSeqLength; i++)
        {
            var img = answerButtons[i].GetComponent<Image>();
            var sp = GameManager.Instance.GetColorFromID((ColorId)i);
            if (img != null && sp != null) img.color = sp;

            // reset label text to actual name by default
            if (answerButtonLabelTexts != null && i < answerButtonLabelTexts.Length && answerButtonLabelTexts[i] != null)
            {
                answerButtonLabelTexts[i].text = GameManager.Instance.GetNameForColor((ColorId)i);
            }
            // reset button color/selection highlight (optional)
            answerButtons[i].interactable = true;
            answerButtons[i].gameObject.SetActive(true);
        }

   
    }

    private int GetCurrentRoundNumber()
    {
        // round is not directly stored in view; try to deduce from GameManager event (it invokes OnRoundChanged earlier)
        // For simplicity assume the GameManager fired OnRoundChanged before OnQuestionReady.
        // If you need exact round number expose it in Question or as GameManager property.
        // We'll simply check currently displayed round text in scene (if available), else 1
        return 1;
    }

  

    private void UpdateDebugStrip(List<ColorId> seq)
    {
        if (debugStripParent == null || debugColorPrefab == null) return;
        for (int i = debugStripParent.childCount - 1; i >= 0; i--) Destroy(debugStripParent.GetChild(i).gameObject);
        foreach (var c in seq)
        {
            var go = Instantiate(debugColorPrefab, debugStripParent);
            var img = go.GetComponent<Image>();
            var spr = GameManager.Instance.GetColorFromID(c);
            if (spr != null && img != null) img.color = spr;
        }
    }

    private void OnAnswerButtonClicked(int buttonIndex)
    {
        if (currentQuestion == null) return;
        ColorId clicked = (ColorId)buttonIndex;


        answerButtons[buttonIndex].GetComponent<Outline>().enabled = true;

        // Toggle selection for combined index question; for single-answer types submit immediately
        if (currentQuestion.type == QuestionType.CombinedIndexQuestion)
        {
            if (currentSelections.Contains(clicked)) currentSelections.Remove(clicked);
            else currentSelections.Add(clicked);

            

            // Visual toggle (you can add better visuals)
            // If two selected, auto-submit
            if (currentSelections.Count == currentQuestion.correctAnswers.Count)
            {
                SubmitAnswers(new List<ColorId>(currentSelections));
            }
        }
        else
        {
            SubmitAnswers(new List<ColorId> { clicked });
        }
    }

    private void SubmitAnswers(List<ColorId> answers)
    {
        // disable buttons to prevent double submissions
        foreach (var b in answerButtons) b.interactable = false;
        // call GameManager
        GameManager.Instance.AnswerSubmitted(answers);
    }


}
