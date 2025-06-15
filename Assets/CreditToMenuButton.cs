using UnityEngine;
using UnityEngine.SceneManagement;

// Handle tombol kembali ke menu dari credit
// Script ngatur transisi scene ke menu utama
public class CreditToMenuButton : MonoBehaviour
{
    [Header("Nama scene menu utama")]
    public string mainMenuSceneName = "MainMenu"; // Nama scene menu

    // Load scene menu utama
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
} 