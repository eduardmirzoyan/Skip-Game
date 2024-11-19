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
        onEnterOverlay?.Invoke(lobbyData);
    }

    public void TriggerOnSelectTeam(TeamData teamData)
    {
        onSelectTeam?.Invoke(teamData);
    }

    public void TriggerOnSelectPlayer(PlayerData playerData)
    {
        onSelectPlayer?.Invoke(playerData);
    }

    public void TriggerOnEnd(LobbyData lobbyData)
    {
        onEnd?.Invoke(lobbyData);
    }
}
