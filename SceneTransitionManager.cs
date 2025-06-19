using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// Handle scene transitions
// Script ngatur transisi scene
public class SceneTransitionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fadeImage;       // Fade overlay
    public Image logoImage;       // Logo
    public Image progressBar;     // Loading bar

    [Header("Timing Settings")]
    public float fadeDuration = 1f;    // Fade time
    public float logoDuration = 2f;    // Logo display time
    public float fakeLoadTime = 2f;    // Loading time
    public string nextSceneName = "MainMenuScene"; // Next scene

    private void Awake()
    {
        // Setup UI
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 1);

        if (logoImage != null)
            logoImage.gameObject.SetActive(false);

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
            progressBar.fillAmount = 0;
        }
    }

    private void Start()
    {
        // Start sequence
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(PlaySequence());
        }
    }

    public void StartTransition()
    {
        // Start transition
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Show logo
        logoImage.gameObject.SetActive(true);

        // Fade in
        yield return StartCoroutine(Fade(1, 0));

        // Wait for logo
        yield return new WaitForSeconds(logoDuration);

        // Fade out
        yield return StartCoroutine(Fade(0, 1));

        // Setup loading
        logoImage.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(true);
        progressBar.fillAmount = 0;

        // Fade in loading
        yield return StartCoroutine(Fade(1, 0));

        // Simulate loading
        float timer = 0f;
        while (timer < fakeLoadTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fakeLoadTime);
            progressBar.fillAmount = progress;
            yield return null;
        }

        // Load next scene
        yield return StartCoroutine(Fade(0, 1));
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float from, float to)
    {
        // Handle fade effect
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, timer / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, to);
    }
}