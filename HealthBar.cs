using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Handle health bar
// Script ngatur tampilan health bar
public class HealthBar : MonoBehaviour
{
    public GameObject healthIconPrefab; // Prefab untuk gambar satu unit health
    public int maxHealth = 5;            // Total maksimal health
    public int currentHealth;            // Health saat ini

    private List<GameObject> healthIcons = new List<GameObject>(); // Daftar objek health icon

    public Slider slider;                          // Slider health bar
    public Gradient gradient;                      // Gradient warna health
    public Image fill;                             // Image fill health bar

    void Start()
    {
        // Inisialisasi health saat game dimulai
        currentHealth = maxHealth;
        // Membuat semua health icons
        CreateHealthIcons();
    }

    void CreateHealthIcons()
    {
        // Membuat health icons sesuai dengan jumlah maxHealth
        for (int i = 0; i < maxHealth; i++)
        {
            // Menginstansiasi health icon prefab di bawah transform ini
            GameObject icon = Instantiate(healthIconPrefab, transform);
            // Menambahkan health icon ke daftar
            healthIcons.Add(icon);
        }
    }

    public void TakeDamage(int amount)
    {
        // Mengurangi health sesuai dengan jumlah damage yang diterima
        currentHealth -= amount;
        // Memastikan health tidak kurang dari 0 atau lebih dari maxHealth
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Memperbarui tampilan health icons
        UpdateHealthIcons();
    }

    public void Heal(int amount)
    {
        // Menambah health sesuai dengan jumlah yang diberikan
        currentHealth += amount;
        // Memastikan health tidak kurang dari 0 atau lebih dari maxHealth
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Memperbarui tampilan health icons
        UpdateHealthIcons();
    }

    void UpdateHealthIcons()
    {
        // Memperbarui tampilan health icons sesuai dengan currentHealth
        for (int i = 0; i < healthIcons.Count; i++)
        {
            // Aktifkan health icon jika indeksnya kurang dari currentHealth, nonaktifkan jika tidak
            healthIcons[i].SetActive(i < currentHealth);
        }
    }

    public void ResetHealth()
    {
        // Mengatur ulang health ke maxHealth
        currentHealth = maxHealth;
        // Memperbarui tampilan health icons
        UpdateHealthIcons();
    }
}