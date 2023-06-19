using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private RaceConfiguration raceConfig;
    [SerializeField] private List<Player> players;
    [SerializeField] private List<Checkpoint> checkpoints;
    private GameConfiguration gameConfig;
    private int _startCounter = 3;

    public GameConfiguration GameConfig
    {
        get => gameConfig;
        set => gameConfig = value;
    }

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if ((CheckEndOfGame() || Input.GetKeyDown(KeyCode.P)) && _startCounter <= 0)
        {
            EndOfGame();
        }
    }

    public void AddPlayer(Player player)
    {
        player.Checkpoints = checkpoints;
        players.Add(player);
    }


    private bool CheckEndOfGame()
    {
        return players.All(player => player.CurrentTurn >= raceConfig.turnCount);
    }

    private void EndOfGame()
    {
        var idToLoad = gameConfig.nextCircuit;
        gameConfig.nextCircuit++;
        if (gameConfig.nextCircuit > gameConfig.circuits.Count)
        {
            gameConfig.nextCircuit = 0;
            SceneManager.LoadScene("Menuprincipal");
        }
        else
        {
            SceneManager.LoadScene(gameConfig.circuits[idToLoad]);
        }

    }

    private IEnumerator StartCountdown()
    {
        for (var i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1);
            _startCounter --;
            Debug.Log("Counter : " + _startCounter);
        }
    }
}
