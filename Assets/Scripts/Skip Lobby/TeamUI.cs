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
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Data")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TeamData teamData;

    [Header("Settings")]
    [SerializeField] private float startAlpha = 0.75f;
    [SerializeField] private Vector2 forceVariance;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float timeTilDestroy = 2f;
    

    private void Start()
    {
        // Sub to events
        LobbyEvents.instance.onAddPlayer += AddPlayers;
        LobbyEvents.instance.onRemovePlayer += RemovePlayer;
        LobbyEvents.instance.onRemoveTeam += RemoveTeam;
        LobbyEvents.instance.onAddTeam += AddTeam;

        // Add a player
        AddPlayerButton();
    }

    private void OnDestroy()
    {
        // Unsub to events
        LobbyEvents.instance.onAddPlayer -= AddPlayers;
        LobbyEvents.instance.onRemovePlayer -= RemovePlayer;
        LobbyEvents.instance.onRemoveTeam -= RemoveTeam;
        LobbyEvents.instance.onAddTeam -= AddTeam;
    }

    public void Initialize(TeamData teamData)
    {
        this.teamData = teamData;
        nameInput.text = teamData.name;

        // Check to see if it's this team and only 1 left
        if (teamData.lobbyData.size > 1)
        {
            // Prevent deleting
            removeTeamButton.SetActive(true);
        }
    }

    # region Logic

    public void AddPlayerButton()
    {
        // Controller -> Logic

        // Make sure team isn't full
        if (teamData.size >= teamData.maxSize) return;

        // Create a new player
        var playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.Initialize("Player " + (teamData.size + 1), teamData);

        // Add to team
        teamData.players.Add(playerData);

        // Logic -> Visuals
        // Trigger event for visuals
        LobbyEvents.instance.TriggerAddPlayer(playerData, teamData);
    }

    public void RemoveTeamButton()
    {
        // Controller -> Logic
        // LobbyManager.instance.RemoveTeam(teamData, teamData.lobbyData);

        var lobbyData = teamData.lobbyData;

        // Error check
        if (teamData == null || lobbyData == null) return;

        // Remove team
        lobbyData.teams.Remove(teamData);

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
        teamData.language = (Language) languageIndex;
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
            if (lobbyData.size > 1)
            {
                // Prevent deleting
                removeTeamButton.SetActive(true);
            }

            return;
        }
    }

    private void RemoveTeam(TeamData teamData, LobbyData lobbyData)
    {
        if (this.teamData != teamData) {
            // Check to see if it's this team and only 1 left
            if (lobbyData.size <= 1)
            {
                // Prevent deleting
                removeTeamButton.SetActive(false);
            }

            return;
        }

        // Give a randomized push
        Vector2 randomPush = new Vector2(Random.Range(-forceVariance.x, forceVariance.x), forceVariance.y);
        rigidbody2d.velocity = randomPush;

        // Enable gravity
        rigidbody2d.gravityScale = gravityMultiplier;

        // Remove parent
        transform.SetParent(transform.root);

        // Fade out
        StartCoroutine(FadeOut(startAlpha, timeTilDestroy)); 
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

    private IEnumerator FadeOut(float startAlpha, float duration)
    {
        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Start alpha
        canvasGroup.alpha = startAlpha;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp alpha
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Destroy
        Destroy(this.gameObject);
    }

    # endregion
}
