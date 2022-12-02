using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This will handle all visuals relating to a lobby screen
/// </summary>
public class LobbyUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject addTeamButton;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI teamCountText;

    [Header("Data")]
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private LobbyData lobbyData;
    [SerializeField] private TextAsset textAsset;

    [Header("Settings")]
    [SerializeField] private int lobbySize = 9;
    [SerializeField] private int teamSize = 5;
    [SerializeField] private bool randomTeamOrder = false;
    [SerializeField] private bool randomPlayerOrder = false;
    [SerializeField] private int gameLength = 5;
    [SerializeField] private float roundTime = 30f;


    private void Start()
    {
        // Initialzie lobby
        lobbyData = ScriptableObject.CreateInstance<LobbyData>();
        lobbyData.Initialize("Lobby", lobbySize, textAsset);

        // Sub to events
        LobbyEvents.instance.onAddTeam += AddTeam;
        LobbyEvents.instance.onRemoveTeam += RemoveTeam;

        // Add a team
        AddTeamButton();

        // Open scene
        TransitionManager.instance.OpenScene();
    }

    private void OnDestroy()
    {
        // Unsub to events
        LobbyEvents.instance.onAddTeam -= AddTeam;
        LobbyEvents.instance.onRemoveTeam -= RemoveTeam;
    }

    public void Initialize(LobbyData lobbyData)
    {
        this.lobbyData = lobbyData;
        nameText.text = lobbyData.name;
    }

    public void AddTeamButton()
    {
        // Controller -> Logic

        // Make sure lobby isn't full or team isn't null
        if (lobbyData.size >= lobbyData.maxSize) return;

        // Create a new team
        var teamData = ScriptableObject.CreateInstance<TeamData>();
        teamData.Initialize("Team " + (lobbyData.size + 1), teamSize, lobbyData);

        // Add team
        lobbyData.teams.Add(teamData);

        // Update UI
        teamCountText.text = "(" + lobbyData.size + "/" +lobbyData.maxSize +  ")";

        // Trigger event for visuals (Logic -> Visuals)
        LobbyEvents.instance.TriggerAddTeam(teamData, lobbyData);
    }

    public void StartGameButton()
    {
        // Controller -> Logic
        // LobbyManager.instance.StartGame(lobbyData);

        // Alter order
        if (randomTeamOrder)
        {
            // Randomize list
            lobbyData.teams.Sort((team1, team2) => UnityEngine.Random.value.CompareTo(UnityEngine.Random.value));
        }

        if (randomPlayerOrder)
        {
            // Loop through each team
            foreach (var teamData in lobbyData.teams)
            {
                // Randomize list
                teamData.players.Sort((player1, player2) => UnityEngine.Random.value.CompareTo(UnityEngine.Random.value));
            }
        }

        // Set game duration
        lobbyData.totalRounds = gameLength;

        // Set round time
        lobbyData.roundTime = roundTime;

        // Save data
        DataManager.instance.SaveData(lobbyData);

        // Change scenes
        TransitionManager.instance.LoadNextScene();
    }

    public void MainMenuButton()
    {
        // Save data
        DataManager.instance.SaveData(null);

        // Change scenes
        TransitionManager.instance.LoadMainMenuScene();
    }

    private void AddTeam(TeamData teamData, LobbyData lobbyData)
    {
        if (this.lobbyData != lobbyData) return;

        // Visuals
        var teamUI = Instantiate(teamPrefab, gridLayoutGroup.transform).GetComponent<TeamUI>();
        teamUI.Initialize(teamData);

        // Move button to end of grid
        addTeamButton.transform.SetAsLastSibling();

        // Check if team is full
        if (lobbyData.IsFull()) {
            // Hide button
            addTeamButton.SetActive(false);
        }
    }

    private void RemoveTeam(TeamData teamData, LobbyData lobbyData)
    {
        if (this.lobbyData != lobbyData) return;

        if (!lobbyData.IsFull())
        {
            // Show button
            addTeamButton.SetActive(true);
        }

        // Update UI
        teamCountText.text = "(" + lobbyData.size + "/" + lobbyData.maxSize + ")";
    }

    public void SetRoundTime(int value)
    {
        switch (value)
        {
            case 0:
                roundTime = 10f;
                break;
            case 1:
                roundTime = 20f;
                break;
            case 2:
                roundTime = 30f;
                break;
            case 3:
                roundTime = 45f;
                break;
            case 4:
                roundTime = 60f;
                break;
            default:
                roundTime = 120f;
                break;
        }
    }

    public void SetGameLength(int value)
    {
        switch (value)
        {
            case 0:
                gameLength = 1;
                break;
            case 1:
                gameLength = 3;
                break;
            case 2:
                gameLength = 5;
                break;
            case 3:
                gameLength = 10;
                break;
            case 4:
                gameLength = 20;
                break;
            default:
                gameLength = 999;
                break;
        }
    }

    public void SetTeamOrder(bool state)
    {
        randomTeamOrder = state;
    }

    public void SetPlayerOrder(bool state)
    {
        randomPlayerOrder = state;
    }
}
