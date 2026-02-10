/*
GameManager.cs
Central game flow: sequence generation, showing sequence, question generation, scoring, and events.
Attach to an empty GameObject named "GameManager".
*/
using DG.Tweening;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum ColorId { Red = 0, Yellow = 1, Green = 2, Blue = 3, Purple = 4 }

public enum QuestionType
{
    IndexQuestion,
    WhichNotShown,
    WhichShownTwice,
    BeforeAfterQuestion,
    CombinedIndexQuestion
}

[Serializable]
public class Question
{
    public QuestionType type;
    public string prompt;
    public List<ColorId> correctAnswers;
    public List<ColorId> presentedChoices;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int SeqLength { get => seqLength; set => seqLength = value; }
    public int CurrentRoundIndex { get => currentRoundIndex; set => currentRoundIndex = value; }

    // ------------------------------------------------------
    //                  UI REFERENCES  (ADDED)
    // ------------------------------------------------------

    [Header("UI Screens")]  // <<< ADDED
    public GameObject homeScreen;  // <<< ADDED
    public GameObject roundScreen; // <<< ADDED
    public GameObject questionScreen; // <<< ADDED

    public List<GameObject> gameplayObjects;

    // ------------------------------------------------------

    [Header("General")]
    [Tooltip("Seconds between showing each color during the sequence")]
    public float delayBetweenColors = 2f;

    [Tooltip("Seconds per round timer")]
    public float secondsPerRound = 60f;

    [Tooltip("Points awarded when a round is answered correctly")]
    public int pointsPerRound = 10;

    [Tooltip("Mapping sprites (in this order): Red, Yellow, Green, Blue, Purple")]
    public Color[] colorSprites = new Color[5];

    [Tooltip("Color names (in same order)")]
    public string[] colorNames = new string[5] { "red", "yellow", "green", "blue", "purple" };

    [Header("Round distribution (default: 5,6,6,7,7,8,8,9,9,10)")]
    public List<int> sequenceLengths = new List<int> { 5, 6, 6, 7, 7, 8, 8, 9, 9, 10 };

    [Header("Final rounds label fuzzing")]
    public bool showFalseLabelsInFinalRounds = true;
    [Range(0f, 1f)]
    public float labelFalsingChance = 0.35f;

    public event Action<int> OnRoundChanged;
    public event Action<float, float> OnTimerUpdated;
    public event Action<List<ColorId>, int> OnSequenceShowProgress;
    public event Action<List<ColorId>> OnSequenceShowComplete;
    public event Action<Question> OnQuestionReady;
    public event Action<int> OnScoreUpdated;
    public event Action<bool> OnRoundComplete;

    private int currentRoundIndex = -1;
    private List<ColorId> currentSequence = new List<ColorId>();
    private int score = 0;
    private Coroutine runningSequenceCoroutine;
    private Coroutine runningTimerCoroutine;

    public Text scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;

