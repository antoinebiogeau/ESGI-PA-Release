using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AICharacter : Agent
{
    [SerializeField] private Transform characterPosition;

    
    [SerializeField] private Vector2 axisView;

    public List<Checkpoint> Checkpoint{ get; set; } = new();
    public int CurrentCheckpoint { get; set; } = 0;
    
    public Vector2 Axis { get; private set; } = Vector2.zero;
    public bool IsJumping { get; set; } = false;
    public bool IsDashing { get; set; } = false;

    public bool IsRunning { get; set; } = false;
    
    public override void CollectObservations(VectorSensor sensor)
    {
        var diff = Checkpoint[CurrentCheckpoint].transform.position - characterPosition.position;
        sensor.AddObservation(diff);
    }
    public override void OnEpisodeBegin()
    {
        characterPosition.localPosition = new Vector3(-210.086731f,0.976806641f,166.651978f);
        Debug.Log("Episode has started");
        CurrentCheckpoint = 0;
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        float x = actions.DiscreteActions[0] switch
        {
            0 => 0,
            1 => -1,
            2 => 1,
            _ => 0
        };
        float y = actions.DiscreteActions[1] switch
        {
            0 => 0,
            1 => -1,
            2 => 1,
            _ => 0
        };
        IsJumping = actions.DiscreteActions[2] switch
        {
            0 => false,
            1 => true,
            _ => false
        };
        IsDashing = actions.DiscreteActions[3] switch
        {
            0 => false,
            1 => true,
            _ => false
        };
        IsRunning = actions.DiscreteActions[4] switch
        {
            0 => false,
            1 => true,
            _ => false
        };
        Axis = new Vector2(x, y);
        axisView = Axis;
        AddReward(-0.01f);
    }
    
}
