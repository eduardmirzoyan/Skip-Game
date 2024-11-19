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
    [SerializeField] private TextMeshProUGUI teamCountText;
    [SerializeField] private AdvancedSettingsUI advancedSettingsUI;
    [SerializeField] private Toggle randomTeamsToggle;
    [SerializeField] private Toggle randomPlayersToggle;

    [Header("Data")]
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private LobbyData lobbyData;

    [Header("Settings")]
    [SerializeField] private int lobbySize = 9;
    [SerializeField] private int teamSize = 5;
    [SerializeField] private Language defaultLanguage = Language.English;
    [SerializeField] private int gameLength = 5;
    [SerializeField] private float turnTime = 30f;
    [SerializeField] private bool randomTeams;
    [SerializeField] private bool randomPlayers;

    const string WORD_BANK = "Word Bank";

    private void Awake()
    {
        // Sub to events
        LobbyEvents.instance.OnAddTeam += AddTeam;
        LobbyEvents.instance.OnRemoveTeam += RemoveTeam;
    }

    private void Start()
    {
        // Attempt to load saved data
        LobbyData lobbyData = DataManager.instance.LoadData();

        // If no saved lobby, then create a new one
        if (lobbyData == null)
        {
            // Load word bank
            TextAsset textAsset = Resources.Load<TextAsset>(WORD_BANK);

            // Initialzie lobby
            lobbyData = ScriptableObject.CreateInstance<LobbyData>();
            lobbyData.Initialize(lobbySize, textAsset);

            // Create one team
            var teamData = ScriptableObject.CreateInstance<TeamData>();
            teamData.Initialize($"Team 1", teamSize, defaultLanguage, lobbyData);
            lobbyData.AddTeam(teamData);

            // Create one player
            var playerData = ScriptableObject.CreateInstance<PlayerData>();
            playerData.Initialize($"Player 1", teamData);

            // Add to team
            teamData.AddPlayer(playerData);
        }

        this.lobbyData = lobbyData;

        InitializeSettings(lobbyData);

        // Update visuals
        InitializeVisuals(lobbyData);

        // Open scene
        TransitionManager.instance.OpenScene();
    }

    private void OnDestroy()
    {
        // Unsub to events
        LobbyEvents.instance.OnAddTeam -= AddTeam;
        LobbyEvents.instance.OnRemoveTeam -= RemoveTeam;
    }

    public void InitializeSettings(LobbyData lobbyData)
    {
        // TODO
        // Init game length settings

        // Init toggles
        randomTeamsToggle.isOn = lobbyData.randomTeams;
        randomPlayersToggle.isOn = lobbyData.randomPlayers;

        // Initialize advanced settings
        advancedSettingsUI.Initialize(lobbyData.advancedSettings);
    }

    public void InitializeVisuals(LobbyData lobbyData)
    {
        foreach (var teamData in lobbyData.teams)
        {
            LobbyEvents.instance.TriggerAddTeam(teamData, lobbyData);

            foreach (var playerData in teamData.players)
            {
                LobbyEvents.instance.TriggerAddPlayer(playerData, teamData);
            }
        }
    }

    public void AddTeamButton()
    {
        // Controller -> Logic

        // Make sure lobby isn't full or team isn't null
        if (lobbyData.Size >= lobbyData.maxSize) return;

        // Create a new team
        var teamData = ScriptableObject.CreateInstance<TeamData>();
        teamData.Initialize($"Team {lobbyData.Size + 1}", teamSize, defaultLanguage, lobbyData);

        // Add team
        lobbyData.AddTeam(teamData);

        // Update UI
        teamCountText.text = $"({lobbyData.Size}/{lobbyData.maxSize})";

        // Trigger event for visuals (Logic -> Visuals)
        LobbyEvents.instance.TriggerAddTeam(teamData, lobbyData);
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
        if (lobbyData.IsFull())
        {
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
        teamCountText.text = $"({lobbyData.Size}/{lobbyData.maxSize})";
    }

    #region UI Methods

    public void StartGameButton()
    {
        // Controller -> Logic

        // Alter order
        lobbyData.randomTeams = randomTeams;
        if (randomTeams)
        {
            // Randomize list
            lobbyData.RandomizeTeams();
        }

        lobbyData.randomPlayers = randomPlayers;
        if (randomPlayers)
        {
            // Loop through each team
            foreach (var teamData in lobbyData.teams)
            {
                // Randomize list
                teamData.RandomizePlayers();
            }
        }

        // Set game duration
        lobbyData.totalRounds = gameLength;

        // Set round time
        lobbyData.turnTime = turnTime;

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

    public void SetRoundTime(int value)
    {
        turnTime = value switch
        {
            0 => 10f,
            1 => 20f,
            2 => 30f,
            3 => 45f,
            4 => 60f,
            5 => 90,
            _ => 120f,
        };
    }

    public void SetGameLength(int value)
    {
        gameLength = value switch
        {
            0 => 1,
            1 => 3,
            2 => 5,
            3 => 10,
            4 => 20,
            _ => -1,
        };
    }

    public void SetTeamOrder(bool state)
    {
        randomTeams = state;
    }

    public void SetPlayerOrder(bool state)
    {
        randomPlayers = state;
    }

    #endregion
}
