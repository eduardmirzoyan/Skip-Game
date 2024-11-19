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
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<PlayerData> OnSetPlayer;
    public event Action<string> OnNewWord;
    public event Action<int, int> OnScoreChanged;
    public event Action<float, float> OnTimeChanged;
    public event Action<TurnData> OnEnd;
    public event Action OnRedo;
    public event Action<ExtraRule> OnSetTurnRule;

    public void TriggerOnSetPlayer(PlayerData playerData)
    {
        OnSetPlayer?.Invoke(playerData);
    }

    public void TriggerOnNewWord(string word)
    {
        OnNewWord?.Invoke(word);
    }

    public void TriggerOnScoreChanged(int points, int change)
    {
        OnScoreChanged?.Invoke(points, change);
    }

    public void TriggerOnTimeChanged(float elapsed, float duration)
    {
        OnTimeChanged?.Invoke(elapsed, duration);
    }

    public void TriggerOnEnd(TurnData turnData)
    {
        OnEnd?.Invoke(turnData);
    }

    public void TriggerOnRedo()
    {
        OnRedo?.Invoke();
    }

    public void TriggerSetTurnRule(ExtraRule rule)
    {
        OnSetTurnRule?.Invoke(rule);
    }
}
