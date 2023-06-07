using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Game configuration", menuName = "Race data/Game Configuration")]
public class GameConfiguration : ScriptableObject
{
    public List<InputDevice> devices;
}
