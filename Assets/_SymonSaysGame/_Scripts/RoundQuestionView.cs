using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoundQuestionView : MonoBehaviour
{
    [Header("References")]
    public Text questionText;
    public FlaskScript[] flasks;                  // replaces Buttons
    public Transform debugStripParent;
    public GameObject debugColorPrefab;
    public TMP_Text debugSequenceText;             // optional

    private Question currentQuestion;
    private List<ColorId> currentDebugSequence = new List<ColorId>();
    private HashSet<ColorId> currentSelections = new HashSet<ColorId>();

    #region Unity Lifecycle

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnQuestionReady += HandleQuestionReady;
            GameManager.Instance.OnSequenceShowComplete += HandleSequenceComplete;
        }

        EventManager.OnFlaskClicked += HandleFlaskClicked;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnQuestionReady -= HandleQuestionReady;
            GameManager.Instance.OnSequenceShowComplete -= HandleSequenceComplete;
        }

        EventManager.OnFlaskClicked -= HandleFlaskClicked;
    }

    #endregion

    #region Game Events

    private void HandleSequenceComplete(List<ColorId> seq)
    {
        currentDebugSequence = new List<ColorId>(seq);
        UpdateDebugStrip(seq);
    }

    private void HandleQuestionReady(Question q)
    {
        currentQuestion = q;
        currentSelections.Clear();

        // Debug sequence text (optional)
        if (debugSequenceText != null)
        {
            string dbg = "Debug: ";
            foreach (var c in currentDebugSequence)
            {
                dbg += $"[{GameManager.Instance.GetNameForColor(c)}] ";
            }
            debugSequenceText.text = dbg;
        }

        // Prompt
        if (questionText != null)
            questionText.text = q.prompt;

        int totalColors = GameManager.Instance.colorSprites.Length;

        // Disable all flasks first
        foreach (var flask in flasks)
        {
            if (flask == null) continue;
            flask.gameObject.SetActive(false);
            flask.SetInteractable(false);
            flask.Highlight(false);
            flask.EnableOutline(false);
        }

        // Initialize active flasks
        for (int i = 0; i < totalColors && i < flasks.Length; i++)
        {
            FlaskScript flask = flasks[i];
            if (flask == null) continue;

            ColorId id = (ColorId)i;
            Color color = GameManager.Instance.GetColorFromID(id);
            string name = GameManager.Instance.GetNameForColor(id);

            flask.gameObject.SetActive(true);
            flask.SetInteractable(true);
            flask.SetColor(id, color, name);
            flask.Highlight(false);
        }


    }

    #endregion

    private void HandleFlaskClicked(object obj)
    {
        if (currentQuestion == null) return;

        FlaskScript flask = obj as FlaskScript;

        OnFlaskClicked(flask);
    }

    #region Input (Called from FlaskScript)

        /// <summary>
        /// Called by FlaskScript when user clicks a flask
        /// </summary>
    public void OnFlaskClicked(FlaskScript flask)
    {
        if (currentQuestion == null || flask == null) return;

        ColorId clicked = flask.colorId;

        // Combined (multi-select) question
        if (currentQuestion.type == QuestionType.CombinedIndexQuestion)
        {
            if (currentSelections.Contains(clicked))
            {
                currentSelections.Remove(clicked);
                flask.Highlight(false);
            }
            else
            {
                currentSelections.Add(clicked);
                flask.Highlight(true);
            }

            if (currentSelections.Count == currentQuestion.correctAnswers.Count)
            {
                SubmitAnswers(new List<ColorId>(currentSelections));
            }
        }
        else
        {
            // Single-select question
            flask.Highlight(true);
            SubmitAnswers(new List<ColorId> { clicked });
        }
    }

    public void OutlineAllFlasks(bool highlight)
    {
        foreach (var flask in flasks)
        {
            flask.EnableOutline(highlight);
        }
    }

    #endregion

    #region Submission

    private void SubmitAnswers(List<ColorId> answers)
    {
        // Disable interaction after submit
        foreach (var flask in flasks)
        {
            if (flask != null)
                flask.SetInteractable(false);
        }

        GameManager.Instance.AnswerSubmitted(answers);
    }

    #endregion

    #region Debug UI

    private void UpdateDebugStrip(List<ColorId> seq)
    {
        if (debugStripParent == null || debugColorPrefab == null)
            return;

        for (int i = debugStripParent.childCount - 1; i >= 0; i--)
            Destroy(debugStripParent.GetChild(i).gameObject);

        foreach (var c in seq)
        {
            var go = Instantiate(debugColorPrefab, debugStripParent);
            var img = go.GetComponent<UnityEngine.UI.Image>();
            var col = GameManager.Instance.GetColorFromID(c);

            if (img != null)
                img.color = col;
        }
    }

    #endregion
}
