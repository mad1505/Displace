using UnityEngine;

// Kontrol kecepatan animasi dari Inspector
public class AnimationSpeedController : MonoBehaviour
{
    [Header("Kecepatan Animasi")]
    [Range(0.1f, 3f)]
    public float animationSpeed = 1f; // Kecepatan animasi (0.1 - 3)

    private Animator animator;

    void Start()
    {
        // Ambil komponen Animator
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = animationSpeed;
        }
    }

    void Update()
    {
        // Update kecepatan kalo diubah pas runtime
        if (animator != null)
        {
            animator.speed = animationSpeed;
        }
    }
}
