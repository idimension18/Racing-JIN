using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Écran de fin affichant les résultats de la course
/// </summary>
public class FinishScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI totalTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    [SerializeField] private TextMeshProUGUI congratsText;
    
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string menuSceneName = "Menu";
    
    [Header("Navigation Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = new Color(1f, 0.85f, 0f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color selectedColor = new Color(1f, 0.9f, 0.5f, 1f);
    
    private GameObject _lastSelected;
    
    private void Start()
    {
        // Configurer les couleurs des boutons
        ConfigureButtonColors(restartButton);
        ConfigureButtonColors(menuButton);
        
        // Configurer les boutons
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartRace);
        }
        
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ReturnToMenu);
        }
    }
    
    private void Update()
    {
        // Vérifier si l'écran est actif
        if (!gameObject.activeInHierarchy)
            return;
        
        // Vérifier si aucun bouton n'est sélectionné
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Réactiver la sélection sur le dernier bouton sélectionné
            if (_lastSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected);
            }
            else if (restartButton != null)
            {
                EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
                _lastSelected = restartButton.gameObject;
            }
        }
        else
        {
            // Mettre à jour le dernier bouton sélectionné
            _lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }
    
    /// <summary>
    /// Configure les couleurs d'un bouton pour la navigation à la manette
    /// </summary>
    private void ConfigureButtonColors(Button button)
    {
        if (button == null) return;
        
        ColorBlock colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = highlightedColor;
        colors.pressedColor = pressedColor;
        colors.selectedColor = selectedColor;
        colors.colorMultiplier = 1f;
        colors.fadeDuration = 0.1f;
        
        button.colors = colors;
        
        // S'assurer que le mode de transition est sur Colors
        button.transition = Selectable.Transition.ColorTint;
    }
    
    /// <summary>
    /// Affiche les résultats de la course
    /// </summary>
    public void ShowResults(float totalTime, float bestLapTime)
    {
        if (totalTimeText != null)
        {
            totalTimeText.text = $"Temps total:\n{RaceManager.FormatTime(totalTime)}";
        }
        
        if (bestLapTimeText != null && bestLapTime < float.MaxValue)
        {
            bestLapTimeText.text = $"Meilleur tour:\n{RaceManager.FormatTime(bestLapTime)}";
        }
        
        if (congratsText != null)
        {
            congratsText.text = "Course terminée !";
        }
        
        // Déverrouiller le curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Sélectionner le bouton "Recommencer" par défaut pour la manette
        if (restartButton != null)
        {
            EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
            _lastSelected = restartButton.gameObject;
        }
    }
    
    /// <summary>
    /// Recommence la course
    /// </summary>
    public void RestartRace()
    {
        Debug.Log("Restarting race...");
        
        // Verrouiller le curseur pour le gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    /// <summary>
    /// Retourne au menu principal
    /// </summary>
    public void ReturnToMenu()
    {
        Debug.Log("Returning to menu...");
        SceneManager.LoadScene(menuSceneName);
    }
}
