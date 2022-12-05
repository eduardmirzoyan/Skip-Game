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
    
    [Header("Data")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private Color popupColor;
    [SerializeField] private Vector2 popupVariance;

    private void Start()
    {
        // Sub
        SkipGameEvents.instance.onTimeChanged += UpdateBar;
        SkipGameEvents.instance.onCorrectWord += UpdateScore;
    }

    private void OnDestroy()
    {
        // Sub
        SkipGameEvents.instance.onTimeChanged -= UpdateBar;
        SkipGameEvents.instance.onCorrectWord -= UpdateScore;
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

    private void UpdateScore(int points)
    {
        // Update display
        pointsText.text = points + " pts";

        // Get a random offset
        Vector3 offset = new Vector2(Random.Range(-popupVariance.x, popupVariance.x), Random.Range(-popupVariance.y, popupVariance.y));
        print(offset);

        // Create a popup
        var popup = Instantiate(popupTextPrefab, transform.root).GetComponent<PopupTextUI>();
        popup.transform.position = pointsText.transform.position + offset;
        popup.Initialize("+1 pt", popupColor);
    }
}
