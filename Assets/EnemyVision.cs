using UnityEngine;

// Handle penglihatan musuh
// Script ngatur deteksi dan respawn player
public class EnemyVision : MonoBehaviour
{
    public Transform player;                 // Referensi player
    public Transform respawnPoint;           // Titik respawn player
    public LayerMask playerLayer;            // Layer player
    public LayerMask bushLayer;              // Layer semak
    public float detectionRadius = 5f;       // Radius deteksi
    public float detectionDelay = 3f;        // Waktu deteksi maksimal
    private float detectionTimer = 0f;       // Timer deteksi

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Cek player di semak
            RaycastHit2D bushCheck = Physics2D.Linecast(player.position, player.position, bushLayer);
            if (bushCheck.collider != null)
            {
                detectionTimer = 0f;
                return;
            }

            // Cek line of sight ke player
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, playerLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                detectionTimer += Time.deltaTime;

                if (detectionTimer >= detectionDelay)
                {
                    RespawnPlayer();
                }
            }
        }
        else
        {
            detectionTimer = 0f;
        }
    }

    private void RespawnPlayer()
    {
        player.position = respawnPoint.position;
        detectionTimer = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        // Visual debug area deteksi
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
