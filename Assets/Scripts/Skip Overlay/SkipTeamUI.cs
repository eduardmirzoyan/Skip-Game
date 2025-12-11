using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkipTeamUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject containerObject;
    [SerializeField] private Transform playersTransform;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI languageText;
    [SerializeField] private TextMeshProUGUI turnsText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Outline outline;

    [Header("Data")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TeamData teamData;
    [SerializeField] private bool shouldMoveRight;

    private void Awake()
    {
        // Sub to events
        SkipOverlayEvents.instance.OnSelectTeam += SelectTeam;
        SkipOverlayEvents.instance.OnSelectLanguage += SelectLanguage;
    }

    private void OnDestroy()
    {
        // Unsub to events
        SkipOverlayEvents.instance.OnSelectTeam -= SelectTeam;
        SkipOverlayEvents.instance.OnSelectLanguage -= SelectLanguage;
    }

    public void Initialize(TeamData teamData, bool shouldMoveRight)
    {
        this.teamData = teamData;
        this.shouldMoveRight = shouldMoveRight;
        nameText.text = teamData.name;
        languageText.text = teamData.language.ToString();
        turnsText.text = $"<b>Total Turns:</b> {teamData.GetTurns()}";
        pointsText.text = $"<b>Total Points:</b> {teamData.GetPoints()}";

        foreach (var playerData in teamData.players)
        {
            var playerUI = Instantiate(playerPrefab, playersTransform).GetComponent<SkipPlayerUI>();
            playerUI.Initialize(playerData);
        }
    }

    private void SelectTeam(TeamData teamData)
    {
        if (this.teamData == teamData)
        {
            outline.enabled = true;
            float offset = shouldMoveRight ? 50f : -50f;
            LeanTween.moveLocalX(containerObject, offset, GameConstants.ANIMATION_DURATION).setEaseOutQuad();
        }
        else
        {
            outline.enabled = false;
            LeanTween.moveLocalX(containerObject, 0f, GameConstants.ANIMATION_DURATION).setEaseOutQuad();
        }
    }

    private void SelectLanguage(TeamData teamData)
    {
        if (this.teamData != teamData) return;

        languageText.text = teamData.language.ToString();
    }
}
