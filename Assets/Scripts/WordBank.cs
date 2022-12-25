using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class WordBank : ScriptableObject
{
    public List<string> englishWords;
    public Dictionary<string, string> russianDict;
    public Dictionary<string, string> armenianDict;
    public Dictionary<string, string> frenchDict;

    public void Initialize(TextAsset textAsset)
    {
        // Initalize lists
        englishWords = new List<string>();
        russianDict = new Dictionary<string, string>();
        armenianDict = new Dictionary<string, string>();
        frenchDict = new Dictionary<string, string>();

        // Parse CSV into lists
        ReadCSV(textAsset);
    }

    private void ReadCSV(TextAsset textAsset)
    {
        // Load Asset
        textAsset = Resources.Load<TextAsset>("Word Bank");

        var lines = textAsset.ToString().Split('\n');
        bool skip = true;
        foreach (var line in lines)
        {
            if (skip) // Skip first one
            {
                skip = false;
                continue;
            }

            // Break line into words
            string[] words = line.Split(',');

            // Add english word
            englishWords.Add(words[0]);

            // Add translations
            russianDict.Add(words[0], words[1]);
            armenianDict.Add(words[0], words[2]);
            frenchDict.Add(words[0], words[3]);
        }

        Debug.Log(englishWords.Count);
    }

    public string GetRandomWord(Language language)
    {
        // Get random index
        int index = Random.Range(0, englishWords.Count);

        // Get english word
        string englishWord = englishWords[index];

        string translatedWord = "";
        // Translate if needed
        switch (language)
        {
            case Language.Russian:
                // Search dict
                translatedWord = russianDict[englishWord];
                russianDict.Remove(englishWord);
                break;
            case Language.Armenian:
                // Search dict
                translatedWord = armenianDict[englishWord];
                russianDict.Remove(englishWord);
            break;
            case Language.French:
                // Search dict
                translatedWord = frenchDict[englishWord];
                frenchDict.Remove(englishWord);
                break;
            default:
                translatedWord = englishWord;
                break;
        }

        // Remove word from bank
        englishWords.Remove(englishWord);

        return translatedWord;
    }
}
