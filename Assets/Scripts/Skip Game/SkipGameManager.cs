using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipGameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button correctButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button penalityButton;

    [Header("Hotkeys")]
    [SerializeField] private KeyCode correctKey = KeyCode.C;
    [SerializeField] private KeyCode skipKey = KeyCode.S;
    [SerializeField] private KeyCode penalityKey = KeyCode.D;
    [SerializeField] private KeyCode endKey = KeyCode.E;

    [Header("Data")]
    [SerializeField] private LobbyData lobbyData;
    [SerializeField] private TeamData teamData;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private WordBank wordBank;

    [Header("Stats")]
    [SerializeField] private int numberOfCorrect;
    [SerializeField] private int numberOfWords;
    [SerializeField] private int score;
    [SerializeField] private float remainingTime;
    [SerializeField] private int penaltyCount;

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
        numberOfWords = -1;
        roundStarted = false;
        penaltyCount = 0;

        // Choose a random rule
        var rule = lobbyData.advancedSettings.GetRandomRestriction();

        // Trigger events
        SkipGameEvents.instance.TriggerOnNewWord("Press SKIP to start");
        SkipGameEvents.instance.TriggerSetTurnRule(rule);
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.turnTime, lobbyData.turnTime);

        // Open scene
        TransitionManager.instance.OpenScene();
    }

    private void Update()
    {
        // Always allow skip
        if (Input.GetKeyDown(skipKey))
        {
            Skip();
        }

        if (!roundStarted) return;

        // Check for hotkeys
        if (Input.GetKeyDown(correctKey))
        {
            Correct();
        }
        else if (Input.GetKeyDown(penalityKey))
        {
            Penalty();
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
        penalityButton.interactable = true;

        // Start background music
        AudioManager.instance.StopImm("Background 3");
        AudioManager.instance.Play("Background 4");

        // Start timer
        StartCoroutine(StartTimer(lobbyData.turnTime));
    }

    private IEnumerator StartTimer(float duration)
    {
        remainingTime = duration;
        while (remainingTime >= 0)
        {
            // Trigger event
            SkipGameEvents.instance.TriggerOnTimeChanged(remainingTime, duration);

            // Decrement
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        // Automatically end
        End();
    }

    public void Correct()
    {
        // Play audio
        AudioManager.instance.PlayImm("Correct");

        // Increment points
        numberOfCorrect++;

        // Increase score
        IncreaseScore();

        // Update time
        AddTime(lobbyData.advancedSettings.timeOnCorrect);

        // Then get new word
        GetNewWord();
    }

    public void Skip()
    {
        if (!roundStarted)
        {
            // Start the game
            StartRound();

            // Set flag
            roundStarted = true;

            // Get next word
            GetNewWord();

            // Finish
            return;
        }

        // Update score
        score += lobbyData.advancedSettings.pointsOnSkip;

        // Trigger event
        SkipGameEvents.instance.TriggerOnScoreChanged(score, lobbyData.advancedSettings.pointsOnSkip);

        // Update time
        AddTime(lobbyData.advancedSettings.timeOnSkip);

        // Get next word
        GetNewWord();
    }

    public void Penalty()
    {
        // Play audio
        AudioManager.instance.PlayImm("Penalty");

        // Incremenet
        penaltyCount++;

        // Reduce score
        DecreaseScore();
    }

    private void GetNewWord()
    {
        // Increment
        numberOfWords++;

        // Get a different word based on language
        string word = wordBank.GetRandomWord(teamData.language);

        // Trigger event
        SkipGameEvents.instance.TriggerOnNewWord(word);
    }
    
    private void AddTime(float amount)
    {
        // Update time with clamping
        remainingTime += amount;
        remainingTime = Mathf.Clamp(remainingTime, 0, lobbyData.turnTime);
    }

    public void End()
    {
        // Stop timer
        StopAllCoroutines();

        // Stop audio
        AudioManager.instance.StopImm("Background 4");
        
        // Play audio
        AudioManager.instance.PlayImm("Turn End");
        AudioManager.instance.Play("Background 3");

        // Trigger event
        SkipGameEvents.instance.TriggerOnEnd(score, numberOfCorrect, numberOfWords, penaltyCount, lobbyData.turnTime, playerData);
    }

    public void IncreaseScore()
    {
        // Increase score value
        score += lobbyData.advancedSettings.pointsOnCorrect;

        // Trigger event
        SkipGameEvents.instance.TriggerOnScoreChanged(score, lobbyData.advancedSettings.pointsOnCorrect);
    }

    public void DecreaseScore()
    {
        // Reduce score value
        score -= lobbyData.advancedSettings.pointsOnCorrect;

        // Trigger event
        SkipGameEvents.instance.TriggerOnScoreChanged(score, -lobbyData.advancedSettings.pointsOnCorrect);
    }

    public void NextRound()
    {
        // Give points to player
        playerData.score += score;
        
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
        numberOfWords = -1;
        score = 0;
        roundStarted = false;
        penaltyCount = 0;

        // Disable buttons
        correctButton.interactable = false;
        endButton.interactable = false;
        penalityButton.interactable = false;

        // Trigger events
        SkipGameEvents.instance.TriggerOnScoreChanged(score, 0);
        SkipGameEvents.instance.TriggerOnNewWord("Press SKIP to start");
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.turnTime, lobbyData.turnTime);

        SkipGameEvents.instance.TriggerOnRedo();
    }
}
