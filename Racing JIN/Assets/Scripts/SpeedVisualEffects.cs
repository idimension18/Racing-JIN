using UnityEngine;

/// <summary>
/// Gère les effets visuels liés à la vitesse de la voiture (trails des roues et boost)
/// Version simplifiée : Trails + Boost uniquement
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpeedVisualEffects : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float speedThreshold = 5f; // Vitesse minimale pour déclencher les trails
    [SerializeField] private float maxSpeedForEffects = 30f; // Vitesse max pour l'intensité des trails
    
    [Header("Trail Effects (Roues)")]
    [SerializeField] private TrailRenderer[] wheelTrails; // Trails pour les roues
    [SerializeField] private float minTrailTime = 0.1f;
    [SerializeField] private float maxTrailTime = 0.5f;
    [SerializeField] private AnimationCurve trailCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Boost Effects")]
    [SerializeField] private ParticleSystem boostParticles; // Particules spéciales pour le boost
    [SerializeField] private Light boostLight; // Lumière pour le boost (optionnel)
    [SerializeField] private float boostLightIntensity = 2f;
    
    [Header("Animation")]
    [SerializeField] private float effectSmoothSpeed = 5f;
    
    private Rigidbody _rb;
    private float _currentSpeed;
    private float _speedNormalized; // Entre 0 et 1
    private bool _isBoostActive = false;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        // Désactiver les particules de boost au départ
        if (boostParticles != null)
        {
            boostParticles.Stop();
        }
        
        if (boostLight != null)
        {
            boostLight.enabled = false;
        }
    }
    
    private void Update()
    {
        // Calculer la vitesse actuelle
        CalculateSpeed();
        
        // Mettre à jour uniquement les trails
        UpdateTrailEffects();
    }
    
    /// <summary>
    /// Calcule la vitesse actuelle et la normalise
    /// </summary>
    private void CalculateSpeed()
    {
        // Vitesse horizontale (on ignore Y pour une voiture au sol)
        Vector3 horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _currentSpeed = horizontalVelocity.magnitude;
        
        // Normaliser la vitesse entre 0 et 1
        _speedNormalized = Mathf.Clamp01((_currentSpeed - speedThreshold) / (maxSpeedForEffects - speedThreshold));
    }
    
    /// <summary>
    /// Met à jour les trails des roues en fonction de la vitesse
    /// </summary>
    private void UpdateTrailEffects()
    {
        if (wheelTrails == null || wheelTrails.Length == 0) return;
        
        foreach (TrailRenderer trail in wheelTrails)
        {
            if (trail == null) continue;
            
            // Activer/désactiver le trail selon la vitesse
            trail.emitting = _currentSpeed > speedThreshold;
            
            // Ajuster la durée du trail en fonction de la vitesse
            float targetTime = Mathf.Lerp(minTrailTime, maxTrailTime, trailCurve.Evaluate(_speedNormalized));
            trail.time = Mathf.Lerp(trail.time, targetTime, Time.deltaTime * effectSmoothSpeed);
        }
    }
    
    /// <summary>
    /// Active les effets visuels du boost
    /// </summary>
    public void TriggerBoostEffect()
    {
        _isBoostActive = true;
        
        if (boostParticles != null)
        {
            boostParticles.Play();
        }
        
        if (boostLight != null)
        {
            boostLight.enabled = true;
            boostLight.intensity = boostLightIntensity;
        }
    }
    
    /// <summary>
    /// Désactive les effets visuels du boost
    /// </summary>
    public void StopBoostEffect()
    {
        _isBoostActive = false;
        
        if (boostParticles != null)
        {
            boostParticles.Stop();
        }
        
        if (boostLight != null)
        {
            boostLight.enabled = false;
        }
    }
    
    /// <summary>
    /// Retourne la vitesse actuelle normalisée (0-1)
    /// </summary>
    public float GetSpeedNormalized()
    {
        return _speedNormalized;
    }
    
    /// <summary>
    /// Retourne la vitesse actuelle en unités/seconde
    /// </summary>
    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }
}
