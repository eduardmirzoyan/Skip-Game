using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SkipOverlayEndUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI teamsText;
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private Animator windowAnimator;
    [SerializeField] private ParticleSystem fireworkParticles;

    [Header("Pedastal")]
    [SerializeField] private TextMeshProUGUI[] placedTeamNames;
    [SerializeField] private TextMeshProUGUI[] placedTeamScores;

    [Header("Settings")]
    [SerializeField] private int maxTeamsDisplay = 10;
    [SerializeField] private int maxPlayersDisplay = 10;

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
        // Play sound
        AudioManager.instance.PlayImm("FanFare");

        // Show UI
        windowAnimator.Play("Show");

        // Get team with highest score
        var orderedTeams = lobbyData.teams.OrderByDescending(team => team.GetScore());

        // Get a list of all players
        var players = new List<PlayerData>();

        string result = "";
        int count = 1;
        // Update team leaderboard
        foreach (var team in orderedTeams)
        {
            // Skip past 10
            if (count > maxTeamsDisplay) continue;

            // Update list
            result += count + ". " + team.name + " -- " + team.GetScore() + " pts\n";

            // Update pedastals
            if (count - 1 < placedTeamNames.Length)
            {
                placedTeamNames[count - 1].text = team.name;
                placedTeamScores[count - 1].text = "" + team.GetScore();
            }

            // Add players
            players.AddRange(team.players);

            count++;
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
            if (count > maxPlayersDisplay) continue;

            result += count + ". " + player.name + " -- " + player.score + " pts\n";
            count++;
        }
        playersText.text = result;

        // Play vfx
        fireworkParticles.Play();
    }
}
