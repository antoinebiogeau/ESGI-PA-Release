using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject checkpointsSource;
    [SerializeField] private List<Checkpoint> _checkpoints;

    private void Start()
    {
        for (var i = 0; i < checkpointsSource.transform.childCount; i++)
        {
            var child = checkpointsSource.transform.GetChild(i);
            _checkpoints.Add(child.GetComponent<Checkpoint>());
            AI.CheckpointPositions.Add(child.position);
        }
    }

    public List<Checkpoint> Checkpoints
    {
        get => _checkpoints;
        set => _checkpoints = value;
    }

    [SerializeField] private int currentCheckpoint = 0;
    [SerializeField] private int lastCheckpoint;

    public Checkpoint LastCheckpoint => _checkpoints[lastCheckpoint];

    [SerializeField]private int currentTurn;
    public int CurrentTurn => currentTurn;

    [SerializeField] private AICharacter AI;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggering");
        if (!other.TryGetComponent<Checkpoint>(out var checkpoint)) return;
        var checkpointIndex = _checkpoints.IndexOf(checkpoint);
        if (checkpointIndex < _checkpoints.Count / 2 && currentCheckpoint > _checkpoints.Count / 2)
        {
            currentTurn++;
        }

        lastCheckpoint = currentCheckpoint;
        Debug.Log($"Last checkpoint : {lastCheckpoint}");
        currentCheckpoint = checkpointIndex;
    }
}
