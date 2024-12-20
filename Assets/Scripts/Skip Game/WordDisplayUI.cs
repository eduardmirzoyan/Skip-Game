using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordDisplayUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI wordText;

    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset defaultFont;
    [SerializeField] private TMP_FontAsset armenianFont;

    const string SIZE_PREF = "Size";
    const float DEFAULT_SIZE = 108f;

    private void Start()
    {
        // Sub to events
        SkipGameEvents.instance.OnSetPlayer += SetFont;
        SkipGameEvents.instance.OnNewWord += ShowWord;
    }

    private void OnDestroy()
    {
        // Unsub to events
        SkipGameEvents.instance.OnSetPlayer -= SetFont;
        SkipGameEvents.instance.OnNewWord -= ShowWord;
    }

    private void SetFont(PlayerData playerData)
    {
        Language language = playerData.teamData.language;
        if (language == Language.Armenian)
            wordText.font = armenianFont;
        else
            wordText.font = defaultFont;
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
}
