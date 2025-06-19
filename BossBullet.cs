using UnityEngine;

// Handle peluru boss
// Script ngatur behavior peluru yang ditembakin boss, termasuk damage dan collision
public class BossBullet : MonoBehaviour
{
    private float speed;        // Kecepatan peluru
    private float lifetime;     // Waktu hidup peluru
    private int damage;         // Damage yang dihasilkan

    [Header("Layer Settings")]
    public LayerMask playerLayer;        // Layer player buat collision
    public LayerMask environmentLayer;   // Layer environment buat collision

    [Header("Damage Settings")]
    public int baseDamage = 10;          // Damage default

    private void OnEnable()
    {
        // Ambil setting dari pool
        speed = BossBulletPool.Instance.bulletSpeed;
        lifetime = BossBulletPool.Instance.bulletLifetime;
        damage = baseDamage;
        Invoke("ReturnToPool", lifetime); // Kembali ke pool setelah lifetime
    }

    // Setup peluru pas di-spawn
    public void Initialize(Transform player, int stage, int damage = -1)
    {
        if (damage >= 0) this.damage = damage;
        
        // Set arah ke kiri (180 derajat)
        transform.rotation = Quaternion.Euler(0, 0, 180f);
    }

    private void Update()
    {
        // Gerak ke kiri dengan kecepatan yang udah diset
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek kena player
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            // Damage player kalo ada komponen PlayerHealth
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            ReturnToPool();
        }
        // Cek kena environment (tembok, dll)
        else if (((1 << other.gameObject.layer) & environmentLayer) != 0)
        {
            ReturnToPool();
        }
    }

    // Kembali ke pool object
    private void ReturnToPool()
    {
        BossBulletPool.Instance.ReturnBullet(gameObject);
    }
} 