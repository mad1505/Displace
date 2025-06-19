using UnityEngine;

// Handle transisi scene setelah boss mati
// Script ngatur perpindahan scene ke credit scene
public class BossSceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "credit"; // Nama scene tujuan
    public float transitionDelay = 2f;      // Delay sebelum transisi

    private static BossSceneManager instance;
    public static BossSceneManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("BossSceneManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Trigger transisi ke scene berikutnya
    public void TransitionToNextScene()
    {
        Debug.Log("TransitionToNextScene called");
        StartCoroutine(TransitionCoroutine());
    }

    // Handle delay dan transisi scene
    private System.Collections.IEnumerator TransitionCoroutine()
    {
        Debug.Log($"Waiting {transitionDelay} seconds before transition");
        yield return new WaitForSeconds(transitionDelay);

        // Coba pake SceneTransitionManager dulu
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        if (transitionManager != null)
        {
            Debug.Log($"Found SceneTransitionManager, transitioning to {nextSceneName}");
            transitionManager.nextSceneName = nextSceneName;
            transitionManager.StartTransition();
        }
        else
        {
            // Kalo gak ada, langsung load scene
            Debug.Log($"SceneTransitionManager not found, directly loading {nextSceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
} 