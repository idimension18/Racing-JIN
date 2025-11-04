using UnityEngine;
using TMPro;

/// <summary>
/// Gère l'interface utilisateur pendant la course
/// </summary>
public class RaceUI : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI lapCountText;
    [SerializeField] private TextMeshProUGUI raceTimeText;
    [SerializeField] private TextMeshProUGUI lastLapTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    
    [Header("Display Settings")]
    [SerializeField] private bool showLastLapTime = true;
    [SerializeField] private bool showBestLapTime = true;
    
    private float lastLapTimeDisplayDuration = 3f;
    private float lastLapTimeDisplayTimer = 0f;
    
    void Start()
    {
        // Cacher le temps du dernier tour au début
        if (lastLapTimeText != null)
        {
            lastLapTimeText.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        // Gérer l'affichage temporaire du temps du dernier tour
        if (lastLapTimeDisplayTimer > 0)
        {
            lastLapTimeDisplayTimer -= Time.deltaTime;
            
            if (lastLapTimeDisplayTimer <= 0 && lastLapTimeText != null)
            {
                lastLapTimeText.gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Met à jour l'affichage du compteur de tours
    /// </summary>
    public void UpdateLapCount(int currentLap, int totalLaps)
    {
        if (lapCountText != null)
        {
            lapCountText.text = $"Tour: {currentLap}/{totalLaps}";
        }
    }
    
    /// <summary>
    /// Met à jour l'affichage du temps de course
    /// </summary>
    public void UpdateRaceTime(float timeInSeconds)
    {
        if (raceTimeText != null)
        {
            raceTimeText.text = RaceManager.FormatTime(timeInSeconds);
        }
    }
    
    /// <summary>
    /// Met à jour l'affichage du temps du dernier tour
    /// </summary>
    public void UpdateLastLapTime(float timeInSeconds)
    {
        if (lastLapTimeText != null && showLastLapTime)
        {
            lastLapTimeText.text = $"Dernier tour: {RaceManager.FormatTime(timeInSeconds)}";
            lastLapTimeText.gameObject.SetActive(true);
            lastLapTimeDisplayTimer = lastLapTimeDisplayDuration;
        }
    }
    
    /// <summary>
    /// Met à jour l'affichage du meilleur temps de tour
    /// </summary>
    public void UpdateBestLapTime(float timeInSeconds)
    {
        if (bestLapTimeText != null && showBestLapTime)
        {
            if (timeInSeconds < float.MaxValue)
            {
                bestLapTimeText.text = $"Meilleur: {RaceManager.FormatTime(timeInSeconds)}";
                bestLapTimeText.gameObject.SetActive(true);
            }
        }
    }
}
