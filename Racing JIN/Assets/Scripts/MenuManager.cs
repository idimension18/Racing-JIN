using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "SampleScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // S'assurer qu'un bouton est s�lectionn� par d�faut pour la navigation � la manette
        if (startButton != null)
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // R�activer la s�lection si l'utilisateur passe de la souris � la manette
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if (startButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(startButton.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Lance le jeu en chargeant la sc�ne de jeu principale
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Starting game - Loading scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Quitte l'application
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
        // Si on est dans l'�diteur Unity, arr�ter le mode Play
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Si on est dans un build, quitter l'application
        Application.Quit();
        #endif
    }
}
