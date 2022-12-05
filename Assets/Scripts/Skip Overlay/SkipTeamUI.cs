using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkipTeamUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI languageText;
    [SerializeField] private Image background;
    [SerializeField] private Shadow shadow;
    [SerializeField] private Outline outline;
    [SerializeField] private Animator animator;

    [Header("Data")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TeamData teamData;

    private void Start()
    {
        // Sub to events
        SkipOverlayEvents.instance.onSelectTeam += SelectTeam;
    }

    private void OnDestroy()
    {
        // Unsub to events
        SkipOverlayEvents.instance.onSelectTeam -= SelectTeam;
    }

    public void Initialize(TeamData teamData)
    {
        this.teamData = teamData;
        nameText.text = teamData.name + " -- " + teamData.GetScore() + " pts"; ;
        languageText.text = teamData.language.ToString();

        foreach (var playerData in teamData.players)
        {
            // Visuals
            var playerUI = Instantiate(playerPrefab, verticalLayoutGroup.transform).GetComponent<SkipPlayerUI>();
            playerUI.Initialize(playerData);
        }
    }

    private void SelectTeam(TeamData teamData)
    {
        if (this.teamData == teamData)
        {
            // Select this
            outline.enabled = true;
            animator.Play("Selected");
        }
        else
        {
            // Deselect
            outline.enabled = false;
            animator.Play("Idle");
        }
    }
}