        //if (colorNames == null || colorNames.Length != 5)
        //    colorNames = new string[5] { "red", "yellow", "green", "blue", "purple" };
    }

    // =======================================================
    //         UI FLOW HELPERS (ADDED)
    // =======================================================


    void EnableGameplayObjects(bool enable)
    {
        foreach (var item in gameplayObjects)
        {
            if (item) item.SetActive(enable);
        }
    }

    private void ShowHomeScreen()   // <<< ADDED
    {
        EnableGameplayObjects(false);

        homeScreen.SetActive(true);
        roundScreen.SetActive(false);
        questionScreen.SetActive(false);
    }

    private void ShowRoundScreen()  // <<< ADDED
    {
        homeScreen.SetActive(false);
        roundScreen.SetActive(true);
        questionScreen.SetActive(false);
    }

    private void ShowQuestionScreen()  // <<< ADDED
    {
        homeScreen.SetActive(false);
        roundScreen.SetActive(false);
        questionScreen.SetActive(true);
    }

    // =======================================================

    #region Public API (UI calls into these)

    public void StartGame()
    {
        combinedIndexQuestionCount = 0;
        score = 0;
        CurrentRoundIndex = -1;
        OnScoreUpdated?.Invoke(score);

        EnableGameplayObjects(true);
        ShowRoundScreen();

        StartNextRound();
    }


    //public void StartGame()
    //{
    //    score = 0;
    //    CurrentRoundIndex = -1;
    //    OnScoreUpdated?.Invoke(score);

    //    EnableGameplayObjects(true);

    //    ShowRoundScreen();  // <<< ADDED

    //    StartNextRound();
    //}

    public void StartNextRound()
    {
        CurrentRoundIndex++;
        if (CurrentRoundIndex >= sequenceLengths.Count)
        {
            Debug.Log("Game over. Final score: " + score);

            // <<< ADDED → return to home screen
            ShowHomeScreen();

            return;
        }

        OnRoundChanged?.Invoke(CurrentRoundIndex + 1);
        StartCoroutine(RunRound(CurrentRoundIndex));
    }

    public GameObject feedbackObj;
    public Text answerCorrectWrongText;

    public GameObject correct, wrong;

    public void ShowFeedback()
    {
        feedbackObj.SetActive(true);

        DOVirtual.DelayedCall(1.5f, () => feedbackObj.SetActive(false));
    }


    public void AnswerSubmitted(List<ColorId> submittedAnswers)
    {
        bool allCorrect = false;

        if (lastQuestion == null)
        {
            Debug.LogWarning("No question to validate.");
            return;
        }

        var correctSet = new HashSet<ColorId>(lastQuestion.correctAnswers);
        var submittedSet = new HashSet<ColorId>(submittedAnswers);
        allCorrect = correctSet.SetEquals(submittedSet);

        correct.SetActive(false);
        wrong.SetActive(false);

        DOVirtual.DelayedCall(1, () =>
        {
            if (allCorrect)
            {
                correct.SetActive(true);

                score += pointsPerRound;
                OnScoreUpdated?.Invoke(score);

                answerCorrectWrongText.color = Color.green;
                answerCorrectWrongText.text = "Correct!";

            }
            else
            {
                wrong.SetActive(true);

                answerCorrectWrongText.color = Color.red;
                answerCorrectWrongText.text = "Wrong!";
            }
            if (scoreText)
                scoreText.text = score + "/100";

            ShowFeedback();

            questionScreen.SetActive(false);

            OnRoundComplete?.Invoke(allCorrect);

            if (runningTimerCoroutine != null) StopCoroutine(runningTimerCoroutine);

            // <<< ADDED → after answering, return to Round Screen for next round
            StartCoroutine(GoToNextRoundFlow());

          //  StartCoroutine(ProceedToNextRoundAfterDelay(nextRoundDelay));

        });

   
    }

    public float nextRoundDelay = 1.5f;

    private IEnumerator GoToNextRoundFlow()   // <<< ADDED
    {
        yield return new WaitForSeconds(nextRoundDelay);
        ShowRoundScreen();
        StartNextRound();
    }


    public Color GetColorFromID(ColorId id)
    {
        if (colorSprites == null || colorSprites.Length == 0)
            return Color.white;

        int i = (int)id;
        i = i % colorSprites.Length; // wrap around if index > array length
        return colorSprites[i];
    }

    public string GetNameForColor(ColorId id)
    {
        if (colorNames == null || colorNames.Length == 0)
            return id.ToString();

        int i = (int)id;
        i = i % colorNames.Length; // wrap around if index > array length
        return colorNames[i];
    }


    //public Color GetColorFromID(ColorId id)
    //{
    //    int i = (int)id;
    //    if (colorSprites != null && i >= 0 && i < colorSprites.Length) return colorSprites[i];
    //    return Color.white;
    //}

    //public string GetNameForColor(ColorId id)
    //{
    //    int i = (int)id;
    //    if (colorNames != null && i >= 0 && i < colorNames.Length) return colorNames[i];
    //    return id.ToString();
    //}

    #endregion

    #region Internal Round Flow

    private Question lastQuestion;

    int seqLength = 0;
    private IEnumerator RunRound(int roundIndex)
    {
        ShowRoundScreen();   // <<< ADDED (Always show round screen during sequence)

        SeqLength = sequenceLengths[roundIndex];
        currentSequence = GenerateRandomSequence(SeqLength);

        if (runningTimerCoroutine != null) StopCoroutine(runningTimerCoroutine);
        runningTimerCoroutine = StartCoroutine(RoundTimerCoroutine(secondsPerRound));

        if (runningSequenceCoroutine != null) StopCoroutine(runningSequenceCoroutine);
        runningSequenceCoroutine = StartCoroutine(ShowSequenceCoroutine(currentSequence));

        while (runningSequenceCoroutine != null) yield return null;

        OnSequenceShowComplete?.Invoke(new List<ColorId>(currentSequence));

        // Switch to Question Screen BEFORE sending question
        ShowQuestionScreen();  // <<< ADDED

        lastQuestion = GenerateQuestionForSequence(currentSequence, roundIndex + 1);
        OnQuestionReady?.Invoke(lastQuestion);

        while (runningTimerCoroutine != null) yield return null;
    }

    private IEnumerator ProceedToNextRoundAfterDelay(float t)
    {
        yield return new WaitForSeconds(t);
        StartNextRound();
    }

    private IEnumerator ShowSequenceCoroutine(List<ColorId> sequence)
    {
        List<ColorId> shownPrefix = new List<ColorId>();
        int total = sequence.Count;
        for (int i = 0; i < total; i++)
        {
            shownPrefix.Add(sequence[i]);
            OnSequenceShowProgress?.Invoke(new List<ColorId>(shownPrefix), total);
            yield return new WaitForSeconds(delayBetweenColors);
        }
        runningSequenceCoroutine = null;
    }

    private IEnumerator RoundTimerCoroutine(float seconds)
    {
        float t = seconds;
        while (t > 0f)
        {
            OnTimerUpdated?.Invoke(t, seconds);
            yield return new WaitForSeconds(0.2f);
            t -= 0.2f;
        }
        OnTimerUpdated?.Invoke(0f, seconds);
        runningTimerCoroutine = null;

        OnRoundComplete?.Invoke(false);

        // <<< ADDED → timeout = go back to round screen then next round
        ShowRoundScreen();

        StartCoroutine(ProceedToNextRoundAfterDelay(1f));
    }

    #endregion

    #region Sequence and Question Generation

    private List<ColorId> GenerateRandomSequence(int length)
    {
        var rnd = new System.Random();
        var seq = new List<ColorId>();
        var possible = Enum.GetValues(typeof(ColorId)).Cast<ColorId>().ToArray();

        ColorId lastColor = (ColorId)(-1); // sentinel value

        for (int i = 0; i < length; i++)
        {
            ColorId next;
            do
            {
                next = possible[rnd.Next(possible.Length)];
            } while (next == lastColor && possible.Length > 1); // avoid consecutive duplicates

            seq.Add(next);
            lastColor = next;
        }

        return seq;
    }


    //private List<ColorId> GenerateRandomSequence(int length)
    //{
    //    var rnd = new System.Random();
    //    var seq = new List<ColorId>();
    //    var possible = Enum.GetValues(typeof(ColorId)).Cast<ColorId>().ToArray();
    //    for (int i = 0; i < length; i++)
    //    {
    //        seq.Add(possible[rnd.Next(possible.Length)]);
    //    }
    //    return seq;
    //}

    private int combinedIndexQuestionCount = 0;

    private Question GenerateQuestionForSequence(List<ColorId> seq, int roundNumber)
    {
        var rnd = new System.Random();

        var counts = seq.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

        var missing = counts.Where(kv => kv.Value == 0).Select(kv => kv.Key).ToList();
        var exactlyTwice = counts.Where(kv => kv.Value == 2).Select(kv => kv.Key).ToList();

        List<QuestionType> candidates = new List<QuestionType>()
    {
        QuestionType.IndexQuestion,
        QuestionType.IndexQuestion,
        QuestionType.BeforeAfterQuestion
    };

        // Only allow CombinedIndexQuestion max twice
        if (combinedIndexQuestionCount < 2)
            candidates.Add(QuestionType.CombinedIndexQuestion);

        if (missing.Count > 0) candidates.Add(QuestionType.WhichNotShown);
        if (exactlyTwice.Count > 0) candidates.Add(QuestionType.WhichShownTwice);

        QuestionType chosen = candidates[rnd.Next(candidates.Count)];

        Question q = new Question();
        q.presentedChoices = Enum.GetValues(typeof(ColorId)).Cast<ColorId>().ToList();

        switch (chosen)
        {
            case QuestionType.WhichNotShown:
                q.type = chosen;
                q.prompt = "Which color was not shown?";
                q.correctAnswers = new List<ColorId>() { missing[rnd.Next(missing.Count)] };
                break;

            case QuestionType.WhichShownTwice:
                q.type = chosen;
                q.prompt = "Which color showed up twice?";
                // Pick only one from the list of colors appearing exactly twice
                q.correctAnswers = new List<ColorId>() { exactlyTwice[rnd.Next(exactlyTwice.Count)] };
                break;
            //case QuestionType.WhichShownTwice:
            //    q.type = chosen;
            //    q.prompt = "Which color showed up twice?";
            //    q.correctAnswers = new List<ColorId>() { exactlyTwice[rnd.Next(exactlyTwice.Count)] };
            //    break;

            case QuestionType.IndexQuestion:
                q.type = chosen;
                int index = rnd.Next(seq.Count);
                q.prompt = $"What was the {Nth(index + 1)} color?";
                q.correctAnswers = new List<ColorId>() { seq[index] };
                break;

            case QuestionType.BeforeAfterQuestion:
                q.type = chosen;

                // find unique colors (count == 1)
                var colorCounts = seq.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
                var uniqueIndices = seq
                    .Select((c, i) => new { Color = c, Index = i })
                    .Where(x => colorCounts[x.Color] == 1)
                    .Select(x => x.Index)
                    .ToList();

                if (uniqueIndices.Count == 0)
                {
                    // fallback: pick first color
                    int idx = 0;
                    q.prompt = $"What was the {Nth(idx + 1)} color?";
                    q.correctAnswers = new List<ColorId>() { seq[idx] };
                }
                else
                {
                    int pos = uniqueIndices[rnd.Next(uniqueIndices.Count)];

                    bool askAfter = rnd.NextDouble() > 0.5;

                    if (askAfter && pos < seq.Count - 1)
                    {
                        q.prompt = $"Which color came after {GetName(seq[pos])}?";
                        q.correctAnswers = new List<ColorId>() { seq[pos + 1] };
                    }
                    else if (!askAfter && pos > 0)
                    {
                        q.prompt = $"Which color came before {GetName(seq[pos])}?";
                        q.correctAnswers = new List<ColorId>() { seq[pos - 1] };
                    }
                    else
                    {
                        // fallback if unique color is at start/end
                        q.prompt = $"What was the {Nth(pos + 1)} color?";
                        q.correctAnswers = new List<ColorId>() { seq[pos] };
                    }
                }
                break;

            case QuestionType.CombinedIndexQuestion:
                combinedIndexQuestionCount++; // increment counter
                                              // existing logic for picking two colors
                int idx1 = rnd.Next(seq.Count);
                int idx2 = rnd.Next(seq.Count);
                while (idx2 == idx1 && seq.Count > 1) idx2 = rnd.Next(seq.Count);
                int a = Math.Min(idx1, idx2);
                int b = Math.Max(idx1, idx2);
                q.type = chosen;
                q.prompt = $"Which colors were the {Nth(a + 1)} and {Nth(b + 1)} colors? (Select two)";
                q.correctAnswers = new List<ColorId>() { seq[a], seq[b] };
                break;
            //case QuestionType.CombinedIndexQuestion:
            //    q.type = chosen;
            //    int idx1 = rnd.Next(seq.Count);
            //    int idx2 = rnd.Next(seq.Count);
            //    while (idx2 == idx1 && seq.Count > 1) idx2 = rnd.Next(seq.Count);
            //    int a = Math.Min(idx1, idx2);
            //    int b = Math.Max(idx1, idx2);
            //    q.prompt = $"Which colors were the {Nth(a + 1)} and {Nth(b + 1)} colors? (Select two)";
            //    q.correctAnswers = new List<ColorId>() { seq[a], seq[b] };
            //    break;

            default:
                q.type = QuestionType.IndexQuestion;
                q.prompt = $"What was the {Nth(1)} color?";
                q.correctAnswers = new List<ColorId>() { seq[0] };
                break;
        }

        return q;
    }

    private string GetName(ColorId id) => GetNameForColor(id);

    private string Nth(int n)
    {
        if (n % 100 >= 11 && n % 100 <= 13) return n + "th";
        switch (n % 10)
        {
            case 1: return n + "st";
            case 2: return n + "nd";
            case 3: return n + "rd";
            default: return n + "th";
        }
    }

    /// <summary>
    /// Testing version: allows falsified labels from the first round
    /// </summary>
    //public bool ShouldFalsifyLabelsThisRound_Test(int roundOneBased)
    //{
    //    return true;
    //}


    public bool ShouldFalsifyLabelsThisRound(int roundOneBased)
    {
        if (!showFalseLabelsInFinalRounds) return false;
        int totalRounds = sequenceLengths.Count;
        return roundOneBased > totalRounds - 3 && UnityEngine.Random.value < labelFalsingChance;
    }

    #endregion
}
