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
    public event Action<int, int> onScoreChanged;
    public event Action<float, float> onTimeChanged;
    public event Action<TurnData> onEnd;
    public event Action onRedo;
    public event Action<ExtraRule> onSetTurnRule;

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

    public void TriggerOnScoreChanged(int points, int change)
    {
        if (onScoreChanged != null)
        {
            onScoreChanged(points, change);
        }
    }

    public void TriggerOnTimeChanged(float elapsed, float duration)
    {
        if (onTimeChanged != null)
        {
            onTimeChanged(elapsed, duration);
        }
    }

    public void TriggerOnEnd(TurnData turnData)
    {
        if (onEnd != null)
        {
            onEnd(turnData);
        }
    }

    public void TriggerOnRedo()
    {
        if (onRedo != null)
        {
            onRedo();
        }
    }

    public void TriggerSetTurnRule(ExtraRule rule)
    {
        if (onSetTurnRule != null)
        {
            onSetTurnRule(rule);
        }
    }
}
