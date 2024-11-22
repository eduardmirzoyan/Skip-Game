using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wordText;
    [SerializeField] private Text normalWordText;

    const string SIZE_PREF = "Size";
    const float DEFAULT_SIZE = 108f;

    private void Start()
    {
        // Sub to events
        SkipGameEvents.instance.OnNewWord += ShowWord;
    }

    private void OnDestroy()
    {
        // Unsub to events
        SkipGameEvents.instance.OnNewWord -= ShowWord;
    }

    private void ShowWord(string word)
    {
        // Change color
        wordText.color = Color.black;

        // Get float size if exists
        float size = PlayerPrefs.GetFloat(SIZE_PREF, DEFAULT_SIZE);
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
        float size = PlayerPrefs.GetFloat(SIZE_PREF, DEFAULT_SIZE);
        // Set size
        normalWordText.fontSize = (int)size;

        // Change text
        normalWordText.text = word;
    }
}
