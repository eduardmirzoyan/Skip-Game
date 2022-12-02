using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkipOverlayUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform teamsTransform;
    [SerializeField] private TextMeshProUGUI nextTeamText;
    [SerializeField] private TextMeshProUGUI nextPlayerText;
    [SerializeField] private TextMeshProUGUI roundDisplayText;

    [Header("Data")]
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private float xRadius = 5f;
    [SerializeField] private float yRadius = 3f;

    private void Start()
    {
        // Sub
        SkipOverlayEvents.instance.onEnterOverlay += DisplayTeams;
        SkipOverlayEvents.instance.onSelectTeam += SelectTeam;
        SkipOverlayEvents.instance.onSelectPlayer += SelectPlayer;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipOverlayEvents.instance.onEnterOverlay -= DisplayTeams;
        SkipOverlayEvents.instance.onSelectTeam -= SelectTeam;
        SkipOverlayEvents.instance.onSelectPlayer -= SelectPlayer;
    }

    private void DisplayTeams(LobbyData lobbyData)
    {
        // Show teams around the center
        CreateTeamsAroundPoint(lobbyData, teamsTransform.position, xRadius, yRadius);

        // Update display
        roundDisplayText.text = "Round " + lobbyData.roundNumber + "/" + lobbyData.totalRounds;
    }

    public void CreateTeamsAroundPoint(LobbyData lobbyData, Vector3 point, float xRadius, float yRadius)
    {
        // Loop through each team
        int num = lobbyData.size;
        for (int i = 0; i < num; i++)
        {
            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i + Mathf.PI / 2;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal * xRadius, vertical * yRadius, 0);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * 1; // Radius is just the distance away from the point

            Debug.DrawLine(Vector3.zero, spawnPos, Color.red, 30f);

            /* Now spawn UI */
            var teamUI = Instantiate(teamPrefab, spawnPos, Quaternion.identity, teamsTransform).GetComponent<SkipTeamUI>();
            teamUI.Initialize(lobbyData.teams[i]);
        }
    }

    private void SelectTeam(TeamData teamData)
    {
        if (teamData != null)
        {
            // Update display
            roundDisplayText.text = "Round " + teamData.lobbyData.roundNumber + "/" + teamData.lobbyData.totalRounds;
        }
    }

    private void SelectPlayer(PlayerData playerData)
    {
        if (playerData != null)
        {
            // Update text
            nextPlayerText.text = "Next Team: " + playerData.teamData.name + "\nNext Player: " + playerData.name;
        }
        
    }
}
