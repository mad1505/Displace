using UnityEngine;
using System.Collections;

// Handle behavior boss jeep
// Script ngatur gerakan, serangan, dan stage boss
public class BossJeep : MonoBehaviour
{
    [Header("Configuration")]
    public BossConfig config;              // Konfigurasi boss dari ScriptableObject

    [Header("References")]
    public BossHealthBar healthBar;        // Reference ke health bar
    public Transform player;               // Reference ke player
    public Transform gatlingGun;           // Posisi spawn peluru

    private int currentHealth;             // HP boss sekarang
    private int currentStage = 1;          // Stage boss sekarang
    private float startX;                  // Posisi X awal untuk patroli
    private bool movingRight = true;       // Status arah gerak
    private SpriteRenderer spriteRenderer; // Reference ke sprite renderer
    private Rigidbody2D rb;               // Reference ke rigidbody
    private float lastAttackTime;          // Waktu serangan terakhir
    private int currentShotIndex = 0;      // Index pattern tembakan

    private void Start()
    {
        // Cek config udah di-set
        if (config == null)
        {
            Debug.LogError("BossConfig not assigned to BossJeep!");
            return;
        }

        // Set HP awal dan health bar
        currentHealth = config.maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(config.maxHealth);
        }
        startX = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (config == null) return;

        // Update stage berdasarkan HP
        UpdateStage();

        // Gerakan patroli
        Patrol();

        // Serang kalo player dalam jangkauan
        if (player != null && Vector2.Distance(transform.position, player.position) <= config.attackRange)
        {
            if (Time.time >= lastAttackTime + GetCurrentAttackCooldown())
            {
                StartCoroutine(Attack());
                lastAttackTime = Time.time;
            }
        }
    }

    // Update stage berdasarkan HP
    private void UpdateStage()
    {
        int newStage = 1;
        if (currentHealth <= config.stage2Threshold)
        {
            newStage = 3;
        }
        else if (currentHealth <= config.stage1Threshold)
        {
            newStage = 2;
        }

        if (newStage != currentStage)
        {
            currentStage = newStage;
            OnStageChange();
        }
    }

    // Handle perubahan stage
    private void OnStageChange()
    {
        // Tambah efek visual/audio saat ganti stage
        Debug.Log($"Boss entering stage {currentStage}!");
    }

    // Gerakan patroli kiri-kanan
    private void Patrol()
    {
        float currentX = transform.position.x;
        
        if (movingRight)
        {
            if (currentX >= startX + config.patrolDistance)
            {
                movingRight = false;
            }
            rb.velocity = new Vector2(config.moveSpeed, rb.velocity.y);
        }
        else
        {
            if (currentX <= startX - config.patrolDistance)
            {
                movingRight = true;
            }
            rb.velocity = new Vector2(-config.moveSpeed, rb.velocity.y);
        }
    }

    // Pattern serangan boss
    private IEnumerator Attack()
    {
        int bulletsToFire = GetCurrentBulletsPerBurst();
        
        for (int i = 0; i < bulletsToFire; i++)
        {
            if (player != null)
            {
                // Hitung sudut tembakan berdasarkan pattern
                float angle = 0f;
                switch (currentShotIndex)
                {
                    case 0: // Tembak atas
                        angle = config.verticalSpread;
                        break;
                    case 1: // Tembak tengah
                        angle = 0f;
                        break;
                    case 2: // Tembak bawah
                        angle = -config.verticalSpread;
                        break;
                }

                FireBullet(angle);
                
                // Pindah ke pattern tembakan berikutnya
                currentShotIndex = (currentShotIndex + 1) % 3;
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Spawn dan setup peluru
    private void FireBullet(float angle)
    {
        GameObject bullet = BossBulletPool.Instance.GetBullet();
        if (bullet != null)
        {
            // Set posisi peluru berdasarkan pattern
            Vector3 bulletPosition = BossBulletPool.Instance.GetBulletPosition(gatlingGun.position, currentShotIndex);
            bullet.transform.position = bulletPosition;
            
            // Setup peluru
            BossBullet bulletScript = bullet.GetComponent<BossBullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(player, currentStage, config.bulletDamage);
            }
            
            bullet.SetActive(true);
        }
    }

    // Get cooldown serangan berdasarkan stage
    private float GetCurrentAttackCooldown()
    {
        switch (currentStage)
        {
            case 1: return config.stage1AttackRate;
            case 2: return config.stage2AttackRate;
            case 3: return config.stage3AttackRate;
            default: return config.stage1AttackRate;
        }
    }

    // Get jumlah peluru per burst berdasarkan stage
    private int GetCurrentBulletsPerBurst()
    {
        switch (currentStage)
        {
            case 1: return config.stage1BulletsPerBurst;
            case 2: return config.stage2BulletsPerBurst;
            case 3: return config.stage3BulletsPerBurst;
            default: return config.stage1BulletsPerBurst;
        }
    }

    // Handle damage ke boss
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        Debug.Log($"Boss took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle kematian boss
    private void Die()
    {
        // Tambah efek kematian, animasi, dll
        Debug.Log("Boss defeated!");
        
        // Matiin komponen yang gak dipake
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        enabled = false;

        // Trigger transisi scene
        if (BossSceneManager.Instance != null)
        {
            Debug.Log("Found BossSceneManager, triggering scene transition");
            BossSceneManager.Instance.TransitionToNextScene();
        }
        else
        {
            Debug.LogError("BossSceneManager not found! Make sure it exists in the scene.");
        }
    }
} 