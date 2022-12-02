using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipGameManager : MonoBehaviour
{
    [Header("Hotkeys")]
    [SerializeField] private KeyCode correctKey = KeyCode.C;
    [SerializeField] private KeyCode skipKey = KeyCode.S;
    [SerializeField] private KeyCode endKey = KeyCode.E;

    [Header("Data")]
    [SerializeField] private LobbyData lobbyData;
    [SerializeField] private TeamData teamData;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private WordBank wordBank;

    [Header("Stats")]
    [SerializeField] private int numberOfCorrect;
    [SerializeField] private int numberOfWords;

    private bool roundStarted;

    private void Start()
    {
        // Get saved data
        lobbyData = DataManager.instance.LoadData();

        // Get indexed team
        teamData = lobbyData.GetIndexedTeam();

        // Get indexed player
        playerData = teamData.GetIndexedPlayer();

        // Set word bank
        wordBank = lobbyData.wordBank;

        // Reset values
        numberOfCorrect = 0;
        numberOfWords = 0;
        roundStarted = false;

        // Trigger events
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.roundTime, lobbyData.roundTime);

        // Open scene
        TransitionManager.instance.OpenScene();
    }

    private void Update()
    {
        // Check for hotkeys
        if (Input.GetKeyDown(correctKey))
        {
            Correct();
        }
        else if (Input.GetKeyDown(skipKey))
        {
            Skip();
        }
        else if (Input.GetKeyDown(endKey))
        {
            End();
        }
    }

    public void StartRound()
    {
        // Start timer
        StartCoroutine(StartTimer(lobbyData.roundTime));
    }

    private IEnumerator StartTimer(float duration)
    {
        float remaining = duration;
        while (remaining >= 0)
        {
            // Trigger event
            SkipGameEvents.instance.TriggerOnTimeChanged(remaining, duration);

            remaining -= Time.deltaTime;
            yield return null;
        }

        // Automatically end
        End();
    }

    public void Correct()
    {
        // Incrment pts
        numberOfCorrect++;

        // Trigger event
        SkipGameEvents.instance.TriggerOnCorrectWord(numberOfCorrect);

        // Then just skip
        Skip();
    }

    public void Skip()
    {
        if (!roundStarted)
        {
            // Start the game
            StartRound();

            // Set flag
            roundStarted = true;
        }

        // Increment
        numberOfWords++;

        // Get a different word based on language
        string word = wordBank.GetRandomWord(teamData.language);

        // Trigger event
        SkipGameEvents.instance.TriggerOnNewWord(word);
    }

    public void End()
    {
        // Stop timer
        StopAllCoroutines();

        // Trigger event
        SkipGameEvents.instance.TriggerOnEnd(numberOfCorrect, numberOfWords, lobbyData.roundTime, playerData);
    }

    public void NextRound()
    {
        // Give points to player
        playerData.score += numberOfCorrect;
        
        // Increment this player first
        lobbyData.GetIndexedTeam().IncrementIndex();

        // Change team
        lobbyData.IncrementIndex();

        // Save
        DataManager.instance.SaveData(lobbyData);

        // Load Previous Scene
        TransitionManager.instance.LoadPreviousScene();
    }

    public void RedoRound()
    {
        // Reset values
        numberOfCorrect = 0;
        numberOfWords = 0;
        roundStarted = false;

        // Trigger events
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.roundTime, lobbyData.roundTime);
    }
}
