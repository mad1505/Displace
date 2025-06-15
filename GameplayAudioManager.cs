using UnityEngine;
using UnityEngine.Audio;

// Handle audio gameplay
// Script ngatur suara saat gameplay
public class GameplayAudioManager : MonoBehaviour
{
    public static GameplayAudioManager instance;    // Instance singleton

    [Header("Audio Sources")]
    public AudioSource musicSource;                // Source musik
    public AudioSource sfxSource;                  // Source efek suara
    public AudioSource enemySource;                // Source suara musuh
    public AudioSource playerSource;               // Source suara player

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;              // Musik latar
    public AudioClip enemyAttackSound;             // Suara serangan musuh
    public AudioClip playerHealSound;              // Suara heal player

    [Header("Audio Mixer")]
    public AudioMixerGroup musicMixerGroup;        // Mixer musik
    public AudioMixerGroup sfxMixerGroup;          // Mixer efek suara

    private void Awake()
    {
        // Setup singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupAudioSources();
    }

    private void Start()
    {
        // Mulai musik latar
        PlayBackgroundMusic();
    }

    void SetupAudioSources()
    {
        // Setup source musik
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.outputAudioMixerGroup = musicMixerGroup;
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        // Setup source efek suara
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
            sfxSource.playOnAwake = false;
        }

        // Setup source musuh
        if (enemySource == null)
        {
            enemySource = gameObject.AddComponent<AudioSource>();
            enemySource.outputAudioMixerGroup = sfxMixerGroup;
            enemySource.playOnAwake = false;
        }

        // Setup source player
        if (playerSource == null)
        {
            playerSource = gameObject.AddComponent<AudioSource>();
            playerSource.outputAudioMixerGroup = sfxMixerGroup;
            playerSource.playOnAwake = false;
        }
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlayEnemyAttackSound()
    {
        if (enemySource != null && enemyAttackSound != null)
        {
            enemySource.PlayOneShot(enemyAttackSound);
        }
    }

    public void PlayPlayerHealSound()
    {
        if (playerSource != null && playerHealSound != null)
        {
            playerSource.PlayOneShot(playerHealSound);
        }
    }

    // Fungsi untuk mengatur volume musik
    public void SetMusicVolume(float volume)
    {
        if (musicMixerGroup != null)
        {
            musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }
    }

    // Fungsi untuk mengatur volume efek suara
    public void SetSFXVolume(float volume)
    {
        if (sfxMixerGroup != null)
        {
            sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }
    }
} 