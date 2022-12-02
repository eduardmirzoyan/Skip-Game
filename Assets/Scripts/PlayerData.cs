using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public new string name;
    public int score; /// How many points this player has earned
    public TeamData teamData;

    public void Initialize(string name, TeamData teamData = null)
    {
        this.name = name;
        score = 0;
        this.teamData = teamData;
    }

}