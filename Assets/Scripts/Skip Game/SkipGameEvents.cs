using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkipGameEvents : MonoBehaviour
{
    public static SkipGameEvents instance;
    private void Awake()
    {
        // Singleton Logic
        if (SkipGameEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<PlayerData> onSetPlayer;
    public event Action<string> onNewWord;
    public event Action<int> onCorrectWord;
    public event Action<float, float> onTimeChanged;
    public event Action<int, int, float, PlayerData> onEnd;

    public void TriggerOnSetPlayer(PlayerData playerData)
    {
        if (onSetPlayer != null)
        {
            onSetPlayer(playerData);
        }
    }

    public void TriggerOnNewWord(string word)
    {
        if (onNewWord != null)
        {
            onNewWord(word);
        }
    }

    public void TriggerOnCorrectWord(int points)
    {
        if (onCorrectWord != null)
        {
            onCorrectWord(points);
        }
    }

    public void TriggerOnTimeChanged(float elapsed, float duration)
    {
        if (onTimeChanged != null)
        {
            onTimeChanged(elapsed, duration);
        }
    }

    public void TriggerOnEnd(int numberOfCorrect, int numberOfWords, float duration, PlayerData playerData)
    {
        if (onEnd != null)
        {
            onEnd(numberOfCorrect, numberOfWords, duration, playerData);
        }
    }
}
