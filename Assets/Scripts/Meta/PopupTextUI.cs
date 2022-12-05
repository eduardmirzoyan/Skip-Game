using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupTextUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    [Header("Data")]
    [SerializeField] private float duration = 1f;

    public void Initialize(string message, Color color)
    {
        // Set message
        text.text = message;
        text.color = color;

        // Destroy this text in a set amount of time
        Destroy(gameObject, duration);
    }

    private IEnumerator Fade(Color startColor, Color endColor, float duration)
    {
        // Set color
        text.color = startColor;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp color
            text.color = Color.Lerp(startColor, endColor, elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set color
        text.color = endColor;
    }
}
