using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Handle pengaturan game
// Script ngatur pengaturan saat gameplay
public class OptionsGame : MonoBehaviour
{
    [Header("UI Elements")]
    public Button backButton;                      // Tombol back
    public Button creditButton;                    // Tombol credit
    public Slider musicVolumeSlider;              // Slider volume musik
    public AudioMixer audioMixer;                  // Mixer audio
    public GameObject mainMenuPanel;               // Panel menu utama
    public GameObject optionPanel;                 // Panel options

    private float initialMusicVolume;              // Volume musik awal
    private bool settingsChanged = false;          // Flag perubahan setting

    void Start()
    {
        // Setup tombol dan volume
        SetupButtons();
        SetupMusicVolume();
    }

    void SetupButtons()
    {
        // Setup listener tombol
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(SaveAndBackToMainMenu);
        }

        if (creditButton != null)
        {
            creditButton.onClick.RemoveAllListeners();
            creditButton.onClick.AddListener(GoToCreditScene);
        }
    }

    void SetupMusicVolume()
    {
        // Setup volume musik
        if (musicVolumeSlider != null)
        {
            initialMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            musicVolumeSlider.value = initialMusicVolume;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            UpdateMusicVolume(initialMusicVolume);
        }
    }

    void OnMusicVolumeChanged(float volume)
    {
        // Update volume musik
        UpdateMusicVolume(volume);
        if (volume != initialMusicVolume)
            settingsChanged = true;
    }

    void UpdateMusicVolume(float volume)
    {
        // Set volume di mixer
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SaveAndBackToMainMenu()
    {
        // Simpan setting dan kembali
        if (settingsChanged)
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
            PlayerPrefs.Save();
            settingsChanged = false;
        }

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (optionPanel != null)
            optionPanel.SetActive(false);
    }

    public void GoToCreditScene()
    {
        // Pindah ke scene credit
        SceneManager.LoadScene("Credit");
    }
}