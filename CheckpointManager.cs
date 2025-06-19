using UnityEngine;
using UnityEngine.SceneManagement;

// Handle checkpoint system
// Script ngatur sistem checkpoint
public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    private string currentSceneName;

    private void Awake()
    {
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

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        
        // Only reset checkpoint if loading a new level scene
        if (IsLevelScene(scene.name) && !IsLoadingFromCheckpoint())
        {
            ResetCheckpoint();
        }
    }

    // Check if scene is a level scene
    private bool IsLevelScene(string sceneName)
    {
        return sceneName.StartsWith("Level") || sceneName.Contains("Level_");
    }

    // Check if loading from checkpoint
    private bool IsLoadingFromCheckpoint()
    {
        return PlayerPrefs.GetInt("HasLastCheckpoint", 0) == 1;
    }

    // Save checkpoint position
    public void SaveCheckpoint(Vector3 position)
    {
        PlayerPrefs.SetFloat("CheckpointX", position.x);
        PlayerPrefs.SetFloat("CheckpointY", position.y);
        PlayerPrefs.SetString("CheckpointScene", currentSceneName);
        PlayerPrefs.SetInt("HasCheckpoint", 1);
        PlayerPrefs.Save();
    }

    // Get checkpoint position
    public Vector3? GetCheckpointPosition()
    {
        if (PlayerPrefs.GetInt("HasCheckpoint", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            return new Vector3(x, y, 0);
        }
        return null;
    }

    // Get checkpoint scene
    public string GetCheckpointScene()
    {
        return PlayerPrefs.GetString("CheckpointScene", "");
    }

    // Reset checkpoint
    public void ResetCheckpoint()
    {
        PlayerPrefs.DeleteKey("HasCheckpoint");
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointScene");
        PlayerPrefs.Save();
    }
} 