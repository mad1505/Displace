using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

// Manager buat handle  audio di game
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // buat nyimpen satu-satunya AudioManager yang aktif

    [Header("Audio Sources")]
    public AudioSource musicSource; // Musik background
    public AudioSource sfxSource;   // Sound effect

    [Header("Audio Clips")]
    public AudioClip backgroundMusic; // Musik utama
    public AudioClip hoverSound;      // Suara hover tombol
    public AudioClip clickSound;      // Suara klik tombol

    [Header("Other Sound Effects")]
    public AudioClip[] soundEffects; // SFX tambahan (lompat, serang, dll)

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // biar gak kehapus saat load scene baru
        }
        else
        {
            Destroy(gameObject); // hapus kalau udah ada yang lain
        }
    }

    private void Start()
    {
        PlayMusic(); // langsung mainin musik pas game mulai
    }

    public void PlayMusic()
    {
        // ngecek apakah source dan musiknya gak kosong, baru mainin
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        // stop musik kalo source-nya ada
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        // mainin sound effect satu kali pake PlayOneShot
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayHoverSound()
    {
        PlaySFX(hoverSound); // mainin suara hover
    }

    public void PlayClickSound()
    {
        PlaySFX(clickSound); // mainin suara klik
    }

    // fungsi buat mainin sound effect lain berdasarkan nama clip
    public void PlaySoundEffectByName(string clipName)
    {
        foreach (AudioClip clip in soundEffects)
        {
            if (clip.name == clipName)
            {
                PlaySFX(clip); // kalau nama cocok, langsung mainin
                return;
            }
        }
        // kalau gak nemu, kasih peringatan
        Debug.LogWarning("Sound effect " + clipName + " not found!");
    }
}
