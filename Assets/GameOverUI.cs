using UnityEngine;

// Handle UI game over
// Script ngatur tampilan game over
public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;         // Instance singleton
    public GameObject gameOverPanel;           // Panel game over

    void Awake()
    {
        // Setup singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Nonaktifkan panel di awal
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        // Tampilkan panel game over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void HideGameOver()
    {
        // Sembunyikan panel game over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
}