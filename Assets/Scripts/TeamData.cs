using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum Language { English, Russian, Armenian, French }

[System.Serializable]
[CreateAssetMenu]
public class TeamData : ScriptableObject
{
    public new string name;
    public List<PlayerData> players;
    public int maxSize;
    public LobbyData lobbyData;
    public Language language;
    public int playerIndex;
    public int Size { get { return players.Count; } }

    public void Initialize(string name, int maxSize, Language language, LobbyData lobbyData)
    {
        this.name = name;
        this.maxSize = maxSize;
        this.language = language;

        players = new List<PlayerData>(maxSize);
        playerIndex = 0;

        this.lobbyData = lobbyData;
    }

    public void Reset()
    {
        playerIndex = 0;
        foreach (var player in players)
        {
            player.Reset();
        }
    }

    public void AddPlayer(PlayerData playerData)
    {
        players.Add(playerData);
    }

    public bool RemovePlayer(PlayerData playerData)
    {
        return players.Remove(playerData);
    }

    public int GetScore()
    {
        return players.Sum(player => player.score);
    }

    public bool IsFull()
    {
        return Size >= maxSize;
    }

    public PlayerData GetIndexedPlayer()
    {
        return players[playerIndex];
    }

    public void IncrementIndex()
    {
        playerIndex++;
        if (playerIndex >= Size)
        {
            playerIndex = 0;
        }
    }

    public void IncrementLanguage()
    {
        int size = Enum.GetNames(typeof(Language)).Length;

        language++;
        if ((int)language >= size)
        {
            language = 0;
        }
    }

    public void RandomizePlayers()
    {
        players.Sort((player1, player2) => UnityEngine.Random.value.CompareTo(UnityEngine.Random.value));
    }
}