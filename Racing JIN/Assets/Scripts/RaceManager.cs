using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère l'état de la course, les tours et le temps
/// </summary>
public class RaceManager : MonoBehaviour
{
    [Header("Race Settings")]
    [SerializeField] private int totalLaps = 3;
    [SerializeField] private CheckpointManager checkpointManager;
    [SerializeField] private bool useCountdown = true;
    
    [Header("UI References")]
    [SerializeField] private RaceUI raceUI;
    [SerializeField] private GameObject finishScreenUI;
    
    private Dictionary<GameObject, int> playerLaps = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, float> playerTimes = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, float> playerBestLapTimes = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, float> playerCurrentLapStartTime = new Dictionary<GameObject, float>();
    
    private bool raceStarted = false;
    private bool raceFinished = false;
    private float raceStartTime;
    
    void Start()
    {
        if (checkpointManager == null)
        {
            checkpointManager = FindFirstObjectByType<CheckpointManager>();
        }
        
        if (raceUI == null)
        {
            raceUI = FindFirstObjectByType<RaceUI>();
        }
        
        if (finishScreenUI != null)
        {
            finishScreenUI.SetActive(false);
        }
        
        // Ne démarrer la course automatiquement que si pas de countdown
        if (!useCountdown)
        {
            Invoke(nameof(StartRace), 1f);
        }
    }
    
    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            // Mettre à jour le temps de course
            float currentTime = Time.time - raceStartTime;
            
            if (raceUI != null)
            {
                raceUI.UpdateRaceTime(currentTime);
            }
        }
    }
    
    /// <summary>
    /// Démarre la course
    /// </summary>
    public void StartRace()
    {
        raceStarted = true;
        raceStartTime = Time.time;
        
        // Trouver tous les joueurs
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            RegisterPlayer(player.gameObject);
        }
        
        Debug.Log("Race started!");
    }
    
    /// <summary>
    /// Enregistre un joueur dans la course
    /// </summary>
    public void RegisterPlayer(GameObject player)
    {
        if (!playerLaps.ContainsKey(player))
        {
            playerLaps[player] = 1; // Commence au tour 1
            playerTimes[player] = 0f;
            playerBestLapTimes[player] = float.MaxValue;
            playerCurrentLapStartTime[player] = Time.time;
            
            checkpointManager.RegisterPlayer(player);
            
            if (raceUI != null)
            {
                // Afficher "Tour: 1/3" au départ
                raceUI.UpdateLapCount(1, totalLaps);
            }
        }
    }
    
    /// <summary>
    /// Complète un tour pour un joueur
    /// </summary>
    public void CompleteLap(GameObject player)
    {
        if (!raceStarted || raceFinished)
            return;
        
        if (!playerLaps.ContainsKey(player))
        {
            RegisterPlayer(player);
            return;
        }
        
        int currentLap = playerLaps[player];
        
        // Calculer le temps du tour
        float lapTime = Time.time - playerCurrentLapStartTime[player];
        playerCurrentLapStartTime[player] = Time.time;
        
        // Mettre à jour le meilleur temps de tour
        if (lapTime < playerBestLapTimes[player])
        {
            playerBestLapTimes[player] = lapTime;
        }
        
        Debug.Log($"Lap {currentLap}/{totalLaps} completed! Time: {lapTime:F2}s");
        
        // Mettre à jour l'UI avec le temps du tour
        if (raceUI != null)
        {
            raceUI.UpdateLastLapTime(lapTime);
            raceUI.UpdateBestLapTime(playerBestLapTimes[player]);
        }
        
        // Vérifier si la course est terminée
        if (currentLap >= totalLaps)
        {
            FinishRace(player);
        }
        else
        {
            // Passer au tour suivant
            playerLaps[player]++;
            
            // Mettre à jour l'affichage du tour
            if (raceUI != null)
            {
                raceUI.UpdateLapCount(playerLaps[player], totalLaps);
            }
        }
    }
    
    /// <summary>
    /// Termine la course
    /// </summary>
    private void FinishRace(GameObject player)
    {
        raceFinished = true;
        float totalTime = Time.time - raceStartTime;
        playerTimes[player] = totalTime;
        
        Debug.Log($"Race finished! Total time: {FormatTime(totalTime)}");
        
        // Afficher l'écran de fin
        if (finishScreenUI != null)
        {
            finishScreenUI.SetActive(true);
            
            FinishScreen finishScreen = finishScreenUI.GetComponent<FinishScreen>();
            if (finishScreen != null)
            {
                finishScreen.ShowResults(totalTime, playerBestLapTimes[player]);
            }
        }
        
        // Désactiver le contrôle du joueur
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }
    
    /// <summary>
    /// Formate le temps en minutes:secondes.millisecondes
    /// </summary>
    public static string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);
        
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
    
    public bool IsRaceStarted() => raceStarted;
    public bool IsRaceFinished() => raceFinished;
    public int GetCurrentLap(GameObject player) => playerLaps.ContainsKey(player) ? playerLaps[player] : 1;
    public int GetTotalLaps() => totalLaps;
}
