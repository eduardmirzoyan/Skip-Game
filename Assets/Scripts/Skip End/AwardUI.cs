using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AwardType
{
    MostSkipped,
    MostPointsInRound,
    LongestGuessed,
    LongestTime,
}

public class AwardUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI awardNameText;
    [SerializeField] private TextMeshProUGUI awardResultText;

    [Header("Data")]
    [SerializeField] private AwardType awardType;

    private void Start()
    {
        // Sub
        SkipEndEvents.instance.OnEnterEndGame += UpdateAward;
    }

    private void OnDestroy()
    {
        // Unsub
        SkipEndEvents.instance.OnEnterEndGame -= UpdateAward;
    }

    private void UpdateAward(LobbyData data)
    {
        PlayerData topPlayer = null;
        switch (awardType)
        {
            case AwardType.MostSkipped:
                {
                    foreach (var team in data.teams)
                    {
                        foreach (var player in team.players)
                        {
                            if (topPlayer == null)
                                topPlayer = player;
                            else if (player.stats.numSkips > topPlayer.stats.numSkips)
                                topPlayer = player;
                        }
                    }
                    awardNameText.text = "Skeeped the most words";
                    awardResultText.text = $"{topPlayer.name}\n({topPlayer.stats.numSkips} skeeps)";
                }
                break;
            case AwardType.MostPointsInRound:
                {
                    foreach (var team in data.teams)
                    {
                        foreach (var player in team.players)
                        {
                            if (topPlayer == null)
                                topPlayer = player;
                            else if (player.stats.maxPointsInRound > topPlayer.stats.maxPointsInRound)
                                topPlayer = player;
                        }
                    }
                    awardNameText.text = "Most points in one round";
                    awardResultText.text = $"{topPlayer.name}\n({topPlayer.stats.maxPointsInRound} points)";
                }

                break;
            case AwardType.LongestGuessed:
                {
                    foreach (var team in data.teams)
                    {
                        foreach (var player in team.players)
                        {
                            if (topPlayer == null)
                                topPlayer = player;
                            else if (player.stats.longestWord.Length > topPlayer.stats.longestWord.Length)
                                topPlayer = player;
                        }
                    }
                    awardNameText.text = "Longest guessed word";
                    awardResultText.text = $"{topPlayer.name}\n({topPlayer.stats.longestWord})\n({topPlayer.stats.longestWord.Length} letters)";
                }
                break;
            case AwardType.LongestTime:
                {
                    foreach (var team in data.teams)
                    {
                        foreach (var player in team.players)
                        {
                            if (topPlayer == null)
                                topPlayer = player;
                            else if (player.stats.longestTimeSeconds > topPlayer.stats.longestTimeSeconds)
                                topPlayer = player;
                        }
                    }
                    awardNameText.text = "Longest time to guess";
                    awardResultText.text = $"{topPlayer.name}\n({topPlayer.stats.longestTimeSeconds:F2} seconds)";
                }
                break;
        }
    }
}
