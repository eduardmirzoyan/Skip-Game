using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedSettingsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup bonusTurnRulesGroup;

    [Header("Data")]
    [SerializeField] private AdvancedSettings advancedSettings;

    [SerializeField] private List<ExtraRule> extraRules;
    

    public void Initialize(AdvancedSettings advancedSettings)
    {
        this.advancedSettings = advancedSettings;
    }

    public void Open()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ToggleBonusRules(bool state)
    {
        // Toggle visibility
        bonusTurnRulesGroup.alpha = state ? 1f : 0f;
        bonusTurnRulesGroup.interactable = state;
        bonusTurnRulesGroup.blocksRaycasts = state;

        if (!state)
        {
            // Clear all rules
            advancedSettings.extraRules.Clear();
        }
    }

    public void SetPointsOnCorrect(int value)
    {
        advancedSettings.pointsOnCorrect = -value + 2;
    }

    public void SetPointsOnSkip(int value)
    {
        advancedSettings.pointsOnSkip = -value + 2;
    }

    public void SetTimeOnCorrect(int value)
    {
        advancedSettings.timeOnCorrect = -value + 2;
    }

    public void SetTimeOnSkip(int value)
    {
        advancedSettings.timeOnSkip = -value + 2;
    }

    public void ToggleBlind(bool state)
    {
        var rule = extraRules[0];
        if (state)
        {
            // Add rule
            advancedSettings.extraRules.Add(rule);
        }
        else
        {
            advancedSettings.extraRules.Remove(rule);
        }
    }

    public void ToggleReversed(bool state)
    {
        var rule = extraRules[1];
        if (state)
        {
            // Add rule
            advancedSettings.extraRules.Add(rule);
        }
        else
        {
            advancedSettings.extraRules.Remove(rule);
        }
    }

    public void ToggleCaveman(bool state)
    {
        var rule = extraRules[2];
        if (state)
        {
            // Add rule
            advancedSettings.extraRules.Add(rule);
        }
        else
        {
            advancedSettings.extraRules.Remove(rule);
        }
    }

    public void ToggleSpellingBee(bool state)
    {
        var rule = extraRules[3];
        if (state)
        {
            // Add rule
            advancedSettings.extraRules.Add(rule);
        }
        else
        {
            advancedSettings.extraRules.Remove(rule);
        }
    }

    public void ToggleCharades(bool state)
    {
        var rule = extraRules[4];
        if (state)
        {
            // Add rule
            advancedSettings.extraRules.Add(rule);
        }
        else
        {
            advancedSettings.extraRules.Remove(rule);
        }
    }
}
