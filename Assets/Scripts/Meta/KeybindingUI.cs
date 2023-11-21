using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeybindingUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI buttonLabel;

    [Header("Debug")]
    [SerializeField] private bool awaitingInput;

    private void Start()
    {
        buttonLabel.text = "--";
    }

    private void Update()
    {
        if (awaitingInput)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    buttonLabel.color = Color.black;
                    buttonLabel.text = keyCode.ToString();

                    awaitingInput = false;
                    break;
                }
            }
        }
    }

    public void ChangeKey()
    {
        buttonLabel.color = Color.white;
        buttonLabel.text = "Press ANY key.";

        awaitingInput = true;
    }
}
