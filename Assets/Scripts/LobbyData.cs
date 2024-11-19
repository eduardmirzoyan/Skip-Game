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
    public List<TeamData> teams;
    public int maxSize;
    public int teamIndex;
    public int roundNumber;
    public int totalRounds; // -1 means infinite
    public float turnTime;
    public WordBank wordBank;
    public AdvancedSettings advancedSettings;
    public bool randomTeams;
    public bool randomPlayers;

    public int Size
    {
        get
        {
            return teams.Count;
        }
    }

    public void Initialize(int maxSize, TextAsset textAsset)
    {
        this.maxSize = maxSize;

        teams = new List<TeamData>(maxSize);
        teamIndex = 0;

        totalRounds = 5;
        roundNumber = 1;
        turnTime = 30f;
        randomTeams = true;
        randomPlayers = false;

        wordBank = CreateInstance<WordBank>();
        wordBank.Initialize(textAsset);

        advancedSettings = CreateInstance<AdvancedSettings>();
        advancedSettings.Initialize();
    }

    public void AddTeam(TeamData teamData)
    {
        teams.Add(teamData);
    }

    public void RemoveTeam(TeamData teamData)
    {
        teams.Remove(teamData);
    }

    public void RandomizeTeams()
    {
        teams.Sort((team1, team2) => Random.value.CompareTo(Random.value));
    }

    public bool IsFull()
    {
        return Size >= maxSize;
    }

    public TeamData GetIndexedTeam()
    {
        return teams[teamIndex];
    }

    public void IncrementIndex()
    {
        teamIndex++;
        if (teamIndex >= Size)
        {
            // Increment round
            roundNumber++;

            // Change team
            teamIndex = 0;
        }
    }

    public bool GameOver()
    {
        return totalRounds != -1 && roundNumber > totalRounds;
    }

    public void RandomizeIndex()
    {
        // Set index to a random number between 0 and current size
        teamIndex = Random.Range(0, Size);
    }

    public void ResetIndex()
    {
        teamIndex = 0;
    }
}