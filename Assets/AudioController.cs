using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

// Handle audio control
// Script ngatur kontrol audio
public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [Header("Scene Settings")]
    public string[] excludedScenes = { "MainMenu", "Credit" };  // Scene yang dikecualikan

    private void Awake()
    {
        // Singleton pattern
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

    private void OnEnable()
    {
        // Subscribe ke event scene loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe dari event scene loaded
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cek apakah scene ini dikecualikan
        if (IsSceneExcluded(scene.name))
            return;

        // Cek apakah scene ini memiliki VideoPlayer
        VideoPlayer[] videoPlayers = FindObjectsOfType<VideoPlayer>();
        if (videoPlayers.Length > 0)
        {
            // Jika ada VideoPlayer, stop semua audio kecuali dari VideoPlayer dan background music
            StopAllAudioExceptVideoAndMusic();
        }
    }

    // Cek apakah scene dikecualikan
    private bool IsSceneExcluded(string sceneName)
    {
        foreach (string excludedScene in excludedScenes)
        {
            if (sceneName == excludedScene)
                return true;
        }
        return false;
    }

    // Stop semua audio kecuali VideoPlayer dan background music
    public void StopAllAudioExceptVideoAndMusic()
    {
        // Dapatkan semua AudioSource
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            // Cek apakah AudioSource ini bagian dari VideoPlayer atau background music
            VideoPlayer videoPlayer = audioSource.GetComponent<VideoPlayer>();
            bool isBackgroundMusic = audioSource.clip != null && 
                                   audioSource.loop && 
                                   audioSource.playOnAwake;

            if (videoPlayer == null && !isBackgroundMusic)
            {
                // Jika bukan bagian dari VideoPlayer dan bukan background music, stop audio
                audioSource.Stop();
            }
        }

        // Stop semua AudioListener kecuali yang terkait dengan VideoPlayer
        AudioListener[] allListeners = FindObjectsOfType<AudioListener>();
        foreach (AudioListener listener in allListeners)
        {
            VideoPlayer videoPlayer = listener.GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                listener.enabled = false;
            }
        }
    }

    // Resume semua audio
    public void ResumeAllAudio()
    {
        // Enable semua AudioListener
        AudioListener[] allListeners = FindObjectsOfType<AudioListener>();
        foreach (AudioListener listener in allListeners)
        {
            listener.enabled = true;
        }
    }

    // Mute semua audio kecuali VideoPlayer dan background music
    public void MuteAllAudioExceptVideoAndMusic()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            VideoPlayer videoPlayer = audioSource.GetComponent<VideoPlayer>();
            bool isBackgroundMusic = audioSource.clip != null && 
                                   audioSource.loop && 
                                   audioSource.playOnAwake;

            if (videoPlayer == null && !isBackgroundMusic)
            {
                audioSource.mute = true;
            }
        }
    }

    // Unmute semua audio
    public void UnmuteAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = false;
        }
    }
} 