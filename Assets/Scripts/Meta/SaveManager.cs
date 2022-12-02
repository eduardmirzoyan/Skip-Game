using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveData(LobbyData lobbyData)
    {
        // BinaryFormatter formatter = new BinaryFormatter();
        // string path = Application.persistentDataPath + "/save.bin";
        // FileStream stream = new FileStream(path, FileMode.Create);

        // formatter.Serialize(stream, lobbyData);
        // stream.Close();

        // Create json
        var json = JsonUtility.ToJson(lobbyData);
        // Set path
        string path = Application.dataPath + "/save.txt";
        // Write to file
        File.WriteAllText(path, json);
        // Debug
        Debug.Log("Saved to: " + path);
    }

    public static LobbyData LoadData()
    {
        string path = Application.dataPath + "/save.txt";
        
        if (File.Exists(path))
        {
            // BinaryFormatter formatter = new BinaryFormatter();
            // FileStream stream = new FileStream(path, FileMode.Open);

            // LobbyData lobbyData = formatter.Deserialize(stream) as LobbyData;

            // Read from file
            string saveString = File.ReadAllText(path);
            // Decode to data
            LobbyData lobbyData = JsonUtility.FromJson<LobbyData>(saveString);

            // Sucessfully loaded save
            Debug.Log("Saved to: " + Application.dataPath);

            return lobbyData;
        }

        // Error!
        Debug.Log("Save not found in: " + path);
        return null;
    }
}
