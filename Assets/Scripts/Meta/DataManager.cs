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

    public void SaveData(LobbyData lobbyData)
    {
        this.lobbyData = lobbyData;
    }

    public LobbyData LoadData()
    {
        return lobbyData;
    }
}
