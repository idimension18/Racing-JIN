using UnityEngine;

/// <summary>
/// Script helper pour créer facilement les effets de trails et boost pour la voiture
/// Version simplifiée : Trails des roues + Boost uniquement
/// </summary>
public class ParticleEffectsSetup : MonoBehaviour
{
    [Header("Instructions")]
    [Tooltip("Utilisez le menu contextuel (clic droit) pour créer les effets")]
    [SerializeField] private string instructions = "Right-click on this component ? Create Wheel Trails / Create Boost Particles";
    
    [Header("Trail Settings")]
    [SerializeField] private Color trailColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    [SerializeField] private float trailWidth = 0.1f;
    
    [Header("Boost Particle Settings")]
    [SerializeField] private Color boostColor = new Color(1f, 0.8f, 0f, 0.7f);
    [SerializeField] private float boostParticleSize = 0.3f;
    
    /// <summary>
    /// Crée des TrailRenderers pour les roues
    /// </summary>
    [ContextMenu("Create Wheel Trails")]
    public void CreateWheelTrails()
    {
        // Positions typiques des roues pour une petite voiture
        Vector3[] wheelPositions = new Vector3[]
        {
            new Vector3(-0.3f, 0.05f, 0.4f),  // Avant gauche
            new Vector3(0.3f, 0.05f, 0.4f),   // Avant droite
            new Vector3(-0.3f, 0.05f, -0.4f), // Arrière gauche
            new Vector3(0.3f, 0.05f, -0.4f)   // Arrière droite
        };
        
        for (int i = 0; i < wheelPositions.Length; i++)
        {
            GameObject trailObj = new GameObject($"WheelTrail_{i}");
            trailObj.transform.SetParent(transform);
            trailObj.transform.localPosition = wheelPositions[i];
            trailObj.transform.localRotation = Quaternion.identity;
            
            TrailRenderer trail = trailObj.AddComponent<TrailRenderer>();
            ConfigureTrailRenderer(trail);
        }
        
        Debug.Log("? Wheel Trails créés ! Assignez-les maintenant dans SpeedVisualEffects.");
    }
    
    /// <summary>
    /// Crée un système de particules pour le boost
    /// </summary>
    [ContextMenu("Create Boost Particles")]
    public void CreateBoostParticles()
    {
        GameObject particleObj = new GameObject("BoostParticles");
        particleObj.transform.SetParent(transform);
        particleObj.transform.localPosition = new Vector3(0, 0.1f, -0.5f);
        particleObj.transform.localRotation = Quaternion.identity;
        
        ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();
        ConfigureBoostParticleSystem(ps);
        
        Debug.Log("? Boost Particles créés ! Assignez-les dans SpeedVisualEffects.");
    }
    
    /// <summary>
    /// Configure un TrailRenderer pour les roues
    /// </summary>
    private void ConfigureTrailRenderer(TrailRenderer trail)
    {
        trail.time = 0.3f;
        trail.startWidth = trailWidth;
        trail.endWidth = 0.01f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = trailColor;
        trail.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0f);
        trail.emitting = false;
        trail.autodestruct = false;
    }
    
    /// <summary>
    /// Configure le système de particules pour le boost
    /// </summary>
    private void ConfigureBoostParticleSystem(ParticleSystem ps)
    {
        var main = ps.main;
        main.startLifetime = 0.5f;
        main.startSpeed = 3f;
        main.startSize = boostParticleSize;
        main.startColor = boostColor;
        main.maxParticles = 100;
        main.loop = true;
        main.playOnAwake = false;
        
        // Emission
        var emission = ps.emission;
        emission.rateOverTime = 30f;
        
        // Shape - cône derrière la voiture
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 20f;
        shape.radius = 0.15f;
        shape.rotation = new Vector3(90f, 0f, 0f);
        
        // Color over lifetime - fade out
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(Color.white, 0f), 
                new GradientColorKey(Color.white, 1f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(0f, 1f) 
            }
        );
        colorOverLifetime.color = gradient;
        
        // Size over lifetime
        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 1f);
        curve.AddKey(1f, 0.2f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);
        
        // Renderer
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
    }
}
