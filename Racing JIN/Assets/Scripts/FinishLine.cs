using UnityEngine;

/// <summary>
/// Ligne d'arrivée qui déclenche la validation des tours
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class FinishLine : MonoBehaviour
{
    [Header("Finish Line Settings")]
    [SerializeField] private CheckpointManager checkpointManager;
    [SerializeField] private RaceManager raceManager;
    
    private void Start()
    {
        // S'assurer que le trigger est activé
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        
        if (checkpointManager == null)
        {
            checkpointManager = FindFirstObjectByType<CheckpointManager>();
        }
        
        if (raceManager == null)
        {
            raceManager = FindFirstObjectByType<RaceManager>();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si c'est le joueur
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            GameObject playerObject = player.gameObject;
            
            // Vérifier que tous les checkpoints ont été passés
            if (checkpointManager.CanValidateLap(playerObject))
            {
                raceManager.CompleteLap(playerObject);
                checkpointManager.ResetCheckpoints(playerObject);
            }
            else
            {
                Debug.Log("Cannot validate lap - not all checkpoints passed!");
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        // Visualiser la ligne d'arrivée dans l'éditeur
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
