using UnityEngine;

/// <summary>
/// Représente un checkpoint individuel sur le circuit
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private int checkpointIndex;
    [SerializeField] private CheckpointManager checkpointManager;
    
    void Start()
    {
        // S'assurer que le trigger est activé
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        
        if (checkpointManager == null)
        {
            checkpointManager = FindFirstObjectByType<CheckpointManager>();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si c'est le joueur
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            GameObject player = other.GetComponentInParent<PlayerController>().gameObject;
            checkpointManager.RegisterCheckpoint(player, checkpointIndex);
        }
    }
    
    private void OnDrawGizmos()
    {
        // Visualiser les checkpoints dans l'éditeur
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
