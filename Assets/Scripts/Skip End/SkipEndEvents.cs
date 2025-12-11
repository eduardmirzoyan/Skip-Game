using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkipEndEvents : MonoBehaviour
{
    public static SkipEndEvents instance;
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

    public event Action<LobbyData> OnEnterEndGame;
    public void TriggerOnEnterEndGame(LobbyData lobbyData)
    {
        OnEnterEndGame?.Invoke(lobbyData);
    }
}
