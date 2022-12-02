using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplayUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI displayText;

    private void Start()
    {
        // Sub
        SkipGameEvents.instance.onSetPlayer += DisplayPlayer;
    }

    private void OnDestroy()
    {
        // Sub
        SkipGameEvents.instance.onSetPlayer -= DisplayPlayer;
    }

    private void DisplayPlayer(PlayerData playerData)
    {
        // Show
        displayText.text = "Player: " + playerData.name + "\nTeam: " + playerData.teamData.name;
    }
}
