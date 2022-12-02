using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wordText;

    private void Start()
    {
        // Sub to events
        SkipGameEvents.instance.onNewWord += ShowWord;
    }

    private void OnDestroy()
    {
        // Unsub to events
        SkipGameEvents.instance.onNewWord -= ShowWord;
    }

    private void ShowWord(string word)
    {
        // Change color
        wordText.color = Color.black;
        
        // Change text
        wordText.text = word;
    }
}
