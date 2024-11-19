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
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<TeamData, LobbyData> OnAddTeam;
    public event Action<TeamData, LobbyData> OnRemoveTeam;
    public event Action<PlayerData, TeamData> OnAddPlayer;
    public event Action<PlayerData, TeamData> OnRemovePlayer;

    public void TriggerAddTeam(TeamData teamData, LobbyData lobbyData)
    {
        OnAddTeam?.Invoke(teamData, lobbyData);
    }

    public void TriggerAddPlayer(PlayerData playerData, TeamData teamData)
    {
        OnAddPlayer?.Invoke(playerData, teamData);
    }

    public void TriggerRemoveTeam(TeamData teamData, LobbyData lobbyData)
    {
        OnRemoveTeam?.Invoke(teamData, lobbyData);
    }

    public void TriggerRemovePlayer(PlayerData playerData, TeamData teamData)
    {
        OnRemovePlayer?.Invoke(playerData, teamData);
    }
}
