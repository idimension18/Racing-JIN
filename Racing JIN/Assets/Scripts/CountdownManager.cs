using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Gère le compte à rebours au départ de la course
/// </summary>
public class CountdownManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private CanvasGroup countdownCanvasGroup;
    
    [Header("Countdown Settings")]
    [SerializeField] private int countdownTime = 3;
    [SerializeField] private float numberDisplayDuration = 1f;
    [SerializeField] private string goText = "GO!";
    
    [Header("Visual Effects")]
    [SerializeField] private Color countdownColor = Color.red;
    [SerializeField] private Color goColor = Color.green;
    [SerializeField] private float scaleAnimationMax = 1.5f;
    [SerializeField] private float fadeSpeed = 2f;
    
    [Header("References")]
    [SerializeField] private RaceManager raceManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MyCarPhysics carPhysics;
    
    private bool countdownComplete = false;
    private Rigidbody carRigidbody;
    private RigidbodyConstraints originalConstraints;
    
    void Start()
    {
        if (raceManager == null)
        {
            raceManager = FindFirstObjectByType<RaceManager>();
        }
        
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
        
        if (carPhysics == null)
        {
            carPhysics = FindFirstObjectByType<MyCarPhysics>();
        }
        
        // Récupérer le Rigidbody pour bloquer complètement la voiture
        if (carPhysics != null)
        {
            carRigidbody = carPhysics.GetComponent<Rigidbody>();
            if (carRigidbody != null)
            {
                // Sauvegarder les contraintes d'origine
                originalConstraints = carRigidbody.constraints;
            }
        }
        
        // Bloquer complètement la voiture pendant le compte à rebours
        BlockCar();
        
        // Démarrer le compte à rebours
        StartCoroutine(CountdownRoutine());
    }
    
    /// <summary>
    /// Bloque complètement la voiture
    /// </summary>
    private void BlockCar()
    {
        // Désactiver le contrôle du joueur
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Désactiver la physique de la voiture
        if (carPhysics != null)
        {
            carPhysics.enabled = false;
        }
        
        // Geler le Rigidbody pour empêcher tout mouvement
        if (carRigidbody != null)
        {
            carRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    
    /// <summary>
    /// Débloque complètement la voiture
    /// </summary>
    private void UnblockCar()
    {
        // Activer le contrôle du joueur
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        
        // Activer la physique de la voiture
        if (carPhysics != null)
        {
            carPhysics.enabled = true;
        }
        
        // Restaurer les contraintes d'origine du Rigidbody
        if (carRigidbody != null)
        {
            carRigidbody.constraints = originalConstraints;
        }
    }
    
    /// <summary>
    /// Coroutine du compte à rebours
    /// </summary>
    private IEnumerator CountdownRoutine()
    {
        // Attendre un court instant avant de commencer
        yield return new WaitForSeconds(0.5f);
        
        // Compte à rebours : 3... 2... 1...
        for (int i = countdownTime; i > 0; i--)
        {
            ShowNumber(i, countdownColor);
            yield return new WaitForSeconds(numberDisplayDuration);
        }
        
        // Afficher "GO!" ET démarrer la course en même temps
        ShowNumber(0, goColor);
        
        // DÉMARRER LA COURSE IMMÉDIATEMENT
        if (raceManager != null)
        {
            raceManager.StartRace();
        }
        
        // DÉBLOQUER LA VOITURE IMMÉDIATEMENT
        UnblockCar();
        
        countdownComplete = true;
        
        // Attendre avant de faire disparaître le "GO!"
        yield return new WaitForSeconds(numberDisplayDuration);
        
        // Cacher le compte à rebours
        StartCoroutine(FadeOut());
    }
    
    /// <summary>
    /// Affiche un nombre avec animation
    /// </summary>
    private void ShowNumber(int number, Color color)
    {
        if (countdownText == null) return;
        
        // Définir le texte
        countdownText.text = number > 0 ? number.ToString() : goText;
        countdownText.color = color;
        
        // Réinitialiser l'opacité
        if (countdownCanvasGroup != null)
        {
            countdownCanvasGroup.alpha = 1f;
        }
        
        // Animation de scale
        StartCoroutine(ScaleAnimation());
    }
    
    /// <summary>
    /// Animation d'agrandissement du texte
    /// </summary>
    private IEnumerator ScaleAnimation()
    {
        if (countdownText == null) yield break;
        
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * scaleAnimationMax;
        Vector3 endScale = Vector3.one;
        
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 0.3f;
            
            countdownText.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            
            yield return null;
        }
        
        countdownText.transform.localScale = endScale;
    }
    
    /// <summary>
    /// Fade out du texte
    /// </summary>
    private IEnumerator FadeOut()
    {
        if (countdownCanvasGroup == null) yield break;
        
        float elapsed = 0f;
        float duration = 1f / fadeSpeed;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            countdownCanvasGroup.alpha = 1f - (elapsed / duration);
            
            yield return null;
        }
        
        countdownCanvasGroup.alpha = 0f;
        
        // Désactiver le GameObject après le fade
        gameObject.SetActive(false);
    }
    
    public bool IsCountdownComplete() => countdownComplete;
}
