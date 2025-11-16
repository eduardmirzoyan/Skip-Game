using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorData : ScriptableObject
{
    public Color correctColor;
    public Color skipColor;
    public Color penaltyColor;
    public Color endColor;

    public string CorrectColorHex => $"#{ColorUtility.ToHtmlStringRGB(correctColor)}";
    public string SkipColorHex => $"#{ColorUtility.ToHtmlStringRGB(skipColor)}";
}
