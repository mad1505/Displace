using UnityEngine;

// Handle objek yang bisa nge-damage player
// Script ngatur collision damage ke player
public class DamageObject : MonoBehaviour
{
    public int damageAmount = 1;           // Jumlah damage ke player

    // Event saat collision dengan trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek collision dengan player
        if (collision.CompareTag("Player"))
        {
            // Ambil komponen PlayerHealth
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            // Kasih damage ke player
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
