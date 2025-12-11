using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int points;
    public int numTurns;
    public PlayerStats stats;
    public TeamData teamData;

    public float AveragePoints
    {
        get
        {
            if (numTurns == 0) return 0f;
            return (float)points / numTurns;
        }
    }

    public PlayerData(string name, TeamData teamData = null)
    {
        this.name = name;
        this.teamData = teamData;
        stats = new();
        points = 0;
        numTurns = 0;
    }

    public void Reset()
    {
        points = 0;
        numTurns = 0;
        stats.Reset();
    }
}