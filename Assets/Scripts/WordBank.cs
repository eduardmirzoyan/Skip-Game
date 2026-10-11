using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WordBank : ScriptableObject
{
    public List<string> englishWords;
    public Dictionary<string, string> russianDict;
    public Dictionary<string, string> armenianDict;
    public Dictionary<string, string> frenchDict;
    // vietnamese 
    public Dictionary<string, string> vietnameseDict;

    public void Initialize(TextAsset textAsset)
    {
        // Initalize lists
        englishWords = new List<string>();
        russianDict = new Dictionary<string, string>();
        armenianDict = new Dictionary<string, string>();
        frenchDict = new Dictionary<string, string>();
        vietnameseDict = new Dictionary<string, string>();

        // Parse CSV into lists
        ReadCSV(textAsset);
    }

    private void ReadCSV(TextAsset textAsset)
    {
        if (textAsset == null)
            throw new System.Exception("No word bank text asset found.");

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
            vietnameseDict.Add(words[0], words[4]);
        }

        Debug.Log($"Word Bank Size: {englishWords.Count}");
    }

    public int GetRandomWordIndex()
    {
        return Random.Range(0, englishWords.Count);
    }

    public string PopWordAtIndex(int index, Language language)
    {
        string englishWord = englishWords[index];

        string translatedWord;
        switch (language)
        {
            case Language.Russian:
                translatedWord = russianDict[englishWord];
                russianDict.Remove(englishWord);
                break;
            case Language.Armenian:
                translatedWord = armenianDict[englishWord];
                armenianDict.Remove(englishWord);
                break;
            case Language.French:
                translatedWord = frenchDict[englishWord];
                frenchDict.Remove(englishWord);
                break;
            case Language.Vietnamese:
                translatedWord = vietnameseDict[englishWord];
                vietnameseDict.Remove(englishWord);
                break;
            default:
                translatedWord = englishWord;
                break;
        }

        // Remove word from bank
        englishWords.RemoveAt(index);

        return translatedWord;
    }

    public string PeakWordAtIndex(int index, Language language)
    {
        string englishWord = englishWords[index];
        string translatedWord = language switch
        {
            Language.Russian => russianDict[englishWord],
            Language.Armenian => armenianDict[englishWord],
            Language.French => frenchDict[englishWord],
            Language.Vietnamese => vietnameseDict[englishWord],
            _ => englishWord,
        };
        return translatedWord;
    }
}
