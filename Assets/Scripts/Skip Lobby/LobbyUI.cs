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
    [SerializeField] private TMP_Dropdown totalRoundsDropdown;
    [SerializeField] private TMP_Dropdown turnTimeDropdown;

    [Header("Data")]
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private LobbyData lobbyData;

    [Header("Settings")]
    [SerializeField] private int lobbySize = 9;
    [SerializeField] private int teamSize = 5;
    [SerializeField] private Language defaultLanguage = Language.English;

    const string WORD_BANK = "Word Bank";

    private void Awake()
    {
        // Sub to events
        LobbyEvents.instance.OnAddTeam += AddTeam;
        LobbyEvents.instance.OnRemoveTeam += RemoveTeam;

        InitializeDropdowns();
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

    private void InitializeSettings(LobbyData lobbyData)
    {
        // Initialize game length
        totalRoundsDropdown.value = lobbyData.totalRounds switch
        {
            1 => 0,
            3 => 1,
            5 => 2,
            10 => 3,
            20 => 4,
            _ => 5,
        };

        turnTimeDropdown.value = lobbyData.turnTime switch
        {
            10f => 0,
            20f => 1,
            30f => 2,
            45f => 3,
            60f => 4,
            90f => 5,
            _ => 6,
        };

        // Initialize randomization
        randomTeamsToggle.isOn = lobbyData.randomTeams;
        randomPlayersToggle.isOn = lobbyData.randomPlayers;

        // Initialize advanced settings
        advancedSettingsUI.Initialize(lobbyData.advancedSettings);
    }

    private void InitializeVisuals(LobbyData lobbyData)
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

    public void AddNewTeamButton()
    {
        // Controller -> Logic

        // Make sure lobby isn't full or team isn't null
        if (lobbyData.Size >= lobbyData.maxSize) return;

        // Create a new team
        var teamData = ScriptableObject.CreateInstance<TeamData>();
        teamData.Initialize($"Team {lobbyData.Size + 1}", teamSize, defaultLanguage, lobbyData);

        // Add team
        lobbyData.AddTeam(teamData);

        // Trigger event for visuals (Logic -> Visuals)
        LobbyEvents.instance.TriggerAddTeam(teamData, lobbyData);

        // ============================================================== //

        // Create a new player
        var playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.Initialize($"Player {teamData.Size + 1}", teamData);

        // Add to team
        teamData.AddPlayer(playerData);

        // Trigger event
        LobbyEvents.instance.TriggerAddPlayer(playerData, teamData);
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

        // Update UI
        teamCountText.text = $"({lobbyData.Size}/{lobbyData.maxSize})";
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
        lobbyData.randomTeams = randomTeamsToggle.isOn;
        if (lobbyData.randomTeams)
        {
            // Randomize list
            lobbyData.RandomizeTeams();
        }

        lobbyData.randomPlayers = randomPlayersToggle.isOn;
        if (lobbyData.randomPlayers)
        {
            // Loop through each team
            foreach (var teamData in lobbyData.teams)
            {
                // Randomize list
                teamData.RandomizePlayers();
            }
        }

        // Set game duration
        lobbyData.totalRounds = totalRoundsDropdown.value switch
        {
            0 => 1,
            1 => 3,
            2 => 5,
            3 => 10,
            4 => 20,
            _ => -1,
        };

        // Set round time
        lobbyData.turnTime = turnTimeDropdown.value switch
        {
            0 => 10f,
            1 => 20f,
            2 => 30f,
            3 => 45f,
            4 => 60f,
            5 => 90f,
            _ => 120f,
        };

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

    #endregion

    #region Helpers

    private void InitializeDropdowns()
    {
        // Total rounds
        totalRoundsDropdown.options.Clear();
        totalRoundsDropdown.options.Add(new TMP_Dropdown.OptionData("1 Round"));
        totalRoundsDropdown.options.Add(new TMP_Dropdown.OptionData("3 Rounds"));
        totalRoundsDropdown.options.Add(new TMP_Dropdown.OptionData("5 Rounds"));
        totalRoundsDropdown.options.Add(new TMP_Dropdown.OptionData("10 Rounds"));
        totalRoundsDropdown.options.Add(new TMP_Dropdown.OptionData("20 Rounds"));
        totalRoundsDropdown.options.Add(new TMP_Dropdown.OptionData("Infinite"));
        totalRoundsDropdown.RefreshShownValue();

        // Turn time
        turnTimeDropdown.options.Clear();
        turnTimeDropdown.options.Add(new TMP_Dropdown.OptionData("10 Seconds"));
        turnTimeDropdown.options.Add(new TMP_Dropdown.OptionData("20 Seconds"));
        turnTimeDropdown.options.Add(new TMP_Dropdown.OptionData("30 Seconds"));
        turnTimeDropdown.options.Add(new TMP_Dropdown.OptionData("45 Seconds"));
        turnTimeDropdown.options.Add(new TMP_Dropdown.OptionData("60 Seconds"));
        turnTimeDropdown.options.Add(new TMP_Dropdown.OptionData("90 Seconds"));
        turnTimeDropdown.options.Add(new TMP_Dropdown.OptionData("120 Seconds"));
        turnTimeDropdown.RefreshShownValue();
    }

    #endregion
}
