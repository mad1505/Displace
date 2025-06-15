using UnityEngine;

// Handle simple projectile
// Script ngatur proyektil sederhana
public class ProjectileSimple : MonoBehaviour
{
    public float lifetime = 5f;                // Durasi proyektil

    private void OnEnable()
    {
        // Setup lifetime
        CancelInvoke();
        Invoke("Deactivate", lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision
        Deactivate();
    }

    private void Deactivate()
    {
        // Disable object
        gameObject.SetActive(false);
    }
}
