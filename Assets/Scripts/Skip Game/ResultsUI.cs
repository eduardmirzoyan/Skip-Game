using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI wpsText;
    [SerializeField] private TextMeshProUGUI skipsText;
    [SerializeField] private TextMeshProUGUI totalwordsText;
    [SerializeField] private Animator animator;

    private void Start()
    {
        // Sub
        SkipGameEvents.instance.onEnd += DisplayResults;
        SkipGameEvents.instance.onRedo += HideResults;
        
    }

    private void OnDestroy()
    {
        // Sub
        SkipGameEvents.instance.onEnd -= DisplayResults;
        SkipGameEvents.instance.onRedo += HideResults;
    }

    private void DisplayResults(int score, int numberOfCorrect, int numberOfWords, float duration, PlayerData playerData)
    {
        // Show UI
        animator.Play("Show");

        // Set name
        playerText.text = "Player: " + playerData.name;

        // Update text values
        scoreText.text = "Score: " + score + " pts";

        correctText.text = "Number of Guesses: " + numberOfCorrect;

        int numberOfSkips = numberOfWords - numberOfCorrect;
        skipsText.text = "Number of SKIPs: " + numberOfSkips;
        
        totalwordsText.text = "Total Words Seen: " + numberOfWords;

        float wps = numberOfCorrect / duration;
        wps = Mathf.Round(wps * 100f) / 100f;
        wpsText.text = "Guesses Per Second: " + wps;
    }

    private void HideResults()
    {
        // Hide window;
        animator.Play("Hide");
    }
}
