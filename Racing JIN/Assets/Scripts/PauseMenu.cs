using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Menu pause accessible pendant la course avec Échap (clavier) ou Start (manette)
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;
    
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string menuSceneName = "Menu";
    
    [Header("Navigation Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = new Color(1f, 0.85f, 0f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color selectedColor = new Color(1f, 0.9f, 0.5f, 1f);
    
    private bool _isPaused = false;
    private GameObject _lastSelected;
    
    private void Start()
    {
        // Configurer les couleurs des boutons
        ConfigureButtonColors(resumeButton);
        ConfigureButtonColors(menuButton);
        
        // Configurer les boutons
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ReturnToMenu);
        }
        
        // S'assurer que le menu est caché au départ
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Vérifier les inputs pour mettre en pause
        // Échap pour le clavier
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
        
        // Start pour la manette (bouton Start sur la plupart des manettes)
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePause();
        }
        
        // Gérer la navigation des boutons si le menu est actif
        if (_isPaused)
        {
            // Vérifier si aucun bouton n'est sélectionné
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                // Réactiver la sélection sur le dernier bouton sélectionné
                if (_lastSelected != null)
                {
                    EventSystem.current.SetSelectedGameObject(_lastSelected);
                }
                else if (resumeButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
                    _lastSelected = resumeButton.gameObject;
                }
            }
            else
            {
                // Mettre à jour le dernier bouton sélectionné
                _lastSelected = EventSystem.current.currentSelectedGameObject;
            }
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
    /// Bascule entre pause et jeu
    /// </summary>
    public void TogglePause()
    {
        if (_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    /// <summary>
    /// Met le jeu en pause
    /// </summary>
    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        
        // Arrêter le temps du jeu
        Time.timeScale = 0f;
        _isPaused = true;
        
        // Déverrouiller et afficher le curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Sélectionner le bouton "Reprendre" par défaut pour la manette
        if (resumeButton != null)
        {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            _lastSelected = resumeButton.gameObject;
        }
        
        Debug.Log("Game paused");
    }
    
    /// <summary>
    /// Reprend le jeu
    /// </summary>
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        
        // Reprendre le temps du jeu
        Time.timeScale = 1f;
        _isPaused = false;
        
        // Verrouiller et cacher le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Debug.Log("Game resumed");
    }
    
    /// <summary>
    /// Retourne au menu principal
    /// </summary>
    public void ReturnToMenu()
    {
        Debug.Log("Returning to menu from pause...");
        
        // Remettre le temps à la normale avant de changer de scène
        Time.timeScale = 1f;
        _isPaused = false;
        
        SceneManager.LoadScene(menuSceneName);
    }
    
    /// <summary>
    /// Vérifie si le jeu est en pause
    /// </summary>
    public bool IsPaused()
    {
        return _isPaused;
    }
}
