using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipEndManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform placementsScreenTransform;
    [SerializeField] private Transform moreStatsScreenTransform;

    [Header("Data")]
    [SerializeField] private LobbyData lobbyData;
    [SerializeField] private float transitionDuration = 2f;

    private void Start()
    {
        lobbyData = DataManager.instance.LoadLobbyData();

        StartCoroutine(DelayedStart(0.25f));
    }

    private IEnumerator DelayedStart(float duration)
    {
        yield return new WaitForSeconds(duration);

        SkipEndEvents.instance.TriggerOnEnterEndGame(lobbyData);
        TransitionManager.instance.OpenScene();
        AudioManager.instance.PlayImm("FanFare");
    }

    public void GoToMoreStats()
    {
        LeanTween.moveLocalX(placementsScreenTransform.gameObject, -3840f, transitionDuration).setEaseInOutQuad();
        LeanTween.moveLocalX(moreStatsScreenTransform.gameObject, 0f, transitionDuration).setEaseInOutQuad();
    }

    public void GoToPlacements()
    {
        LeanTween.moveLocalX(placementsScreenTransform.gameObject, 0f, transitionDuration).setEaseInOutQuad();
        LeanTween.moveLocalX(moreStatsScreenTransform.gameObject, 3840f, transitionDuration).setEaseInOutQuad();
    }

    public void NewGame()
    {
        // Clear lobby
        lobbyData.Reset();

        // Save
        DataManager.instance.SaveLobbyData(lobbyData);

        // Change scnes
        TransitionManager.instance.LoadSceneAt(1);
    }

    public void MainMenu()
    {
        // Clear lobby
        DataManager.instance.SaveLobbyData(null);

        // Change scnes
        TransitionManager.instance.LoadMainMenuScene();
    }
}
