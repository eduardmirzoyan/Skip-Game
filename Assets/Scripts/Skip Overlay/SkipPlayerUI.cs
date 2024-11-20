using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkipPlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image background;
    [SerializeField] private Shadow selectShadow;
    [SerializeField] private Outline selectOutline;
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
        nameText.text = $"{playerData.name} -- {playerData.score} pts";
    }

    private void SelectPlayer(PlayerData playerData)
    {
        if (this.playerData == playerData)
        {
            // Select this
            background.color = selectColor;
            selectShadow.enabled = true;
            selectOutline.enabled = true;
        }
        else
        {
            // Deselect this
            background.color = defaultColor;
            selectShadow.enabled = false;
            selectOutline.enabled = false;
        }
    }
}
