using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkipOverlayManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startHotkey;

    [Header("Data")]
    [SerializeField] private LobbyData lobbyData;
    [SerializeField] private Keybindings keybindings;

    const float WAIT_TIME = 0.1f;

    private void Start()
    {
        // Get saved data
        lobbyData = DataManager.instance.LoadLobbyData();

        // Trigger event
        SkipOverlayEvents.instance.TriggerOnEnterOverlay(lobbyData);

        // Setup hotkey
        SetupHotkeys();

        StartCoroutine(DelayedStart(WAIT_TIME));
    }

    private void Update()
    {
        if (Input.GetKeyDown(keybindings.skipKey))
        {
            // Press corresponding button
            ExecuteEvents.Execute(startButton.gameObject, new BaseEventData(null), ExecuteEvents.submitHandler);
        }
    }

    private void SetupHotkeys()
    {
        keybindings = DataManager.instance.LoadKeybindings();
        startHotkey.text = $"[{keybindings.skipKey}]";
    }

    private IEnumerator DelayedStart(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Check if game is over
        if (lobbyData.GameOver())
        {
            // Trigger event
            // SkipOverlayEvents.instance.TriggerOnEnd(lobbyData);
            EndGame();
        }
        else
        {
            // Select team
            SkipOverlayEvents.instance.TriggerOnSelectTeam(lobbyData.GetIndexedTeam());

            // Select player on that team
            SkipOverlayEvents.instance.TriggerOnSelectPlayer(lobbyData.GetIndexedTeam().GetIndexedPlayer());
        }

        // Open scene
        TransitionManager.instance.OpenScene();
    }

    public void StartRound()
    {
        // Save
        DataManager.instance.SaveLobbyData(lobbyData);

        // Load Scene
        TransitionManager.instance.LoadNextScene();
    }

    public void SkipTeam()
    {
        // Change team index
        lobbyData.IncrementIndex();

        var team = lobbyData.GetIndexedTeam();

        // Select team
        SkipOverlayEvents.instance.TriggerOnSelectTeam(team);

        // Select player on that team
        SkipOverlayEvents.instance.TriggerOnSelectPlayer(team.GetIndexedPlayer());

        // Check if game is over
        if (lobbyData.GameOver())
        {
            // Trigger event
            // SkipOverlayEvents.instance.TriggerOnEnd(lobbyData);

            EndGame();
        }
    }

    public void SkipPlayer()
    {
        var team = lobbyData.GetIndexedTeam();

        // Change player index
        team.IncrementIndex();

        // Trigger event
        SkipOverlayEvents.instance.TriggerOnSelectPlayer(team.GetIndexedPlayer());
    }

    public void SkipLanguage()
    {
        var team = lobbyData.GetIndexedTeam();

        // Change to next language
        team.IncrementLanguage();

        // Trigger event
        SkipOverlayEvents.instance.TriggerOnSelectLanguage(team);
    }

    public void EndGame()
    {
        // // End the game prematurely

        // // Trigger event
        // SkipOverlayEvents.instance.TriggerOnEnd(lobbyData);

        // Save
        DataManager.instance.SaveLobbyData(lobbyData);

        // Load Scene
        TransitionManager.instance.LoadSceneAt(4);
    }

    public void NewGame()
    {
        // Clear lobby
        lobbyData.Reset();

        // Save
        DataManager.instance.SaveLobbyData(lobbyData);

        // Change scnes
        TransitionManager.instance.LoadPreviousScene();
    }

    public void MainMenu()
    {
        // Clear lobby
        DataManager.instance.SaveLobbyData(null);

        // Change scnes
        TransitionManager.instance.LoadMainMenuScene();
    }
}
