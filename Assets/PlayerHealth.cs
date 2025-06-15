using UnityEngine;

// Handle player health
// Script ngatur health player
public class PlayerHealth : MonoBehaviour
{
    [Header("Pengaturan Health")]
    public int maxHealth = 5;                       // Max health
    public int currentHealth;                       // Health saat ini

    [Header("Referensi UI")]
    public HealthBarUI healthBarUI;                 // UI health bar
    public PlayerBlink playerBlink;                 // Efek kedip

    [Header("Animasi")]
    public Animator animator;                       // Animator player
    public float dieAnimationDuration = 1.5f;       // Durasi animasi mati

    private bool isDying = false;                   // Status mati
    private string[] animationTriggers = { "Idle", "Run", "Jump", "Fall", "Crouch", "Stealth" };  // Trigger animasi
    private string[] animationBools = { "isJumping", "isFalling", "isRunning", "isCrouching", "isStealth" };  // Bool animasi

    void Start()
    {
        // Setup health
        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);

        // Get animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
                Debug.LogError("Animator tidak ditemukan!");
        }
    }

    public void TakeDamage(int amount)
    {
        // Skip if dying
        if (isDying) return;

        // Update health
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBarUI.UpdateHealth(currentHealth);

        // Blink effect
        if (playerBlink != null)
            playerBlink.TakeDamage();

        // Check death
        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        // Skip if dying
        if (isDying) return;

        // Update health
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBarUI.UpdateHealth(currentHealth);
    }

    private void Die()
    {
        // Skip if already dying
        if (isDying) return;

        isDying = true;
        Debug.Log("Player Dead!");

        // Disable movement
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Reset animations
        if (animator != null)
        {
            // Reset triggers
            foreach (string trigger in animationTriggers)
                animator.ResetTrigger(trigger);

            // Reset bools
            foreach (string boolParam in animationBools)
                animator.SetBool(boolParam, false);

            // Play death animation
            animator.Rebind();
            animator.Update(0f);
            animator.Play("Die", 0, 0f);
            animator.SetTrigger("Die");

            // Show game over
            Invoke("ShowGameOver", dieAnimationDuration);
        }
        else
        {
            // Show game over immediately
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        // Show game over UI
        if (GameOverUI.instance != null)
            GameOverUI.instance.ShowGameOver();
        else
            Debug.LogWarning("GameOverUI tidak ditemukan!");

        // Stop time
        Time.timeScale = 0f;
    }

    public void ResetHealth()
    {
        // Reset health
        currentHealth = maxHealth;
        healthBarUI.UpdateHealth(currentHealth);
        isDying = false;

        // Enable movement
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Reset animation
        if (animator != null)
            animator.SetTrigger("Idle");
    }
}