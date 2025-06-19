using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Handle continue button
// Script ngatur tombol continue
public class ContinueButton : MonoBehaviour
{
    private Button continueButton;
    private SaveManager saveManager;

    void Start()
    {
        // Setup button
        continueButton = GetComponent<Button>();
        saveManager = SaveManager.Instance;

        // Check save data
        continueButton.interactable = saveManager.HasSavedGame();
        continueButton.onClick.AddListener(ContinueGame);
    }

    void OnDestroy()
    {
        // Clean up event listener
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(ContinueGame);
        }
    }

    void ContinueGame()
    {
        try
        {
            // Load game from last checkpoint
            saveManager.LoadGameProgress();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error continuing game: {e.Message}");
            SceneManager.LoadScene("MainMenu");
        }
    }
} 