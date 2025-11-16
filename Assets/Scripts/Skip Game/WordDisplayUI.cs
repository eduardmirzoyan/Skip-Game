using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordDisplayUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI wordText;
    [SerializeField] private TextMeshProUGUI subWordText;

    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset defaultFont;
    [SerializeField] private TMP_FontAsset armenianFont;

    private void Start()
    {
        // Sub to events
        SkipGameEvents.instance.OnSetPlayer += SetFont;
        SkipGameEvents.instance.OnNewWord += ShowWord;

        bool enableTranslationDisplay = PlayerPrefs.GetInt(GameConstants.DISPLAY_TRANSLATION_KEY, 0) == 1;
        subWordText.enabled = enableTranslationDisplay;
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

    private void ShowWord(string mainWord, string subWord)
    {
        float size = PlayerPrefs.GetInt(GameConstants.WORD_SIZE_KEY, GameConstants.DEFUALT_WORD_SIZE);
        wordText.fontSizeMax = size;
        wordText.text = mainWord;

        // If words are different, show subword
        if (mainWord != subWord)
            subWordText.text = subWord;
    }
}
