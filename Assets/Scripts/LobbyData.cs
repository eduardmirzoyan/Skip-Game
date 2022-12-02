using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This stores all the data needed for a lobby
/// </summary>
[System.Serializable]
[CreateAssetMenu]
public class LobbyData : ScriptableObject
{
    public new string name;
    public List<TeamData> teams;
    public int maxSize;
    public int size { get { return teams.Count; } }
    public int teamIndex;
    public int roundNumber;
    public int totalRounds;
    public float roundTime;
    public WordBank wordBank;

    public void Initialize(string name, int maxSize, TextAsset textAsset)
    {
        this.name = name;
        this.maxSize = maxSize;
        
        teams = new List<TeamData>(maxSize);
        teamIndex = 0;

        this.totalRounds = 5;
        this.roundNumber = 1;
        this.roundTime = 30f;

        wordBank = ScriptableObject.CreateInstance<WordBank>();
        wordBank.Initialize(textAsset);
    }

    public bool IsFull()
    {
        return size >= maxSize;
    }

    public TeamData GetIndexedTeam()
    {
        return teams[teamIndex];
    }

    public void IncrementIndex()
    {
        teamIndex++;
        if (teamIndex >= size)
        {
            // Increment round
            roundNumber++;

            // Change team
            teamIndex = 0;
        }
    }

    public bool GameOver()
    {
        return roundNumber > totalRounds;
    }

    public void RandomizeIndex()
    {
        // Set index to a random number between 0 and max size
        teamIndex = Random.Range(0, size);
    }

    public void ResetIndex()
    {
        teamIndex = 0;
    }
}