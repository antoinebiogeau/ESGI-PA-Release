using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private Transform checkpointBatch;
    [SerializeField] private RaceConfiguration raceConfig;
    public List<Player> Players { get; set; } = new();
    
    private GameConfiguration gameConfig;
    private int _startCounter = 3;

    public GameConfiguration GameConfig
    {
        get => gameConfig;
        set => gameConfig = value;
    }

    public List<Checkpoint> Checkpoints { get; set; } = new();

    private void Start()
    {
        RetrieveCheckpoints();
    }

    private void Update()
    {
        if (CheckEndOfGame())
        {
            Debug.Log("Players before end : " + Players.Count);
            Debug.Log("The end");
            EndOfGame();
        }
    }

    private void RetrieveCheckpoints()
    {
        for (var i = 0; i < checkpointBatch.childCount; i++)
        {
            Checkpoints.Add(checkpointBatch.GetChild(i).GetComponent<Checkpoint>());
            Checkpoints[i].Loop = this;
        }
    }
    


    private bool CheckEndOfGame()
    {
        return Players.All(player => player.TurnCount >= raceConfig.turnCount);
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
