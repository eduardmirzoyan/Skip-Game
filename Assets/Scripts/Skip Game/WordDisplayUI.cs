using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wordText;
    [SerializeField] private Text normalWordText;

    private void Start()
    {
        // Sub to events
        SkipGameEvents.instance.onNewWord += ShowWordNormal;
    }

    private void OnDestroy()
    {
        // Unsub to events
        SkipGameEvents.instance.onNewWord -= ShowWordNormal;
    }

    private void ShowWord(string word)
    {
        // Change color
        wordText.color = Color.black;

        // Get float size if exists
        float size = PlayerPrefs.GetFloat("Size", 108f);
        // Set size
        wordText.fontSizeMax = size;
        
        // Change text
        wordText.text = word;
    }

    private void ShowWordNormal(string word)
    {
        // Change color
        normalWordText.color = Color.black;

        // Get float size if exists
        float size = PlayerPrefs.GetFloat("Size", 108f);
        // Set size
        normalWordText.fontSize = (int) size;

        // Change text
        normalWordText.text = word;
    }
}
