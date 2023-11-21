using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Keybindings : ScriptableObject
{
    public KeyCode correctKey = KeyCode.C;
    public KeyCode skipKey = KeyCode.S;
    public KeyCode penalityKey = KeyCode.D;
    public KeyCode endKey = KeyCode.E;
}
