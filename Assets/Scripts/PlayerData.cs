using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public new string name;
    public int score; /// How many points this player has earned
    public int numTurns;
    public TeamData teamData;

    public void Initialize(string name, TeamData teamData = null)
    {
        this.name = name;
        score = 0;
        this.teamData = teamData;
        numTurns = 0;
    }

    public void IncrementTurn()
    {
        // Increase num turns
        numTurns++;
    }

    public float GetAverageScore()
    {
        // If player hasn't gotten a turn, return their score
        if (numTurns == 0) return score;
        
        // Calculate average
        float average = score / numTurns;

        // Return average, rounded to 2 decimals
        return Mathf.Round(average * 100f) / 100f;
    }
}