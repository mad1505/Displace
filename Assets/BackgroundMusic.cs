using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


// ngatur musik yang muter di background, termasuk volume dan loop
// Handle musik background game
public class BackgroundMusic : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip backgroundMusicClip;     // File musik yang bakal dimuter
    public AudioMixerGroup outputMixerGroup;  // Output mixer buat atur efek suara
    public bool loop = true;                  // Loop musik terus-terusan
    public float volume = 0.5f;               // Volume default (0-1)

    [Header("Optional - Link to Volume Slider")]
    public Slider volumeSlider;               // Slider UI buat atur volume

    private AudioSource audioSource;          // Komponen buat muter musik

    private void Awake()
    {
        SetupAudioSource(); // Setup AudioSource pas game mulai
    }

    private void Start()
    {
        // Play musik kalo ada file-nya
        if (backgroundMusicClip != null)
        {
            PlayMusic();
        }

        // Setup slider volume kalo ada
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged); // Dengerin perubahan slider
            volumeSlider.value = volume; // Set nilai awal slider
        }
    }

    // Setup komponen AudioSource
    void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        // Tambahin AudioSource kalo belum ada
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Setup properti AudioSource
        audioSource.clip = backgroundMusicClip;
        audioSource.outputAudioMixerGroup = outputMixerGroup;
        audioSource.loop = loop;
        audioSource.playOnAwake = false; // Jangan play otomatis
        audioSource.volume = volume;
    }

    // Play musik kalo belum muter
    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Stop musik kalo lagi muter
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Update volume pas slider diubah
    public void OnVolumeChanged(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = newVolume;
    }
}
