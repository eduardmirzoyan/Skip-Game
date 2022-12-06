using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipGameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button correctButton;
    [SerializeField] private Button endButton;

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

        // Choose a random rule
        var rule = lobbyData.advancedSettings.GetRandomRestriction();

        // Trigger events
        SkipGameEvents.instance.TriggerSetTurnRule(rule);
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.turnTime, lobbyData.turnTime);

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
        // Enable buttons
        correctButton.interactable = true;
        endButton.interactable = true;

        // Start background music
        // AudioManager.instance.Play("Turn Start 0");

        // Start timer
        StartCoroutine(StartTimer(lobbyData.turnTime));
    }

    private IEnumerator StartTimer(float duration)
    {
        float remaining = duration;
        while (remaining >= 0)
        {
            // Trigger event
            SkipGameEvents.instance.TriggerOnTimeChanged(remaining, duration);

            // Decrement
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

        // Play audio

        // Trigger event
        SkipGameEvents.instance.TriggerOnCorrectWord(numberOfCorrect * lobbyData.advancedSettings.pointsOnCorrect);

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

        // Play audio

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

        // Play audio
        AudioManager.instance.PlayImm("Turn End 0");

        // Trigger event
        SkipGameEvents.instance.TriggerOnEnd(numberOfCorrect, numberOfWords, lobbyData.turnTime, playerData);
    }

    public void NextRound()
    {
        // Give points to player
        playerData.score += numberOfCorrect * lobbyData.advancedSettings.pointsOnCorrect;
        
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

        // Disable buttons
        correctButton.interactable = false;
        endButton.interactable = false;

        // Trigger events
        SkipGameEvents.instance.TriggerOnCorrectWord(0);
        SkipGameEvents.instance.TriggerOnNewWord("Press SKIP to start");
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.turnTime, lobbyData.turnTime);

        SkipGameEvents.instance.TriggerOnRedo();
    }
}
