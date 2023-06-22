using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Checkpoint : MonoBehaviour
{
    private GameLoop loop;
    public GameLoop Loop
    {
        get => loop;
        set => loop = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.TryGetComponent<PhysicCharacter>(out PhysicCharacter character);
        if (character.isIAControlled && character.Components.aiModule.Checkpoint.IndexOf(this) == character.Components.aiModule.CurrentCheckpoint)
        {
            character.Components.aiModule.AddReward(1f);
            Debug.Log("Checkpoint passed : " + character.Components.aiModule.CurrentCheckpoint);
            character.Components.aiModule.CurrentCheckpoint++;
           
        }
        else
        {
            character.Components.aiModule.AddReward(-0.5f);
            Debug.Log("Wrong checkpoint passed, subtracting reward as consequence");
        }
        /*int checkpointIndex = Loop.Checkpoints.IndexOf(gameObject.GetComponent<Checkpoint>());
        var info = Loop.PlayerInfo[other.gameObject];
        info.lastCheckpoint = info.currentCheckpoint;
        info.currentCheckpoint = checkpointIndex;
        if ((checkpointIndex == 0 && info.lastCheckpoint > Loop.Checkpoints.Count * 0.7f) )
        {
            info.turnCount++;
        }
        Debug.Log("Info : " + checkpointIndex + ":" + info.lastCheckpoint + ":" + info.turnCount);
        Loop.PlayerInfo[other.gameObject] = info;*/
    }
}
