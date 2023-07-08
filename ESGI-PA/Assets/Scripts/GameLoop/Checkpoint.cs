using UnityEngine;
using Debug = UnityEngine.Debug;

public class Checkpoint : MonoBehaviour
{
    public GameLoop Loop { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        ManagePlayer(other);
    }

    private void ManagePlayer(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;
        var currentIndex = Loop.Checkpoints.IndexOf(this);
        var lastIndex = Loop.Checkpoints.IndexOf(player.CurrentCheckpoint);
        var checkpointCount = Loop.Checkpoints.Count;
        if (currentIndex >= 0 && currentIndex < checkpointCount * 0.1f && lastIndex > checkpointCount * 0.8)
        {
            player.TurnCount++;
            Debug.Log("Incrementing Turn count");
        }
        player.LastCheckpoint = player.CurrentCheckpoint;
        player.CurrentCheckpoint = this;
        Debug.Log($"Current index : {currentIndex} / Last index : {lastIndex} / Checkpoint count : {checkpointCount}");
    }
}


