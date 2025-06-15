using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// Handle fade out image dan transisi scene
// Script ngatur fade out image dan pindah scene
public class ImageFadeOut : MonoBehaviour
{
    public Image imageToFade;          // Image yang mau di fade
    public AudioSource audioSource;    // Source suara
    public AudioClip soundEffect;      // Efek suara
    public float displayDuration = 5f; // Durasi tampil image
    public float fadeDuration = 3f;    // Durasi fade out
    public string nextSceneName = "MainMenu";  // Scene tujuan

    // Start dipanggil sekali saat GameObject diaktifkan: inisialisasi alpha, suara, dan mulai proses fade.
    private void Start()
    {
        // Setup fade out
        if (imageToFade != null)
        {
            SetAlpha(1f);
            PlaySoundEffect();
            StartCoroutine(WaitAndFadeOut());
        }
    }

    // Memainkan efek suara jika audioSource dan soundEffect tersedia.
    private void PlaySoundEffect()
    {
        // Mainkan efek suara
        if (audioSource != null && soundEffect != null)
        {
            audioSource.clip = soundEffect;
            audioSource.Play();
        }
    }

    // Coroutine untuk menunggu durasi tampilan lalu melakukan fade-out secara bertahap.
    private IEnumerator WaitAndFadeOut()
    {
        // Tunggu durasi tampil
        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        Color initialColor = imageToFade.color;

        // Fade out image
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
        LoadNextScene();
    }

    // Mengatur nilai alpha pada Image untuk efek transparansi.
    private void SetAlpha(float alpha)
    {
        // Set alpha image
        if (imageToFade != null)
        {
            Color color = imageToFade.color;
            color.a = alpha;
            imageToFade.color = color;
        }
    }

    // Memuat scene berikutnya setelah proses fade selesai.
    private void LoadNextScene()
    {
        // Pindah ke scene berikutnya
        SceneManager.LoadScene(nextSceneName);
    }
}