/*
RoundScreenView.cs
Attach to the Round Screen UI GameObject that displays the sequence while it's being shown.
Assign references in inspector for:

* bigColorImage: an Image used to display the current color sprite
* sequenceText: Text or TMPro for "Watch this sequence of colors: X / N"
* roundText: Text that shows "Round #"
* debugStripParent (optional): a container to show debug small circle images of colors
  */
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundScreenView : MonoBehaviour
{
    [Header("References")]
    public Image bigColorImage;
    public Text sequenceText;
    public Text roundText;
    public Transform debugStripParent;
    public GameObject debugColorPrefab; // small dot prefab (Image) for debug


private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRoundChanged += HandleRoundChanged;
            GameManager.Instance.OnSequenceShowProgress += HandleSequenceProgress;
            GameManager.Instance.OnSequenceShowComplete += HandleSequenceComplete;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRoundChanged -= HandleRoundChanged;
            GameManager.Instance.OnSequenceShowProgress -= HandleSequenceProgress;
            GameManager.Instance.OnSequenceShowComplete -= HandleSequenceComplete;
        }
    }

    private void HandleRoundChanged(int roundNumber)
    {
        if (roundText != null) roundText.text = $"{roundNumber}";
        if (bigColorImage != null) bigColorImage.enabled = false;
        if (sequenceText != null) sequenceText.text = "";
        ClearDebug();
    }

    private void HandleSequenceProgress(List<ColorId> shownPrefix, int total)
    {
        if (sequenceText != null) sequenceText.text = $"Watch this sequence of colors: {shownPrefix.Count} / {total}";

      

        // show last color in big image
        if (bigColorImage != null && shownPrefix.Count > 0)
        {
            var last = shownPrefix[shownPrefix.Count - 1];
            var clr = GameManager.Instance.GetColorFromID(last);
            if (clr != null)
            {
                bigColorImage.color = clr;
                bigColorImage.enabled = true;
            }
        }

        // If final rounds require falsified labels, ask GameManager
        int roundNumber = GameManager.Instance.CurrentRoundIndex;
        if (GameManager.Instance != null && GameManager.Instance.ShouldFalsifyLabelsThisRound(roundNumber))
        {
            FalsifySomeLabels();
        }

        UpdateDebugStrip(shownPrefix);
    }

    public Text falseLablesText;
    private System.Random rnd = new System.Random();
    private ColorId lastFalseLabelId = (ColorId)(-1);
    private ColorId lastFalseColorId = (ColorId)(-1);

    private void FalsifySomeLabels()
    {
        if (falseLablesText == null || bigColorImage == null) return;

        falseLablesText.gameObject.SetActive(true);

        // Get current big color
        ColorId currentColorId = GetColorIdFromColor(bigColorImage.color);
        int totalColors = Enum.GetNames(typeof(ColorId)).Length;

        // Build list of valid options (exclude current big color and last used)
        List<ColorId> otherIdsForLabel = Enumerable.Range(0, totalColors)
            .Select(i => (ColorId)i)
            .Where(i => i != currentColorId && i != lastFalseLabelId)
            .ToList();

        List<ColorId> otherIdsForColor = Enumerable.Range(0, totalColors)
            .Select(i => (ColorId)i)
            .Where(i => i != currentColorId && i != lastFalseColorId)
            .ToList();

        // Pick random label and color
        ColorId falseLabelId = otherIdsForLabel[rnd.Next(otherIdsForLabel.Count)];
        ColorId falseColorId = otherIdsForColor[rnd.Next(otherIdsForColor.Count)];

        // Assign
        falseLablesText.text = GameManager.Instance.GetNameForColor(falseLabelId);
        falseLablesText.color = GameManager.Instance.GetColorFromID(falseColorId);

        // Save for next call to avoid consecutive duplicates
        lastFalseLabelId = falseLabelId;
        lastFalseColorId = falseColorId;
    }

    /// <summary>
    /// Helper: find ColorId from actual Color
    /// </summary>
    private ColorId GetColorIdFromColor(Color color)
    {
        var colors = Enum.GetValues(typeof(ColorId)).Cast<ColorId>();
        foreach (var id in colors)
        {
            if (GameManager.Instance.GetColorFromID(id) == color)
                return id;
        }
        return 0; // fallback (Red)
    }

    //private void FalsifySomeLabels()
    //{
    //    falseLablesText.gameObject.SetActive(true);
    //    // Replace text labels randomly with other color names to create "false text" effect
    //    System.Random rnd = new System.Random();

    //    int otherLabel = rnd.Next(0, Enum.GetNames(typeof(ColorId)).Length);
    //    falseLablesText.text = GameManager.Instance.GetNameForColor((ColorId)otherLabel);

    //    int otherColor = rnd.Next(0, Enum.GetNames(typeof(ColorId)).Length);
    //    falseLablesText.color = GameManager.Instance.GetColorFromID((ColorId)otherColor);

    //}



    private void HandleSequenceComplete(List<ColorId> fullSequence)
    {
        // optionally clear the big image or leave the last shown
        // bigColorImage.enabled = false;
    }

    private void UpdateDebugStrip(List<ColorId> seq)
    {
        ClearDebug();
        if (debugStripParent == null || debugColorPrefab == null) return;
        foreach (var c in seq)
        {
            var go = Instantiate(debugColorPrefab, debugStripParent);
            var img = go.GetComponent<Image>();
            var spr = GameManager.Instance.GetColorFromID(c);
            if (spr != null && img != null) img.color = spr;
        }
    }

    private void ClearDebug()
    {
        if (debugStripParent == null) return;
        for (int i = debugStripParent.childCount - 1; i >= 0; i--) Destroy(debugStripParent.GetChild(i).gameObject);
    }


}
