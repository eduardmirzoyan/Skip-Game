using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TurnData
{
    public int score;
    public int numCorrect;
    public int numWords;
    public int numPenalties;
    public PlayerData playerData;

    public List<(string, bool)> encounteredWords; // bool - guessed?

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;

        encounteredWords = new List<(string, bool)>();
        numWords = -1;
    }
}
