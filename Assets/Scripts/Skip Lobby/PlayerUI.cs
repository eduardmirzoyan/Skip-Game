using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject removePlayerButton;

    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        // Sub to events
        LobbyEvents.instance.onRemovePlayer += RemovePlayer;
        LobbyEvents.instance.onAddPlayer += AddPlayer;
    }

    private void OnDestroy()
    {
        // Unsub to events
        LobbyEvents.instance.onRemovePlayer -= RemovePlayer;
        LobbyEvents.instance.onAddPlayer -= AddPlayer;
    }

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        nameInput.text = playerData.name;

        // Check to see if it's this team and only 1 left
        if (playerData.teamData.size > 1)
        {
            // Enable
            removePlayerButton.SetActive(true);
        }
    }

    public void RemovePlayerButton()
    {
        // Controller -> Logic
        // LobbyManager.instance.RemovePlayer(playerData, playerData.teamData);

        var teamData = playerData.teamData;

        // Error check
        if (teamData == null || playerData == null) return;

        // Remove player
        teamData.players.Remove(playerData);

        // Trigger event
        LobbyEvents.instance.TriggerRemovePlayer(playerData, teamData);
    }

    public void ChangePlayerName(string newName)
    {
        // Change name
        playerData.name = newName;
    }

    private void AddPlayer(PlayerData playerData, TeamData teamData)
    {
        if (this.playerData != playerData)
        {
            // Check to see if it's this team and only 1 left
            if (this.playerData.teamData == teamData && teamData.size > 1)
            {
                // Prevent deleting
                removePlayerButton.SetActive(true);
            }

            return;
        }
    }

    private void RemovePlayer(PlayerData playerData, TeamData teamData)
    {
        if (this.playerData != playerData) 
        {
            // Check to see if it's this team and only 1 left
            if (this.playerData.teamData == teamData && teamData.size <= 1)
            {
                // Prevent deleting
                removePlayerButton.SetActive(false);
            }

            return;
        }

        Destroy(this.gameObject);
    }
}
