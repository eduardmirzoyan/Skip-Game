using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI winnerText;

    private void Start()
    {
        // Sub
        SkipEndEvents.instance.OnEnterEndGame += UpdateWinner;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipEndEvents.instance.OnEnterEndGame -= UpdateWinner;
    }

    private void UpdateWinner(LobbyData lobbyData)
    {
        var orderedTeams = lobbyData.teams.OrderByDescending(team => team.GetPoints());
        var winningTeam = orderedTeams.FirstOrDefault();

        if (winningTeam != null)
            winnerText.text = $"<b>{winningTeam.name}</b> is the winner!";
        else
            winnerText.text = "There is no winner :()";
    }
}
