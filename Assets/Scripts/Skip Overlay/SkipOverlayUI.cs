using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkipOverlayUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform teamsTransform;
    [SerializeField] private TextMeshProUGUI nextTeamText;
    [SerializeField] private TextMeshProUGUI roundDisplayText;

    [Header("Data")]
    [SerializeField] private GameObject teamPrefab;

    private void Start()
    {
        // Sub
        SkipOverlayEvents.instance.OnEnterOverlay += DisplayTeams;
        SkipOverlayEvents.instance.OnSelectTeam += SelectTeam;
        SkipOverlayEvents.instance.OnSelectPlayer += SelectPlayer;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipOverlayEvents.instance.OnEnterOverlay -= DisplayTeams;
        SkipOverlayEvents.instance.OnSelectTeam -= SelectTeam;
        SkipOverlayEvents.instance.OnSelectPlayer -= SelectPlayer;
    }

    private void DisplayTeams(LobbyData lobbyData)
    {
        SpawnTeams(lobbyData);
        roundDisplayText.text = $"Round {lobbyData.roundNumber}/{lobbyData.totalRounds}";
    }

    private void SpawnTeams(LobbyData lobbyData)
    {
        int i = 0;
        foreach (var team in lobbyData.teams)
        {
            var teamUI = Instantiate(teamPrefab, teamsTransform).GetComponent<SkipTeamUI>();
            teamUI.Initialize(team, i % 2 == 0);
            i++;
        }
    }

    private void SelectTeam(TeamData teamData)
    {
        if (teamData != null)
        {
            // If infinite, the display only round #
            if (teamData.lobbyData.totalRounds == -1)
            {
                roundDisplayText.text = $"Round {teamData.lobbyData.roundNumber}";
            }
            else
            {
                roundDisplayText.text = $"Round {teamData.lobbyData.roundNumber}/{teamData.lobbyData.totalRounds}";
            }

        }
    }

    private void SelectPlayer(PlayerData playerData)
    {
        if (playerData != null)
        {
            // Update text
            nextTeamText.text = $"Next Team: {playerData.teamData.name}\nNext Player: {playerData.name}";
        }
    }
}
