using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Language { English, Russian, French }

[System.Serializable]
[CreateAssetMenu]
public class TeamData : ScriptableObject
{
    public new string name;
    public List<PlayerData> players; /// Members of the team
    public int maxSize;
    public LobbyData lobbyData;
    public int size { get { return players.Count; } }
    public Language language;
    public int playerIndex;

    public void Initialize(string name, int maxSize, LobbyData lobbyData = null)
    {
        this.name = name;
        this.maxSize = maxSize;
        this.language = Language.English;

        players = new List<PlayerData>(maxSize);
        playerIndex = 0;

        this.lobbyData = lobbyData;
    }

    public int GetScore()
    {
        return players.Sum(player => player.score);
    }

    public bool IsFull() 
    {
        return size >= maxSize;
    }

    public PlayerData GetIndexedPlayer()
    {
        return players[playerIndex];
    }

    public void IncrementIndex()
    {
        playerIndex++;
        if (playerIndex >= size)
        {
            playerIndex = 0;
        }
    }

    public void RandomizeIndex()
    {
        // Set index to a random number between 0 and max size
        playerIndex = Random.Range(0, size);
    }

    public void ResetIndex()
    {
        playerIndex = 0;
    }
}