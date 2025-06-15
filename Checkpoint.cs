using UnityEngine;

// Handle checkpoint system
// Script ngatur sistem checkpoint
public class Checkpoint : MonoBehaviour
{
    [Header("Visual Feedback")]
    public GameObject activeVisual;           // Visual saat aktif
    public GameObject inactiveVisual;         // Visual saat nonaktif
    public ParticleSystem activateEffect;     // Efek aktivasi

    [Header("Audio")]
    public AudioSource activateSound;         // Suara aktivasi

    private bool isActivated = false;         // Status aktivasi

    private void Start()
    {
        // Setup visuals
        if (activeVisual != null) activeVisual.SetActive(false);
        if (inactiveVisual != null) inactiveVisual.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check player collision
        if (collision.CompareTag("Player") && !isActivated)
        {
            // Save checkpoint data
            SaveCheckpoint();
            
            // Visual feedback
            ShowActivationFeedback();
            
            // Play effects
            PlayActivationEffects();
        }
    }

    private void SaveCheckpoint()
    {
        try
        {
            // Save position
            PlayerPrefs.SetFloat("CheckpointX", transform.position.x);
            PlayerPrefs.SetFloat("CheckpointY", transform.position.y);
            
            // Save scene
            PlayerPrefs.SetString("CheckpointScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            
            // Set checkpoint flag
            PlayerPrefs.SetInt("HasCheckpoint", 1);
            PlayerPrefs.Save();

            // Update status
            isActivated = true;

            Debug.Log($"Checkpoint saved at: {transform.position} in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving checkpoint: {e.Message}");
        }
    }

    private void ShowActivationFeedback()
    {
        // Update visuals
        if (activeVisual != null) activeVisual.SetActive(true);
        if (inactiveVisual != null) inactiveVisual.SetActive(false);
    }

    private void PlayActivationEffects()
    {
        // Play particle effect
        if (activateEffect != null)
            activateEffect.Play();

        // Play sound
        if (activateSound != null)
            activateSound.Play();
    }
}
