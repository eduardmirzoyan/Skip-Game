using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PedestalUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI[] placementNames;
    [SerializeField] private TextMeshProUGUI[] placementScores;

    [Header("Data")]
    [SerializeField] private bool isTeamMode;

    private void Start()
    {
        // Sub
        SkipEndEvents.instance.OnEnterEndGame += UpdatePedestal;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipEndEvents.instance.OnEnterEndGame -= UpdatePedestal;
    }

    private void UpdatePedestal(LobbyData lobbyData)
    {
        if (isTeamMode)
            DisplayTeams(lobbyData);
        else
            DisplayPlayers(lobbyData);
    }

    private void DisplayTeams(LobbyData lobbyData)
    {
        var orderedTeams = lobbyData.teams.OrderByDescending(team => team.GetPoints());
        for (int i = 0; i < placementNames.Length; i++)
        {
            if (i < orderedTeams.Count())
            {
                var team = orderedTeams.ElementAt(i);
                placementNames[i].text = team.name;
                placementScores[i].text = $"{team.GetPoints()}";
            }
            else
            {
                placementNames[i].text = "N/A";
                placementScores[i].text = string.Empty;
            }
        }
    }

    private void DisplayPlayers(LobbyData lobbyData)
    {
        List<PlayerData> players = new();
        foreach (var team in lobbyData.teams)
            players.AddRange(team.players);

        var orderedPlayers = players.OrderByDescending(player => player.points);
        for (int i = 0; i < placementNames.Length; i++)
        {
            if (i < orderedPlayers.Count())
            {
                var player = orderedPlayers.ElementAt(i);
                placementNames[i].text = player.name;
                placementScores[i].text = $"{player.points}";
            }
            else
            {
                placementNames[i].text = "N/A";
                placementScores[i].text = string.Empty;
            }
        }
    }
}
