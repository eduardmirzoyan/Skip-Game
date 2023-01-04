using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipOverlayManager : MonoBehaviour
{
    [SerializeField] private LobbyData lobbyData;

    private void Start() 
    {
        // Get saved data
        lobbyData = DataManager.instance.LoadData();

        // Trigger event
        SkipOverlayEvents.instance.TriggerOnEnterOverlay(lobbyData);

        StartCoroutine(Wait(0.1f));
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

        // Select team
        SkipOverlayEvents.instance.TriggerOnSelectTeam(lobbyData.GetIndexedTeam());

        // Select player on that team
        SkipOverlayEvents.instance.TriggerOnSelectPlayer(lobbyData.GetIndexedTeam().GetIndexedPlayer());

        // Check if game is over
        if (lobbyData.GameOver())
        {
            // Trigger event
            SkipOverlayEvents.instance.TriggerOnEnd(lobbyData);
        }
    }

    public void SkipPlayer()
    {
        // Change player index
        lobbyData.GetIndexedTeam().IncrementIndex();

        // Select player on that team
        SkipOverlayEvents.instance.TriggerOnSelectPlayer(lobbyData.GetIndexedTeam().GetIndexedPlayer());
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
        DataManager.instance.SaveData(null);

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
