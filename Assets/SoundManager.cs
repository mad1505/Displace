using UnityEngine;
using System.Collections.Generic;

// Handle sound effects
// Script ngatur efek suara
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;                    // Singleton instance
    private List<AudioSource> sfxSources = new List<AudioSource>();  // SFX sources
    private float sfxVolume = 0.75f;                       // Default volume

    private void Awake()
    {
        // Setup singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterSFXSource(AudioSource source)
    {
        // Add SFX source
        if (!sfxSources.Contains(source))
        {
            sfxSources.Add(source);
            source.volume = sfxVolume;
        }
    }

    public void UnregisterSFXSource(AudioSource source)
    {
        // Remove SFX source
        if (sfxSources.Contains(source))
        {
            sfxSources.Remove(source);
        }
    }

    public void UpdateSFXVolume(float volume)
    {
        // Update volume
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();

        // Update all sources
        foreach (var src in sfxSources)
        {
            if (src != null)
                src.volume = sfxVolume;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        // Play sound effect
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempSFX");
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = sfxVolume;
        aSource.Play();

        Destroy(tempGO, clip.length);
    }
}