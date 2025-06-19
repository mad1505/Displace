using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDragPlay : MonoBehaviour
{
    [Header("Drag Audio Clip Here")]
    public AudioClip clip;

    private AudioSource audioSource;

    void Start()
    {
        // Ambil komponen AudioSource dari GameObject ini
        audioSource = GetComponent<AudioSource>();

        if (clip != null)
        {
            audioSource.clip = clip;
        }
        else
        {
            Debug.LogWarning("AudioClip belum di-drag ke komponen AudioDragPlay!");
        }
    }

    void Update()
    {
        // Tekan tombol Space untuk memutar audio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (clip != null)
            {
                audioSource.Play();
            }
        }
    }
}
