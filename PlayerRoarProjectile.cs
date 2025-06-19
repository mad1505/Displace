using UnityEngine;

// Handle roar projectile
// Script ngatur proyektil roar
public class PlayerRoarProjectile : MonoBehaviour
{
    private float speed;                            // Kecepatan proyektil
    private float lifetime;                         // Durasi proyektil
    private Vector2 direction;                      // Arah proyektil
    private float currentLifetime;                  // Timer durasi
    private int damage;                             // Damage proyektil

    [Header("Layer Settings")]
    public LayerMask bossLayer;                     // Layer boss
    public LayerMask environmentLayer;              // Layer environment

    [Header("Damage Settings")]
    public int baseDamage = 10;                     // Damage dasar

    private void OnEnable()
    {
        // Reset on enable
        currentLifetime = 0f;
        damage = baseDamage;
    }

    public void Initialize(Vector2 direction, float speed, float lifetime, int damage = -1)
    {
        // Setup proyektil
        this.direction = direction;
        this.speed = speed;
        this.lifetime = lifetime;
        if (damage >= 0) this.damage = damage;
        
        // Set rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        // Move proyektil
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Check lifetime
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= lifetime)
            ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check boss hit
        if (((1 << other.gameObject.layer) & bossLayer) != 0)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damage);
            ReturnToPool();
        }
        // Check environment hit
        else if (((1 << other.gameObject.layer) & environmentLayer) != 0)
            ReturnToPool();
    }

    private void ReturnToPool()
    {
        // Return to pool
        PlayerRoarPool.Instance.ReturnRoar(gameObject);
    }
} 