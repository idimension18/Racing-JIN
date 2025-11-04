using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère les checkpoints du circuit pour éviter la triche
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private List<Transform> checkpoints = new List<Transform>();
    [SerializeField] private Transform finishLine;
    [SerializeField] private bool debugMode = false;
    
    private Dictionary<GameObject, int> playerCheckpointProgress = new Dictionary<GameObject, int>();
    
    void Start()
    {
        if (checkpoints.Count == 0)
        {
            Debug.LogWarning("No checkpoints assigned! Add checkpoint transforms to the list.");
        }
        
        if (finishLine == null)
        {
            Debug.LogError("Finish line not assigned!");
        }
    }
    
    /// <summary>
    /// Enregistre le passage d'un checkpoint par un joueur
    /// </summary>
    public void RegisterCheckpoint(GameObject player, int checkpointIndex)
    {
        if (!playerCheckpointProgress.ContainsKey(player))
        {
            playerCheckpointProgress[player] = -1;
        }
        
        // Vérifier que c'est le prochain checkpoint attendu
        if (checkpointIndex == playerCheckpointProgress[player] + 1)
        {
            playerCheckpointProgress[player] = checkpointIndex;
            
            if (debugMode)
            {
                Debug.Log($"Player passed checkpoint {checkpointIndex}/{checkpoints.Count}");
            }
        }
    }
    
    /// <summary>
    /// Vérifie si le joueur peut valider un tour (tous les checkpoints passés)
    /// </summary>
    public bool CanValidateLap(GameObject player)
    {
        if (!playerCheckpointProgress.ContainsKey(player))
            return false;
        
        // Le joueur doit avoir passé tous les checkpoints
        return playerCheckpointProgress[player] >= checkpoints.Count - 1;
    }
    
    /// <summary>
    /// Réinitialise les checkpoints pour un nouveau tour
    /// </summary>
    public void ResetCheckpoints(GameObject player)
    {
        if (playerCheckpointProgress.ContainsKey(player))
        {
            playerCheckpointProgress[player] = -1;
        }
    }
    
    /// <summary>
    /// Initialise le joueur dans le système de checkpoints
    /// </summary>
    public void RegisterPlayer(GameObject player)
    {
        if (!playerCheckpointProgress.ContainsKey(player))
        {
            playerCheckpointProgress[player] = -1;
        }
    }
    
    public int GetCheckpointCount()
    {
        return checkpoints.Count;
    }
}
