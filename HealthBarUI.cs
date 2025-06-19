using UnityEngine;
using UnityEngine.UI;

// Handle health bar UI
// Script ngatur tampilan health bar di UI
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;    // Image fill health bar

    private int maxHealth;                            // Max health player

    // Fungsi untuk mengatur nilai maksimal health
    public void SetMaxHealth(int maxHealth)
    {
        // Set nilai max health
        this.maxHealth = maxHealth;
        UpdateHealth(maxHealth);
    }

    // Fungsi untuk memperbarui tampilan health bar sesuai dengan health saat ini
    public void UpdateHealth(int currentHealth)
    {
        // Update tampilan health bar
        if (healthFillImage != null && maxHealth > 0)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthFillImage.fillAmount = fillAmount;
        }
    }
}