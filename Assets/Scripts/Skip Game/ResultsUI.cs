using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI wpsText;
    [SerializeField] private TextMeshProUGUI skipsText;
    [SerializeField] private TextMeshProUGUI totalwordsText;
    [SerializeField] private TextMeshProUGUI penalityText;
    [SerializeField] private Text wordsText;
    [SerializeField] private Animator animator;

    private void Start()
    {
        // Sub
        SkipGameEvents.instance.OnEnd += DisplayResults;
        SkipGameEvents.instance.OnRedo += HideResults;
        SkipGameEvents.instance.OnScoreChanged += UpdateScore;
    }

    private void OnDestroy()
    {
        // Sub
        SkipGameEvents.instance.OnEnd -= DisplayResults;
        SkipGameEvents.instance.OnRedo -= HideResults;
        SkipGameEvents.instance.OnScoreChanged -= UpdateScore;
    }

    private void DisplayResults(TurnData data)
    {
        // Show UI
        animator.Play("Show");

        // Set name
        playerText.text = $"Player: {data.playerData.name}";

        // Update text values
        scoreText.text = $"Score: {data.score} pts";

        correctText.text = $"Number of Guesses: {data.numCorrect}";

        int numberOfSkips = data.numWords - data.numCorrect;
        skipsText.text = $"Number of SKIPs: {numberOfSkips}";

        totalwordsText.text = $"Total Words Seen: {data.numWords}";

        float rate = (float)data.numCorrect / data.numWords * 100;
        wpsText.text = $"Accuracy: {(int)rate}%";

        penalityText.text = $"Number of Penalities: {data.numPenalties}";

        string encounteredWords = "";
        foreach (var pair in data.encounteredWords)
        {
            string tag = "";
            if (pair.Item2)
                tag = $"[C]";
            else
                tag = $"[S]";

            encounteredWords += $"{pair.Item1} {tag}\n";
        }
        wordsText.text = encounteredWords;
    }

    private void UpdateScore(int score, int change)
    {
        // Update text values
        scoreText.text = $"Score: {score} pts";
    }

    private void HideResults()
    {
        // Hide window;
        animator.Play("Hide");
    }
}
