using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI listText;

    [Header("Data")]
    [SerializeField] private bool listByPoints;

    private void Start()
    {
        // Sub
        SkipEndEvents.instance.OnEnterEndGame += UpdatePlayerList;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipEndEvents.instance.OnEnterEndGame -= UpdatePlayerList;
    }

    private void UpdatePlayerList(LobbyData lobbyData)
    {
        if (listByPoints)
            ListByPoints(lobbyData);
        else
            ListByAverage(lobbyData);
    }

    private void ListByPoints(LobbyData lobbyData)
    {
        List<PlayerData> players = new();
        foreach (var team in lobbyData.teams)
            players.AddRange(team.players);

        var orderedPlayers = players.OrderByDescending(player => player.points);

        listText.text = string.Empty;

        int rank = 1;
        foreach (var player in orderedPlayers)
        {
            listText.text += $"{rank}. {player.name} -- {player.points} pts\n";
            rank++;
        }
    }

    private void ListByAverage(LobbyData lobbyData)
    {
        List<PlayerData> players = new();
        foreach (var team in lobbyData.teams)
            players.AddRange(team.players);

        var orderedPlayers = players.OrderByDescending(player => player.AveragePoints);

        listText.text = string.Empty;

        int rank = 1;
        foreach (var player in orderedPlayers)
        {
            listText.text += $"{rank}. {player.name} -- {player.AveragePoints:F1} avg\n";
            rank++;
        }
    }
}
