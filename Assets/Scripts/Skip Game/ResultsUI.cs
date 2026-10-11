using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI wordsText;
    [SerializeField] private Animator animator;

    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset defaultFont;
    [SerializeField] private TMP_FontAsset armenianFont;

    [Header("Data")]
    [SerializeField] private ColorData colorData;

    private void Start()
    {
        // Sub
        SkipGameEvents.instance.OnSetPlayer += SetFont;
        SkipGameEvents.instance.OnEnd += DisplayResults;
        SkipGameEvents.instance.OnRedo += HideResults;
        SkipGameEvents.instance.OnScoreChanged += UpdateScore;
    }

    private void OnDestroy()
    {
        // Sub
        SkipGameEvents.instance.OnSetPlayer -= SetFont;
        SkipGameEvents.instance.OnEnd -= DisplayResults;
        SkipGameEvents.instance.OnRedo -= HideResults;
        SkipGameEvents.instance.OnScoreChanged -= UpdateScore;
    }

    private void SetFont(PlayerData playerData)
    {
        Language language = playerData.teamData.language;
        if (language == Language.Armenian)
            wordsText.font = armenianFont;
        else
            wordsText.font = defaultFont;
    }

    private void DisplayResults(TurnData data)
    {
        // Show UI
        animator.Play("Show");

        // Set name
        playerText.text = $"Player: {data.playerData.name}";

        // Update text values
        scoreText.text = $"Score: {data.score} pts";

        correctText.text = $"Guesses: {data.numCorrect}";

        int numberOfSkips = data.numWords - data.numCorrect;
        skipsText.text = $"SKEEPs: {numberOfSkips}";

        totalwordsText.text = $"Total Words: {data.numWords}";

        float rate = (float)data.numCorrect / data.numWords * 100;
        wpsText.text = $"Guess Rate: {(int)rate}%";

        penalityText.text = $"Penalities: {data.numPenalties}";

        string encounteredWords = "";
        foreach (var pair in data.encounteredWords)
        {
            string word = "";
            if (pair.Item2) // If guessed correctly
                word = $"<color={colorData.CorrectColorHex}>[C] {pair.Item1}</color>\n";
            else
                word = $"<color={colorData.SkipColorHex}>[S] {pair.Item1}</color>\n";

            encounteredWords += word;
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
