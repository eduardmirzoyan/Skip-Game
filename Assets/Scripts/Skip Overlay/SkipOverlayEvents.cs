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
        if (SkipOverlayEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<LobbyData> onEnterOverlay;
    public event Action<TeamData> onSelectTeam;
    public event Action<PlayerData> onSelectPlayer;
    public event Action<LobbyData> onEnd;

    public void TriggerOnEnterOverlay(LobbyData lobbyData)
    {
        if (onEnterOverlay != null)
        {
            onEnterOverlay(lobbyData);
        }
    }

    public void TriggerOnSelectTeam(TeamData teamData)
    {
        if (onSelectTeam != null)
        {
            onSelectTeam(teamData);
        }
    }

    public void TriggerOnSelectPlayer(PlayerData playerData)
    {
        if (onSelectPlayer != null)
        {
            onSelectPlayer(playerData);
        }
    }

    public void TriggerOnEnd(LobbyData lobbyData)
    {
        if (onEnd != null)
        {
            onEnd(lobbyData);
        }
    }
}
