using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkipOverlayEvents : MonoBehaviour
{
    public static SkipOverlayEvents instance;
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

    public event Action<LobbyData> OnEnterOverlay;
    public event Action<TeamData> OnSelectTeam;
    public event Action<PlayerData> OnSelectPlayer;
    public event Action<TeamData> OnSelectLanguage;
    public event Action<LobbyData> OnEnd;

    public void TriggerOnEnterOverlay(LobbyData lobbyData)
    {
        OnEnterOverlay?.Invoke(lobbyData);
    }

    public void TriggerOnSelectTeam(TeamData teamData)
    {
        OnSelectTeam?.Invoke(teamData);
    }

    public void TriggerOnSelectPlayer(PlayerData playerData)
    {
        OnSelectPlayer?.Invoke(playerData);
    }

    public void TriggerOnSelectLanguage(TeamData teamData)
    {
        OnSelectLanguage?.Invoke(teamData);
    }

    public void TriggerOnEnd(LobbyData lobbyData)
    {
        OnEnd?.Invoke(lobbyData);
    }
}
