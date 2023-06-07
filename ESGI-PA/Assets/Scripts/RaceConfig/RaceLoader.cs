using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RaceLoader : MonoBehaviour
{
    [SerializeField] private GameConfiguration gameConfig;

    [SerializeField] private PlayerInputManager manager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private RaceConfiguration config;
    
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameLoop loop;

    private PlayerInput lastInput;
    
    private int playerJoined;

    public List<Vector3> spawningPosition = new();

    private Rect[] viewportDuo =
    {
        new Rect(0,0,1,0.5f),
        new Rect(0,0.5f,1,0.5f)
    };
    
    void Start()
    {
        spawningPosition = config.respawnPositions;
        InstantiatePlayers();
        
    }

    private void InstantiatePlayers()
    {
        playerJoined = 0;
        manager.playerPrefab = playerPrefab;
        if (gameConfig.devices.Count <= 1) manager.splitScreen = false;
        for (var i = 0; i < gameConfig.devices.Count; i++)
        {
            manager.JoinPlayer(i, i, gameConfig.devices[i] is Gamepad ? "gamepad" : "keyboard", gameConfig.devices[i]);
        }
        for (var i = 0; i < 8 - gameConfig.devices.Count; i++)
        {
            GameObject bot = Instantiate(playerPrefab);
            bot.TryGetComponent<Player>(out var playerInfo);
            playerInfo.Gameloop = loop;
            playerInfo.TurnCount = config.turnCount;
            bot.GetComponent<PlayerInput>().DeactivateInput();
            bot.GetComponent<PhysicCharacter>().camera.gameObject.SetActive(false);
        }
    }

    public void SetPlayer(PlayerInput player)
    {
        
        // uiManager.Players.Append(player.gameObject);
        // loop.PlayersRank.Append(player.gameObject);
        //loop.AddPlayer(player.gameObject);
        //uiManager.LinkToUI(player.gameObject);
        SetCameraLayout(player.GetComponent<PhysicCharacter>().camera.GetComponent<Camera>());
        player.TryGetComponent<Player>(out var playerInfo);
        playerInfo.Gameloop = loop;
        playerInfo.TurnCount = config.turnCount;
        

        player.gameObject.transform.position = spawningPosition[playerJoined];
        playerJoined++;
    }

    private void SetCameraLayout(Camera camera)
    {
        if (gameConfig.devices.Count == 1)
        {
            camera.rect = new Rect(0,0,1,1);
        }
        if (gameConfig.devices.Count == 2)
        {
            camera.rect = viewportDuo[playerJoined];
        }

        camera.transform.parent = null;
    }
}
