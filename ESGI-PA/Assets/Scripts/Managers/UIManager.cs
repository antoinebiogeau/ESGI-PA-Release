using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] uiList;
    [SerializeField] private GameConfiguration gameConfig;
    private int _playerCount = 0;

    private void Start()
    {
        SetUIConfig();
    }

    private void SetUIConfig()
    {

        switch (gameConfig.devices.Count)
        {
            case 1:
                uiList[0].SetActive(true);
                break;
            case 2:
                uiList[1].SetActive(true);
                break;
            case 3:
                uiList[2].SetActive(true);
                break;
            case 4:
                uiList[3].SetActive(true);
                break;
        }
    }

    public void LinkUIToPlayer(Player player)
    {
        uiList[gameConfig.devices.Count - 1].transform.GetChild(_playerCount).GetComponent<PlayerUI>().PlayerInfo = player;
        _playerCount++;
    }
}
