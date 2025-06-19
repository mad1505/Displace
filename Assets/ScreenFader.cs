using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// Handle screen fade effects
// Script ngatur efek fade layar
public class ScreenFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;           // Fade overlay
    public float fadeDuration = 1f;   // Fade time

    private bool isFading = false;    // Fade status
    private string sceneToLoad = "";  // Target scene

    private void Awake()
    {
        // Setup fade image
        if (fadeImage != null)
        {
            fadeImage.transform.SetAsLastSibling();
            fadeImage.gameObject.SetActive(true);
            SetAlpha(1f);
        }
    }

    private void Start()
    {
        // Start fade in
        StartCoroutine(FadeIn());
    }

    public void FadeAndLoadScene(string sceneName)
    {
        // Start fade out
        if (!isFading)
        {
            sceneToLoad = sceneName;
            StartCoroutine(FadeOutAndLoad());
        }
    }

    private IEnumerator FadeIn()
    {
        // Handle fade in
        isFading = true;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
        isFading = false;

        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutAndLoad()
    {
        // Handle fade out
        isFading = true;

        if (fadeImage != null && !fadeImage.gameObject.activeSelf)
        {
            fadeImage.gameObject.SetActive(true);
            SetAlpha(0f);
        }

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SetAlpha(float alpha)
    {
        // Set fade alpha
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }
}