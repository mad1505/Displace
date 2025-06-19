using UnityEngine;

// Handle player roar ability
// Script ngatur skill roar player
public class PlayerRoar : MonoBehaviour
{
    [Header("Roar Settings")]
    public float roarCooldown = 2f;                 // Cooldown roar
    public float roarSpeed = 15f;                   // Kecepatan roar
    public float roarLifetime = 2f;                 // Durasi roar
    public Transform roarSpawnPoint;                // Posisi spawn roar
    public KeyCode roarKey = KeyCode.Q;             // Tombol roar

    [Header("Damage Settings")]
    public int roarDamage = 10;                     // Damage roar

    private float lastRoarTime;                     // Waktu roar terakhir
    private PlayerRoarPool roarPool;                // Pool roar

    private void Start()
    {
        // Setup roar pool
        roarPool = PlayerRoarPool.Instance;
        lastRoarTime = -roarCooldown;
    }

    void Update()
    {
        // Check roar input
        if (Input.GetKeyDown(roarKey) && Time.time >= lastRoarTime + roarCooldown)
        {
            Roar();
            lastRoarTime = Time.time;
        }
    }

    void Roar()
    {
        // Spawn roar projectile
        GameObject roarProjectile = roarPool.GetRoar();
        if (roarProjectile != null)
        {
            roarProjectile.transform.position = roarSpawnPoint.position;
            roarProjectile.transform.rotation = transform.rotation;
            roarProjectile.SetActive(true);

            PlayerRoarProjectile projectile = roarProjectile.GetComponent<PlayerRoarProjectile>();
            if (projectile != null)
                projectile.Initialize(transform.right, roarSpeed, roarLifetime, roarDamage);
        }
    }
}
