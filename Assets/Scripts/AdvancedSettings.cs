using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedSettings : ScriptableObject
{
    public int pointsOnCorrect;
    public int pointsOnSkip;
    public int timeOnCorrect;
    public int timeOnSkip;

    public List<ExtraRule> extraRules;

    public void Initialize()
    {
        // Set default values
        pointsOnCorrect = 1;
        pointsOnSkip = 0;
        timeOnCorrect = 0;
        timeOnSkip = 0;

        // Initialize
        extraRules = new List<ExtraRule>();
    }

    public ExtraRule GetRandomRestriction()
    {
        if (extraRules.Count == 0) return null;

        return extraRules[Random.Range(0, extraRules.Count)];
    }
}
