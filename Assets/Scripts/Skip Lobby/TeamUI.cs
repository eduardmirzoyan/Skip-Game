using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeamUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject addPlayerButton;
    [SerializeField] private GameObject removeTeamButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Dropdown dropdown;

    [Header("Data")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TeamData teamData;

    [Header("Settings")]
    [SerializeField] private float scaleTime = 0.1f;

    private void Awake()
    {
        // Sub to events
        LobbyEvents.instance.OnAddPlayer += AddPlayers;
        LobbyEvents.instance.OnRemovePlayer += RemovePlayer;
        LobbyEvents.instance.OnRemoveTeam += RemoveTeam;
        LobbyEvents.instance.OnAddTeam += AddTeam;
    }

    private void OnDestroy()
    {
        // Unsub to events
        LobbyEvents.instance.OnAddPlayer -= AddPlayers;
        LobbyEvents.instance.OnRemovePlayer -= RemovePlayer;
        LobbyEvents.instance.OnRemoveTeam -= RemoveTeam;
        LobbyEvents.instance.OnAddTeam -= AddTeam;
    }

    public void Initialize(TeamData teamData)
    {
        this.teamData = teamData;
        nameInput.text = teamData.name;

        // Check to see if it's this team and only 1 left
        if (teamData.lobbyData.Size > 1)
        {
            // Prevent deleting
            removeTeamButton.SetActive(true);
        }

        // Initialize dropdown
        dropdown.options = new List<TMP_Dropdown.OptionData>();
        string[] languages = System.Enum.GetNames(typeof(Language));
        foreach (var language in languages)
        {
            var option = new TMP_Dropdown.OptionData(language);
            dropdown.options.Add(option);
        }

        // Select option based on language
        dropdown.value = (int)teamData.language;

        // Grow in
        StartCoroutine(GrowIn(scaleTime));
    }

    # region Logic

    public void AddPlayerButton()
    {
        // Controller -> Logic

        // Make sure team isn't full
        if (teamData.Size >= teamData.maxSize) return;

        // Create a new player
        var playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.Initialize($"Player {teamData.Size + 1}", teamData);

        // Add to team
        teamData.AddPlayer(playerData);

        // Logic -> Visuals
        // Trigger event for visuals
        LobbyEvents.instance.TriggerAddPlayer(playerData, teamData);
    }

    public void RemoveTeamButton()
    {
        // Controller -> Logic
        var lobbyData = teamData.lobbyData;

        // Error check
        if (teamData == null || lobbyData == null) return;

        // Remove team
        lobbyData.RemoveTeam(teamData);

        // Trigger event
        LobbyEvents.instance.TriggerRemoveTeam(teamData, lobbyData);
    }

    public void ChangeTeamName(string newName)
    {
        // Change name
        teamData.name = newName;
    }

    public void ChangeLanguage(int languageIndex)
    {
        // Change language
        teamData.language = (Language)languageIndex;
    }

    # endregion

    # region Visuals

    private void AddPlayers(PlayerData playerData, TeamData teamData)
    {
        if (this.teamData != teamData) return;

        // Visuals
        var playerUI = Instantiate(playerPrefab, verticalLayoutGroup.transform).GetComponent<PlayerUI>();
        playerUI.Initialize(playerData);

        // Move button to end of grid
        addPlayerButton.transform.SetAsLastSibling();

        // Check if team is full
        if (teamData.IsFull())
        {
            // Hide button
            addPlayerButton.SetActive(false);
        }
    }

    private void AddTeam(TeamData teamData, LobbyData lobbyData)
    {
        if (this.teamData != teamData)
        {
            // Check to see if it's this team and only 1 left
            if (lobbyData.Size > 1)
            {
                // Prevent deleting
                removeTeamButton.SetActive(true);
            }

            return;
        }
    }

    private void RemoveTeam(TeamData teamData, LobbyData lobbyData)
    {
        if (this.teamData != teamData)
        {
            // Check to see if it's this team and only 1 left
            if (lobbyData.Size <= 1)
            {
                // Prevent deleting
                removeTeamButton.SetActive(false);
            }

            return;
        }

        // Shrink out
        StartCoroutine(ShrinkOut(scaleTime));
    }

    private void RemovePlayer(PlayerData playerData, TeamData teamData)
    {
        if (this.teamData != teamData) return;

        if (!teamData.IsFull())
        {
            // Show button
            addPlayerButton.SetActive(true);
        }
    }

    private IEnumerator ShrinkOut(float duration)
    {
        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp scale
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Destroy
        Destroy(this.gameObject);
    }

    private IEnumerator GrowIn(float duration)
    {
        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp scale
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set scale
        transform.localScale = Vector3.one;

        // Allow interaction
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    # endregion
}
