using UnityEngine;
using UnityEngine.SceneManagement;

// Handle scene finish
// Script ngatur transisi ke scene berikutnya
public class FinishScene : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName;        // Nama scene berikutnya
    public float delay = 2f;            // Delay sebelum pindah scene

    private bool isFinished = false;    // Status finish

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFinished && other.CompareTag("Player"))
        {
            isFinished = true;
            Finish();
        }
    }

    private void Finish()
    {
        // Pindah scene setelah delay
        Invoke(nameof(LoadNextScene), delay);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene berikutnya belum diatur");
        }
    }
} 