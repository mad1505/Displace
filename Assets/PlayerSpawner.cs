using UnityEngine;

// Handle player spawning
// Script ngatur spawn player
public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        // Check checkpoint
        if (PlayerPrefs.GetInt("HasCheckpoint", 0) == 1)
        {
            // Check scene
            string checkpointScene = PlayerPrefs.GetString("CheckpointScene", "");
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (checkpointScene == currentScene)
            {
                // Spawn at checkpoint
                float x = PlayerPrefs.GetFloat("CheckpointX");
                float y = PlayerPrefs.GetFloat("CheckpointY");
                transform.position = new Vector2(x, y);
            }
            else
            {
                // Reset checkpoint
                PlayerPrefs.SetInt("HasCheckpoint", 0);
                PlayerPrefs.DeleteKey("CheckpointX");
                PlayerPrefs.DeleteKey("CheckpointY");
                PlayerPrefs.DeleteKey("CheckpointScene");
                PlayerPrefs.Save();
            }
        }
    }

    public void RespawnAtCheckpoint()
    {
        // Check checkpoint
        if (PlayerPrefs.GetInt("HasCheckpoint", 0) == 1)
        {
            // Check scene
            string checkpointScene = PlayerPrefs.GetString("CheckpointScene", "");
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (checkpointScene == currentScene)
            {
                // Spawn at checkpoint
                float x = PlayerPrefs.GetFloat("CheckpointX");
                float y = PlayerPrefs.GetFloat("CheckpointY");
                transform.position = new Vector2(x, y);
            }
            else
            {
                // Restart level
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
                );
            }
        }
        else
        {
            // Restart level
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}