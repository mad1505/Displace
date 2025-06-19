using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Handle game settings
// Script ngatur pengaturan game
public class GameSettings : MonoBehaviour
{
    [Header("Player Settings")]
    // Setting dasar player
    public float playerMaxHealth = 100f;      // Nyawa maksimal player
    public float playerRespawnDelay = 2f;     // Waktu tunggu respawn
    public int maxLives = 3;                  // Jumlah nyawa maksimal

    [Header("Checkpoint Settings")]
    // Setting buat checkpoint
    public float checkpointActivationRange = 2f;   // Jarak aktivasi checkpoint
    public Color checkpointActiveColor = Color.green;      // Warna checkpoint aktif
    public Color checkpointInactiveColor = Color.gray;     // Warna checkpoint nonaktif

    [Header("UI Settings")]
    // Setting buat UI
    public float healthBarUpdateSpeed = 5f;   // Kecepatan update health bar
    public float fadeInDuration = 1f;         // Lama waktu fade in
    public float fadeOutDuration = 1f;        // Lama waktu fade out

    [Header("Audio Settings")]
    // Setting buat audio
    public AudioMixer audioMixer;                  // Mixer audio
    public Slider musicVolumeSlider;              // Slider volume musik
    public Slider sfxVolumeSlider;                // Slider volume efek suara

    [Header("Graphics Settings")]
    public Toggle fullscreenToggle;               // Toggle fullscreen
    public Dropdown qualityDropdown;              // Dropdown kualitas grafis
    public Dropdown resolutionDropdown;           // Dropdown resolusi

    private Resolution[] resolutions;             // List resolusi yang tersedia

    void Start()
    {
        // Load pengaturan yang tersimpan
        LoadSettings();
        SetupResolutionDropdown();
    }

    void LoadSettings()
    {
        // Load volume musik
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolumeSlider.value = musicVolume;
        SetMusicVolume(musicVolume);

        // Load volume efek suara
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxVolumeSlider.value = sfxVolume;
        SetSFXVolume(sfxVolume);

        // Load fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        // Load kualitas grafis
        int qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
        qualityDropdown.value = qualityLevel;
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    void SetupResolutionDropdown()
    {
        // Ambil resolusi yang tersedia
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        // Buat list opsi resolusi
        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Setup dropdown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMusicVolume(float volume)
    {
        // Set volume musik
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Set volume efek suara
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        // Set mode fullscreen
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetQuality(int qualityIndex)
    {
        // Set kualitas grafis
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        // Set resolusi layar
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}