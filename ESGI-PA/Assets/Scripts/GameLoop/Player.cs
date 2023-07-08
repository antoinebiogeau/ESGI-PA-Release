using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Checkpoint CurrentCheckpoint { get; set; } = null;

    public Checkpoint LastCheckpoint { get; set; } = null;

    [SerializeField] public int TurnCount = 0;

    public float RankingScore { get; set; }

    public int Rank { get; set; } = 0;
    
    public bool IsTurningBack { get; set; } = false;

    private float _lastDistanceFromCurrent = 100000;

    private void Update()
    {
        if (!CurrentCheckpoint) return;
        var distanceFromCurrent = Vector3.Distance(transform.position, CurrentCheckpoint.transform.position);
        IsTurningBack = distanceFromCurrent < _lastDistanceFromCurrent;
    }

    
}
