using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode
{
    Cup,
    Free,
    Chrono
}


[CreateAssetMenu(fileName = "Game configuration", menuName = "Race data/Game Configuration")]
public class GameConfiguration : ScriptableObject
{
    public List<InputDevice> devices;
    public List<string> circuits;
    public int nextCircuit = 0;
    public GameMode mode;
}
