using UnityEngine;

// Handle objek yang bisa kena damage
// Script ngatur health dan efek saat objek kena damage
public class DamageableObject : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public int maxHealth = 100;            // HP maksimal objek
    private int currentHealth;             // HP sekarang

    [Header("Effects")]
    public GameObject hitEffect;           // Efek visual saat kena damage
    public AudioClip hitSound;             // SFX saat kena damage

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Handle damage ke objek
    public void TakeDamage(int damage)
    {
        // Kurangin HP
        currentHealth -= damage;

        // Play efek visual
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // Play SFX
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        // Cek HP habis
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle kematian objek
    private void Die()
    {
        // Tambah efek kematian di sini
        // Destroy(gameObject);
        
        // Atau nonaktifin objek
        gameObject.SetActive(false);
    }
} 