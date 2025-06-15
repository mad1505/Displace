using UnityEngine;
using UnityEngine.SceneManagement;

// Handle UI scene management
// Script ngatur scene UI
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        // Setup singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadUIScene()
    {
        // Load UI scene
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

    public void UnloadUIScene()
    {
        // Unload UI scene
        SceneManager.UnloadSceneAsync("UIScene");
    }
} 