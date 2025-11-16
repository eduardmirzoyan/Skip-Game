using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkipGameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button correctButton;
    [SerializeField] private TextMeshProUGUI correctHotkey;
    [SerializeField] private Button endButton;
    [SerializeField] private TextMeshProUGUI endHotkey;
    [SerializeField] private Button penalityButton;
    [SerializeField] private TextMeshProUGUI penHotkey;
    [SerializeField] private Button skipButton;
    [SerializeField] private TextMeshProUGUI skipHotkey;
    [SerializeField] private Button nextRoundButton;
    [SerializeField] private TextMeshProUGUI nextRoundHotkey;
    [SerializeField] private Button redoRoundButton;
    [SerializeField] private TextMeshProUGUI redoRoundHotkey;

    [Header("Hotkeys")]
    [SerializeField] private Keybindings keybindings;

    [Header("Data")]
    [SerializeField] private LobbyData lobbyData;
    [SerializeField] private TeamData teamData;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private WordBank wordBank;

    [Header("Stats")]
    [SerializeField] private TurnData turnData;
    [SerializeField] private string currentWord;
    [SerializeField] private float remainingTime;

    private bool roundStarted;
    private bool roundOver;

    private void Start()
    {
        // Get saved data
        lobbyData = DataManager.instance.LoadLobbyData();

        // Get indexed team
        teamData = lobbyData.GetIndexedTeam();

        // Get indexed player
        playerData = teamData.GetIndexedPlayer();

        // Set word bank
        wordBank = lobbyData.wordBank;

        // Reset values
        currentWord = "";
        roundStarted = false;
        roundOver = false;
        turnData = new TurnData();
        turnData.Initialize(playerData);

        // Choose a random rule
        var rule = lobbyData.advancedSettings.GetRandomRestriction();

        SetupHotkeys();

        // Trigger events
        SkipGameEvents.instance.TriggerOnNewWord("Press SKIP to start", string.Empty);
        SkipGameEvents.instance.TriggerSetTurnRule(rule);
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.turnTime, lobbyData.turnTime);

        // Open scene
        TransitionManager.instance.OpenScene();
    }

    private void Update()
    {
        if (!roundOver)
        {
            if (Input.GetKeyDown(keybindings.skipKey))
            {
                // Press corresponding button
                ExecuteEvents.Execute(skipButton.gameObject, new BaseEventData(null), ExecuteEvents.submitHandler);
            }
        }

        if (roundStarted)
        {
            if (Input.GetKeyDown(keybindings.correctKey))
            {
                // Press corresponding button
                ExecuteEvents.Execute(correctButton.gameObject, new BaseEventData(null), ExecuteEvents.submitHandler);
            }
            else if (Input.GetKeyDown(keybindings.penalityKey))
            {
                // Press corresponding button
                ExecuteEvents.Execute(penalityButton.gameObject, new BaseEventData(null), ExecuteEvents.submitHandler);
            }
            else if (Input.GetKeyDown(keybindings.endKey))
            {
                // Press corresponding button
                ExecuteEvents.Execute(endButton.gameObject, new BaseEventData(null), ExecuteEvents.submitHandler);
            }
        }

        if (roundOver)
        {
            if (Input.GetKeyDown(keybindings.correctKey))
            {
                // Press corresponding button
                ExecuteEvents.Execute(redoRoundButton.gameObject, new BaseEventData(null), ExecuteEvents.submitHandler);
            }
            else if (Input.GetKeyDown(keybindings.skipKey))
            {
                // Press corresponding button
                ExecuteEvents.Execute(nextRoundButton.gameObject, new BaseEventData(null), ExecuteEvents.submitHandler);
            }
        }
    }

    private void SetupHotkeys()
    {
        keybindings = DataManager.instance.LoadKeybindings();

        correctHotkey.text = $"[{keybindings.correctKey}]";
        skipHotkey.text = $"[{keybindings.skipKey}]";
        penHotkey.text = $"[{keybindings.penalityKey}]";
        endHotkey.text = $"[{keybindings.endKey}]";

        nextRoundHotkey.text = $"[{keybindings.skipKey}]";
        redoRoundHotkey.text = $"[{keybindings.correctKey}]";
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
        turnData.numCorrect++;

        // Store word
        turnData.encounteredWords.Add((currentWord, true));

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
        turnData.score += lobbyData.advancedSettings.pointsOnSkip;

        // Store word
        turnData.encounteredWords.Add((currentWord, false));

        // Trigger event
        SkipGameEvents.instance.TriggerOnScoreChanged(turnData.score, lobbyData.advancedSettings.pointsOnSkip);

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
        turnData.numPenalties++;

        // Reduce score
        DecreaseScore();
    }

    private void GetNewWord()
    {
        // Increment
        turnData.numWords++;

        // Get a different word based on language
        int randomIndex = wordBank.GetRandomWordIndex();
        string subWord = wordBank.PeakWordAtIndex(randomIndex, Language.English);
        currentWord = wordBank.PopWordAtIndex(randomIndex, teamData.language); // wordBank.GetRandomWord(teamData.language);

        // Trigger event
        SkipGameEvents.instance.TriggerOnNewWord(currentWord, subWord);
    }

    private void AddTime(float amount)
    {
        // Update time with clamping
        remainingTime += amount;
        remainingTime = Mathf.Clamp(remainingTime, 0, lobbyData.turnTime);
    }

    public void End()
    {
        roundOver = true;

        // Add word
        turnData.encounteredWords.Add((currentWord, false));

        // Stop timer
        StopAllCoroutines();

        // Stop audio
        AudioManager.instance.StopImm("Background 4");

        // Play audio
        AudioManager.instance.PlayImm("Turn End");
        AudioManager.instance.Play("Background 3");

        // Trigger event
        SkipGameEvents.instance.TriggerOnEnd(turnData);
    }

    public void IncreaseScore()
    {
        // Increase score value
        turnData.score += lobbyData.advancedSettings.pointsOnCorrect;

        // Trigger event
        SkipGameEvents.instance.TriggerOnScoreChanged(turnData.score, lobbyData.advancedSettings.pointsOnCorrect);
    }

    public void DecreaseScore()
    {
        // Reduce score value
        turnData.score -= lobbyData.advancedSettings.pointsOnCorrect;

        // Trigger event
        SkipGameEvents.instance.TriggerOnScoreChanged(turnData.score, -lobbyData.advancedSettings.pointsOnCorrect);
    }

    public void NextRound()
    {
        // Give points to player
        playerData.score += turnData.score;

        // Increment this player first
        lobbyData.GetIndexedTeam().IncrementIndex();

        // Change team
        lobbyData.IncrementIndex();

        // Save
        DataManager.instance.SaveLobbyData(lobbyData);

        // Load Previous Scene
        TransitionManager.instance.LoadPreviousScene();
    }

    public void RedoRound()
    {
        // Reset values
        roundStarted = false;
        roundOver = false;

        turnData = new TurnData();
        turnData.Initialize(playerData);

        // Disable buttons
        correctButton.interactable = false;
        endButton.interactable = false;
        penalityButton.interactable = false;

        // Trigger events
        SkipGameEvents.instance.TriggerOnScoreChanged(turnData.score, 0);
        SkipGameEvents.instance.TriggerOnNewWord("Press SKIP to start", string.Empty);
        SkipGameEvents.instance.TriggerOnSetPlayer(playerData);
        SkipGameEvents.instance.TriggerOnTimeChanged(lobbyData.turnTime, lobbyData.turnTime);

        SkipGameEvents.instance.TriggerOnRedo();
    }
}
