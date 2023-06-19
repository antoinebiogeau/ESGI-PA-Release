using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = System.Object;


public class InputManagerEvents : MonoBehaviour
{
    private const int MAX_DEVICES = 4;

    [SerializeField] private GameConfiguration gameConfig;

    [SerializeField] private List<RawImage> playerImages;
    
    [SerializeField] private bool checkForInputs = false;

    public bool CheckForInputs
    {
        get => checkForInputs;
        set => checkForInputs = value;
    }

    private bool isKeyboardActive;

    private List<InputDevice> activeDevices = new();
    private Keyboard keyboard;

    void Start()
    {
        keyboard = InputSystem.GetDevice<Keyboard>();
    }
    private void Update()
    {
        if (!checkForInputs) return;
        Debug.Log("Detecting devices");
        DetectDevices();
        UpdateUI();
    }

    private void DetectDevices()
    {
        DetectKeyboard();
        DetectGamepads();
    }

    private void DetectKeyboard()
    {
        if (keyboard.anyKey.isPressed && !keyboard.backspaceKey.isPressed && !isKeyboardActive && activeDevices.Count < MAX_DEVICES)
        {
            isKeyboardActive = true;
            activeDevices.Add(keyboard);
        }
        else if (keyboard.backspaceKey.isPressed && isKeyboardActive)
        {
            isKeyboardActive = false;
            activeDevices.Remove(keyboard);
        }
    }

    private void DetectGamepads()
    {
        //On regarde tous les gamepads dispo, si parmi l'un de ces gamepads, l'une des touches est jouée, on active le gamepad
        var gamepads = Gamepad.all.ToArray();
        foreach (var gamepad in gamepads)
        {
            if (gamepad.buttonSouth.isPressed && !activeDevices.Contains(gamepad) && activeDevices.Count < MAX_DEVICES)
            {
                activeDevices.Add(gamepad);
            }
        }
        foreach (var activeDevice in activeDevices)
        {
            if (activeDevice is not Gamepad) continue;
            var device = (Gamepad)activeDevice;
            if (Gamepad.all.ToArray().Contains(device) && !device.buttonEast.isPressed) continue;
            activeDevices.Remove(device);
            break;
        }
    }

    private void UpdateUI()
    {
        for (var i = 0; i < 4; i++)
        {
            if (i < activeDevices.Count)
            {
                if (activeDevices[i] is Keyboard)
                {
                    playerImages[i].color = new Color(0.8f,0.8f,0.8f,1.0f);
                }
                else
                {
                    playerImages[i].color = Color.white;
                }
                
            }
            else
            {
                playerImages[i].color = Color.grey;
            }
        }
    }

    private void OnDestroy()
    {
        gameConfig.devices = activeDevices;
    }
}