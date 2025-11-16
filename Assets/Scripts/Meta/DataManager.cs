using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private LobbyData lobbyData;

    public static DataManager instance;
    private void Awake()
    {
        // Singleton Logic
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        instance = this;
    }

    public void SaveLobbyData(LobbyData lobbyData)
    {
        this.lobbyData = lobbyData;
    }

    public LobbyData LoadLobbyData()
    {
        return lobbyData;
    }

    public Keybindings LoadKeybindings()
    {
        Keybindings keybindings = new();

        int keycodeID = PlayerPrefs.GetInt(KeyType.Correct.ToString(), -1);
        keybindings.correctKey = keycodeID == -1 ? KeyCode.None : (KeyCode)keycodeID;

        keycodeID = PlayerPrefs.GetInt(KeyType.Skip.ToString(), -1);
        keybindings.skipKey = keycodeID == -1 ? KeyCode.None : (KeyCode)keycodeID;

        keycodeID = PlayerPrefs.GetInt(KeyType.Penalty.ToString(), -1);
        keybindings.penalityKey = keycodeID == -1 ? KeyCode.None : (KeyCode)keycodeID;

        keycodeID = PlayerPrefs.GetInt(KeyType.End.ToString(), -1);
        keybindings.endKey = keycodeID == -1 ? KeyCode.None : (KeyCode)keycodeID;

        keycodeID = PlayerPrefs.GetInt(KeyType.Silence.ToString(), -1);
        keybindings.silenceKey = keycodeID == -1 ? KeyCode.None : (KeyCode)keycodeID;

        return keybindings;
    }
}
