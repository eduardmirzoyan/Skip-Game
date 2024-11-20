using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipOverlayManager : MonoBehaviour
{
    [SerializeField] private LobbyData lobbyData;

    const float WAIT_TIME = 0.1f;

    private void Start()
    {
        // Get saved data
        lobbyData = DataManager.instance.LoadData();

        // Trigger event
        SkipOverlayEvents.instance.TriggerOnEnterOverlay(lobbyData);

        StartCoroutine(Wait(WAIT_TIME));
    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Check if game is over
        if (lobbyData.GameOver())
        {
            // Trigger event
            SkipOverlayEvents.instance.TriggerOnEnd(lobbyData);

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
        DataManager.instance.SaveData(lobbyData);

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
            SkipOverlayEvents.instance.TriggerOnEnd(lobbyData);
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
        // End the game prematurely

        // Trigger event
        SkipOverlayEvents.instance.TriggerOnEnd(lobbyData);
    }

    public void NewGame()
    {
        // Clear lobby
        lobbyData.Reset();

        // Save
        DataManager.instance.SaveData(lobbyData);

        // Change scnes
        TransitionManager.instance.LoadPreviousScene();
    }

    public void MainMenu()
    {
        // Clear lobby
        DataManager.instance.SaveData(null);

        // Change scnes
        TransitionManager.instance.LoadMainMenuScene();
    }
}
