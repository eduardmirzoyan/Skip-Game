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
    [SerializeField] private ParticleSystem[] fireworkParticlesList;

    [Header("Pedastal")]
    [SerializeField] private TextMeshProUGUI[] placedTeamNames;
    [SerializeField] private TextMeshProUGUI[] placedTeamScores;

    private void Start()
    {
        // Sub
        SkipOverlayEvents.instance.OnEnd += DisplayResults;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipOverlayEvents.instance.OnEnd -= DisplayResults;
    }

    private void DisplayResults(LobbyData lobbyData)
    {
        // Play sound
        AudioManager.instance.PlayImm("FanFare");

        // Show UI
        windowAnimator.Play("Show");

        // Update UI
        DisplayTeams(lobbyData);
        DisplayPlayers(lobbyData);

        // Play vfx
        foreach (var p in fireworkParticlesList)
            p.Play();
    }

    private void DisplayTeams(LobbyData lobbyData)
    {
        // Get team with highest score
        var orderedTeams = lobbyData.teams.OrderByDescending(team => team.GetScore());

        // Get team placements
        Dictionary<int, int> placementTable = new(); // Score - Placement
        int placement = 1;
        foreach (var team in orderedTeams)
        {
            int score = team.GetScore();

            // If we have placed this team, then stop
            if (placementTable.ContainsKey(score))
                continue;

            placementTable.Add(score, placement);
            placement++;
        }

        string result = "";
        int count = 1;
        // Update team leaderboard
        foreach (var team in orderedTeams)
        {
            int score = team.GetScore();
            int place = placementTable[score];

            // Update list
            result += $"{place}. {team.name} -- {score} pts\n";

            // Update pedastals
            if (count - 1 < placedTeamNames.Length)
            {
                placedTeamNames[count - 1].text = team.name;
                placedTeamScores[count - 1].text = $"{score}";
                count++;
            }
        }
        teamsText.text = result;
    }

    private void DisplayPlayers(LobbyData lobbyData)
    {
        // Get all players from lobby
        List<PlayerData> players = new();
        foreach (var team in lobbyData.teams)
            players.AddRange(team.players);

        // Sort players
        var orderedPlayers = players.OrderByDescending(player => player.score);

        // Get team placements
        Dictionary<int, int> placementTable = new(); // Score - Placement
        int placement = 1;
        foreach (var player in orderedPlayers)
        {
            int score = player.score;

            // If we have placed this team, then stop
            if (placementTable.ContainsKey(score))
                continue;

            placementTable.Add(score, placement);
            placement++;
        }

        // Update player leaderboard
        string result = "";
        foreach (var player in orderedPlayers)
        {
            int score = player.score;
            int place = placementTable[score];

            result += $"{place}. {player.name} -- {score} pts\n";
        }
        playersText.text = result;
    }
}
