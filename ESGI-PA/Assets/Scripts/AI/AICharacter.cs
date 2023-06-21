using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AICharacter : Agent
{
    [SerializeField] private Transform characterPosition;

    public Vector2 Axis { get; set; } = Vector2.zero;
    

    public List<Vector3> CheckpointPositions { get; set; } = new();

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(characterPosition.position);
        sensor.AddObservation(CheckpointPositions[0]);
    }
    public override void OnEpisodeBegin()
    {
        characterPosition.localPosition = new Vector3(-187.734482f,4.97680664f,170.19281f);
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
        /*x = actions.ContinuousActions[0];
        y = actions.ContinuousActions[1];*/
        Axis = new Vector2(x, y);
        if (Axis == Vector2.zero)
        {
            AddReward(-0.1f);
        }
        AddReward(-0.05f);
    }
    
}
