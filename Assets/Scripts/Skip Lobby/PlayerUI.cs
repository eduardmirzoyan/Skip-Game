using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject removePlayerButton;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Data")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float scaleTime = 0.1f;

    private void Awake()
    {
        // Sub to events
        LobbyEvents.instance.OnRemovePlayer += RemovePlayer;
        LobbyEvents.instance.OnAddPlayer += AddPlayer;
    }

    private void OnDestroy()
    {
        // Unsub to events
        LobbyEvents.instance.OnRemovePlayer -= RemovePlayer;
        LobbyEvents.instance.OnAddPlayer -= AddPlayer;
    }

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        nameInput.text = playerData.name;

        // Check to see if it's this team and only 1 left
        if (playerData.teamData.Size > 1)
        {
            // Enable
            removePlayerButton.SetActive(true);
        }

        // Grow in
        StartCoroutine(GrowIn(scaleTime));
    }

    public void RemovePlayerButton()
    {
        // Controller -> Logic
        // LobbyManager.instance.RemovePlayer(playerData, playerData.teamData);

        var teamData = playerData.teamData;

        // Error check
        if (teamData == null || playerData == null) return;

        // Remove player
        teamData.RemovePlayer(playerData);

        // Trigger event
        LobbyEvents.instance.TriggerRemovePlayer(playerData, teamData);
    }

    public void ChangePlayerName(string newName)
    {
        // Change name
        playerData.name = newName;
    }

    private void AddPlayer(PlayerData playerData, TeamData teamData)
    {
        if (this.playerData != playerData)
        {
            // Check to see if it's this team and only 1 left
            if (this.playerData.teamData == teamData && teamData.Size > 1)
            {
                // Prevent deleting
                removePlayerButton.SetActive(true);
            }

            return;
        }
    }

    private void RemovePlayer(PlayerData playerData, TeamData teamData)
    {
        if (this.playerData != playerData)
        {
            // Check to see if it's this team and only 1 left
            if (this.playerData.teamData == teamData && teamData.Size <= 1)
            {
                // Prevent deleting
                removePlayerButton.SetActive(false);
            }

            return;
        }

        // Grow in
        StartCoroutine(ShrinkOut(scaleTime));

        // Destroy(this.gameObject);
    }

    private IEnumerator ShrinkOut(float duration)
    {
        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp scale
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Destroy
        Destroy(this.gameObject);
    }

    private IEnumerator GrowIn(float duration)
    {
        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp scale
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set scale
        transform.localScale = Vector3.one;

        // Allow interaction
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
