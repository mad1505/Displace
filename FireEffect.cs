using UnityEngine;

// Handle efek api
// Script ngatur animasi api
public class FireEffect : MonoBehaviour
{
    private Animator animator;           // Komponen animasi

    void Start()
    {
        // Setup animator
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Animator tidak ditemukan");
        }
        else
        {
            // Mulai animasi
            animator.Play("api2", 0, 0f);
        }
    }

    void Update()
    {
        // Loop animasi
        if (animator != null)
        {
            animator.Play("api2");
        }
    }
} 