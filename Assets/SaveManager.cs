using UnityEngine;
using UnityEngine.SceneManagement;

// Handle game saving
// Script ngatur save game
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

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

    // Save game progress
    public void SaveGameProgress()
    {
        try
        {
            // Save current scene
            string currentScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("LastScene", currentScene);
            
            // Save checkpoint data if exists
            if (PlayerPrefs.GetInt("HasCheckpoint", 0) == 1)
            {
                // Save checkpoint scene and position
                PlayerPrefs.SetString("LastCheckpointScene", PlayerPrefs.GetString("CheckpointScene"));
                PlayerPrefs.SetFloat("LastCheckpointX", PlayerPrefs.GetFloat("CheckpointX"));
                PlayerPrefs.SetFloat("LastCheckpointY", PlayerPrefs.GetFloat("CheckpointY"));
                PlayerPrefs.SetInt("HasLastCheckpoint", 1);
            }
            
            PlayerPrefs.Save();
            Debug.Log("Game progress saved successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving game progress: {e.Message}");
        }
    }

    // Load game progress
    public void LoadGameProgress()
    {
        try
        {
            // Check for last checkpoint
            if (PlayerPrefs.GetInt("HasLastCheckpoint", 0) == 1)
            {
                string savedScene = PlayerPrefs.GetString("LastCheckpointScene");
                if (!string.IsNullOrEmpty(savedScene))
                {
                    SceneManager.LoadScene(savedScene);
                    return;
                }
            }
            
            // Fallback to last scene if no checkpoint
            string lastScene = PlayerPrefs.GetString("LastScene");
            if (!string.IsNullOrEmpty(lastScene))
            {
                SceneManager.LoadScene(lastScene);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading game progress: {e.Message}");
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Check if there's a saved game
    public bool HasSavedGame()
    {
        return PlayerPrefs.GetInt("HasLastCheckpoint", 0) == 1 || 
               !string.IsNullOrEmpty(PlayerPrefs.GetString("LastScene"));
    }

    // Delete all save data
    public void ClearSaveData()
    {
        PlayerPrefs.DeleteKey("LastScene");
        PlayerPrefs.DeleteKey("HasCheckpoint");
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointScene");
        PlayerPrefs.DeleteKey("HasLastCheckpoint");
        PlayerPrefs.DeleteKey("LastCheckpointScene");
        PlayerPrefs.DeleteKey("LastCheckpointX");
        PlayerPrefs.DeleteKey("LastCheckpointY");
        PlayerPrefs.Save();
        Debug.Log("Save data cleared");
    }
} 