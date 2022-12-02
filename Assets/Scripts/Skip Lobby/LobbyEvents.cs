using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LobbyEvents : MonoBehaviour
{
    public static LobbyEvents instance;
    private void Awake()
    {
        // Singleton Logic
        if (LobbyEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<TeamData, LobbyData> onAddTeam;
    public event Action<TeamData, LobbyData> onRemoveTeam;
    public event Action<PlayerData, TeamData> onAddPlayer;
    public event Action<PlayerData, TeamData> onRemovePlayer;

    public void TriggerAddTeam(TeamData teamData, LobbyData lobbyData)
    {
        if (onAddTeam != null)
        {
            onAddTeam(teamData, lobbyData);
        }
    }

    public void TriggerAddPlayer(PlayerData playerData, TeamData teamData)
    {
        if (onAddPlayer != null)
        {
            onAddPlayer(playerData, teamData);
        }
    }

    public void TriggerRemoveTeam(TeamData teamData, LobbyData lobbyData)
    {
        if (onRemoveTeam != null)
        {
            onRemoveTeam(teamData, lobbyData);
        }
    }

    public void TriggerRemovePlayer(PlayerData playerData, TeamData teamData)
    {
        if (onRemovePlayer != null)
        {
            onRemovePlayer(playerData, teamData);
        }
    }
}
