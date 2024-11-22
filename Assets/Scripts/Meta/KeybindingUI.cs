using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum KeyType { Correct, Skip, Penalty, End }

public class KeybindingUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI buttonLabel;

    [Header("Setting")]
    [SerializeField] private KeyType keyType;
    [SerializeField] private KeyCode cancelKey = KeyCode.Escape;

    [Header("Debug")]
    [SerializeField] private bool awaitingInput;

    private Dictionary<KeyType, KeyCode> defaultHotkeys;

    private void Start()
    {
        defaultHotkeys = new Dictionary<KeyType, KeyCode>()
        {
            { KeyType.Correct, KeyCode.A },
            { KeyType.Skip, KeyCode.S },
            { KeyType.Penalty, KeyCode.D },
            { KeyType.End, KeyCode.F }
        };

        ResetToSavedValue();
    }

    private void Update()
    {
        if (awaitingInput)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    if (keyCode == cancelKey)
                    {
                        ResetToSavedValue();

                        awaitingInput = false;
                        break;
                    }

                    string hotkey = keyCode.ToString();
                    buttonLabel.color = Color.black;
                    buttonLabel.text = hotkey;

                    int hotkeyID = (int)keyCode;
                    PlayerPrefs.SetInt(keyType.ToString(), hotkeyID);
                    PlayerPrefs.Save();

                    awaitingInput = false;
                    break;
                }
            }
        }
    }

    public void ChangeKey()
    {
        buttonLabel.color = Color.red;
        buttonLabel.text = "Press ANY key";

        awaitingInput = true;
    }

    public void ResetKey()
    {
        KeyCode keyCode = defaultHotkeys[keyType];

        buttonLabel.text = keyCode.ToString();
        buttonLabel.color = Color.black;
        awaitingInput = false;

        PlayerPrefs.SetInt(keyType.ToString(), (int)keyCode);
        PlayerPrefs.Save();
    }

    private void ResetToSavedValue()
    {
        int hotkeyID = PlayerPrefs.GetInt(keyType.ToString(), -1);
        string hotkeyValue;

        if (hotkeyID == -1)
        {
            KeyCode keyCode = defaultHotkeys[keyType];
            hotkeyValue = keyCode.ToString();

            PlayerPrefs.SetInt(keyType.ToString(), (int)keyCode);
            PlayerPrefs.Save();
        }
        else
        {
            KeyCode keyCode = (KeyCode)hotkeyID;
            hotkeyValue = keyCode.ToString();
        }

        buttonLabel.color = Color.black;
        buttonLabel.text = hotkeyValue;
    }
}
