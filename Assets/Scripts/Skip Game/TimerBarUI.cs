using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerBarUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image barImage;
    [SerializeField] private TextMeshProUGUI numericalText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject popupTextPrefab;
    [SerializeField] private TextMeshProUGUI turnRuleText;

    [Header("Data")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private Color skipColor;
    [SerializeField] private Color correctColor;
    [SerializeField] private Vector2 popupVariance;

    private void Start()
    {
        // Sub
        SkipGameEvents.instance.OnTimeChanged += UpdateBar;
        SkipGameEvents.instance.OnScoreChanged += UpdateScore;
        SkipGameEvents.instance.OnSetTurnRule += UpdateTurnRule;
    }

    private void OnDestroy()
    {
        // Sub
        SkipGameEvents.instance.OnTimeChanged -= UpdateBar;
        SkipGameEvents.instance.OnScoreChanged -= UpdateScore;
        SkipGameEvents.instance.OnSetTurnRule -= UpdateTurnRule;
    }

    private void UpdateBar(float remaining, float duration)
    {
        // Goes from 0 -> 1
        float ratio = remaining / duration;

        // Set fill amount
        barImage.fillAmount = ratio;

        // Change color based on fill
        barImage.color = Color.Lerp(endColor, startColor, ratio);

        int rem = (int)remaining;
        // Change numerical display
        numericalText.text = "" + rem;

    }

    private void UpdateScore(int score, int change)
    {
        // Update display
        pointsText.text = score + " pts";

        // Get a random offset
        Vector3 offset = new Vector2(Random.Range(-popupVariance.x, popupVariance.x), Random.Range(-popupVariance.y, popupVariance.y));

        // Create a popup
        var popup = Instantiate(popupTextPrefab, transform.root).GetComponent<PopupTextUI>();
        popup.transform.position = pointsText.transform.position + offset;
        if (change > 0)
        {
            // Increase
            popup.Initialize("+" + change + " pt", correctColor);
        }
        else if (change < 0)
        {
            // Decrease
            popup.Initialize("-" + -change + " pt", skipColor);
        }
        else
        {
            // Don't show
            popup.Initialize("", Color.clear);
        }

    }

    private void UpdateTurnRule(ExtraRule rule)
    {
        // Set text
        if (rule != null)
            turnRuleText.text = rule.name + ": " + rule.description;
        else
            turnRuleText.text = "";
    }
}
