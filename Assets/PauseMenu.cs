using UnityEngine;
using UnityEngine.SceneManagement;

// Handle pause menu
// Script ngatur menu pause
public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;           // Status pause game
    public GameObject pauseMenuUI;                  // UI menu pause
    public GameObject optionsMenuUI;                // UI menu options
    private PlayerSpawner playerSpawner;            // Referensi player spawner

    void Start()
    {
        // Get player spawner
        playerSpawner = FindObjectOfType<PlayerSpawner>();
    }

    void Update()
    {
        // Check escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        // Resume game
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        // Reset checkpoint
        PlayerPrefs.SetInt("HasCheckpoint", 0);
        PlayerPrefs.Save();

        // Reset time
        Time.timeScale = 1f;
        isPaused = false;

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadFromCheckpoint()
    {
        // Reset time
        Time.timeScale = 1f;
        isPaused = false;
        
        // Hide UI
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);

        // Hide game over
        if (GameOverUI.instance != null)
            GameOverUI.instance.HideGameOver();

        if (playerSpawner != null)
        {
            // Reset player
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.ResetHealth();

            // Load checkpoint
            playerSpawner.RespawnAtCheckpoint();
        }
        else
        {
            Debug.LogWarning("PlayerSpawner not found!");
            Restart();
        }
    }

    public void OpenOptions()
    {
        // Show options menu
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void QuitToMenu()
    {
        // Save progress
        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveGameProgress();

        // Load main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseButton()
    {
        // Pause via button
        Pause();
    }

    void Pause()
    {
        // Pause game
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}