using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int numSkips;
    public int maxPointsInRound;
    public string longestWord;
    public float longestTimeSeconds;

    public PlayerStats()
    {
        Reset();
    }

    public void Reset()
    {
        numSkips = 0;
        maxPointsInRound = 0;
        longestWord = string.Empty;
        longestTimeSeconds = 0f;
    }
}
