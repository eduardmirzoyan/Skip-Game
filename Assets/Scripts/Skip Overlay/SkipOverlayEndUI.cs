using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkipOverlayEndUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private TextMeshProUGUI teamsText;
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private Animator animator;

    private void Start()
    {
        // Sub
        SkipOverlayEvents.instance.onEnd += DisplayResults;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipOverlayEvents.instance.onEnd -= DisplayResults;
    }

    private void DisplayResults(LobbyData lobbyData)
    {
        // Show UI
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Enable animation
        animator.Play("Win");

        // Get team with highest score
        var orderedTeams = lobbyData.teams.OrderByDescending(team => team.GetScore());

        // Update winner
        winnerText.text = "Team " + orderedTeams.First().name + " wins!";

        // Get a list of all players
        var players = new List<PlayerData>();

        string result = "";
        int count = 1;
        // Update team leaderboard
        foreach (var team in orderedTeams)
        {
            // Skip past 10
            if (count > 10) continue;

            // Update list
            result += count + ". " + team.name + " -- " + team.GetScore() + " pts\n";
            count++;

            // Add players
            players.AddRange(team.players);
        }
        teamsText.text = result;

        // Sort players
        var orderedPlayers = players.OrderByDescending(player => player.score);

        // Update player leaderboard
        result = "";
        count = 1;
        foreach (var player in orderedPlayers)
        {
            // Skip past 10
            if (count > 10) continue;

            result += count + ". " + player.name + " -- " + player.score + " pts\n";
            count++;
        }
        playersText.text = result;
    }
}
