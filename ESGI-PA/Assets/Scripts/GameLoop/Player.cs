using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Checkpoint> _checkpoints;

    public List<Checkpoint> Checkpoints
    {
        get => _checkpoints;
        set => _checkpoints = value;
    }

    [SerializeField] private int currentCheckpoint = 0;

    [SerializeField]private int currentTurn;
    public int CurrentTurn => currentTurn;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggering");
        if (other.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
        {
            var checkpointIndex = _checkpoints.IndexOf(checkpoint);
            if (checkpointIndex < _checkpoints.Count / 2 && currentCheckpoint > _checkpoints.Count / 2)
            {
                currentTurn++;
            }
            currentCheckpoint = checkpointIndex;
        }
    }
}
