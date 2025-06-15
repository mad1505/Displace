using UnityEngine;
using UnityEngine.UI;

// Handle tampilan health bar boss
// Script ngatur UI health bar boss dan update-nya
public class BossHealthBar : MonoBehaviour
{
    [Header("Health Bar Settings")]
    [SerializeField] private Image healthFillImage; // Image yang diisi sesuai HP boss

    private int maxHealth; // HP maksimal boss

    private void Awake()
    {
        // Cek image udah di-set atau belum
        if (healthFillImage == null)
        {
            Debug.LogError("Health Fill Image belum diatur di BossHealthBar!");
            return;
        }

        // Set fill amount ke 1 (100%)
        healthFillImage.fillAmount = 1f;
    }

    // Set HP maksimal boss
    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateHealth(maxHealth); // Set HP awal ke maksimal
    }

    // Update tampilan health bar sesuai HP sekarang
    public void UpdateHealth(int currentHealth)
    {
        // Cek image dan maxHealth udah di-set
        if (healthFillImage != null && maxHealth > 0)
        {
            // Hitung fill amount berdasarkan HP sekarang
            float fillAmount = (float)currentHealth / maxHealth;
            
            // Batasi nilai antara 0-1
            fillAmount = Mathf.Clamp01(fillAmount);
            
            // Update fill amount image
            healthFillImage.fillAmount = fillAmount;
            }
        }
    }
