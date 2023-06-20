
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class InputManagerEvents : MonoBehaviour
{
    private const int MAX_DEVICES = 4;

    [SerializeField] private GameConfiguration gameConfig;

    [SerializeField] private List<RawImage> playerImages;
    
    [SerializeField] private bool checkForInputs = false;

    private void Awake()
    {
        _keyboard = Keyboard.current;
    }

    public bool CheckForInputs
    {
        get => checkForInputs;
        set => checkForInputs = value;
    }

    private bool _isKeyboardActive;

    private List<InputDevice> _activeDevices = new();
    private Keyboard _keyboard;

    private void OnEnable()
    {
        checkForInputs = true;
        Debug.Log("Enabling");
    }

    private void OnDisable()
    {
        checkForInputs = false;
        Debug.Log("Disable");
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
        if (_keyboard.anyKey.isPressed && !_keyboard.backspaceKey.isPressed && !_isKeyboardActive && _activeDevices.Count < MAX_DEVICES)
        {
            _isKeyboardActive = true;
            _activeDevices.Add(_keyboard);
        }
        else if (_keyboard.backspaceKey.isPressed && _isKeyboardActive)
        {
            _isKeyboardActive = false;
            _activeDevices.Remove(_keyboard);
        }
    }

    private void DetectGamepads()
    {
        //On regarde tous les gamepads dispo, si parmi l'un de ces gamepads, l'une des touches est jouée, on active le gamepad
        var gamepads = Gamepad.all.ToArray();
        foreach (var gamepad in gamepads)
        {
            if (gamepad.buttonSouth.isPressed && !_activeDevices.Contains(gamepad) && _activeDevices.Count < MAX_DEVICES)
            {
                _activeDevices.Add(gamepad);
            }
        }
        foreach (var activeDevice in _activeDevices)
        {
            if (activeDevice is not Gamepad) continue;
            var device = (Gamepad)activeDevice;
            if (Gamepad.all.ToArray().Contains(device) && !device.buttonEast.isPressed) continue;
            _activeDevices.Remove(device);
            break;
        }
    }

    private void UpdateUI()
    {
        for (var i = 0; i < 4; i++)
        {
            if (i < _activeDevices.Count)
            {
                if (_activeDevices[i] is Keyboard)
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
        gameConfig.devices = _activeDevices;
    }
}