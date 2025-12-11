using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkipPlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI turnsText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI averageText;
    [SerializeField] private Image background;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectColor;

    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        // Sub to events
        SkipOverlayEvents.instance.OnSelectPlayer += SelectPlayer;
    }

    private void OnDestroy()
    {
        // Unsub to events
        SkipOverlayEvents.instance.OnSelectPlayer -= SelectPlayer; ;
    }

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        nameText.text = playerData.name;
        turnsText.text = playerData.numTurns.ToString();
        pointsText.text = playerData.points.ToString();
        averageText.text = $"{playerData.AveragePoints:F1}";
    }

    private void SelectPlayer(PlayerData playerData)
    {
        if (this.playerData == playerData)
        {
            // Select this
            background.color = selectColor;
        }
        else
        {
            // Deselect this
            background.color = defaultColor;
        }
    }
}
