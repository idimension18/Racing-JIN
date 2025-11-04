using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button buttonSetting;
    [SerializeField] private Button quitButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "SampleScene";

    [Header("Navigation Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = new Color(1f, 0.85f, 0f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color selectedColor = new Color(1f, 0.9f, 0.5f, 1f);

    private GameObject _lastSelected;

    void Start()
    {
        // Configurer les couleurs de navigation pour tous les boutons
        ConfigureButtonColors(startButton);
        ConfigureButtonColors(buttonSetting);
        ConfigureButtonColors(quitButton);
        
        // Sélectionner le bouton "Jouer" par défaut
        if (startButton != null)
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            _lastSelected = startButton.gameObject;
        }
    }

    void Update()
    {
        // Vérifier si aucun bouton n'est sélectionné
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Réactiver la sélection sur le dernier bouton sélectionné
            if (_lastSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected);
            }
            else if (startButton != null)
            {
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
                _lastSelected = startButton.gameObject;
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
    /// Lance le jeu en chargeant la scène de jeu principale
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Starting game - Loading scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Ouvre le menu des paramètres
    /// </summary>
    public void OpenSettings()
    {
        Debug.Log("Opening settings menu...");
        // TODO: Implémenter l'ouverture du menu des paramètres
    }

    /// <summary>
    /// Quitte l'application
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
        // Si on est dans l'éditeur Unity, arrêter le mode Play
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Si on est dans un build, quitter l'application
        Application.Quit();
        #endif
    }
}
