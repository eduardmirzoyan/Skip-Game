using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExtraRuleUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI ruleNameText;
    [SerializeField] private TooltipTriggerUI tooltipTriggerUI;

    [Header("Data")]
    [SerializeField] private ExtraRule extraRule;

    private void Start()
    {
        ruleNameText.text = extraRule.name;
        tooltipTriggerUI.SetTooltip(extraRule.name, extraRule.description);
    }
}
