using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilenceAnnouncerManger : PersistentSingleton<SilenceAnnouncerManger>
{
    [Header("Data")]
    [SerializeField] private Keybindings keybindings;

    private const string SILENCE_KEY = "Silence";

    private void Start()
    {
        keybindings = DataManager.instance.LoadKeybindings();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keybindings.silenceKey))
            AudioManager.instance.PlayImm(SILENCE_KEY);
    }
}