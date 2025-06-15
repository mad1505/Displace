using UnityEngine;

// Template konfigurasi musuh
// ScriptableObject untuk setting musuh yang bisa diatur di Inspector
[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Game/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [Header("Basic Stats")]
    public float maxHealth = 100f;        // Nyawa maksimal
    public float moveSpeed = 3f;          // Kecepatan gerak
    public float attackDamage = 10f;      // Damage serangan
    public float attackRange = 2f;        // Jarak serangan
    public float detectionRange = 5f;     // Jarak deteksi player

    [Header("Patrol Settings")]
    public float patrolWaitTime = 2f;     // Waktu tunggu patrol
    public float patrolDistance = 5f;     // Jarak patrol

    [Header("Combat Settings")]
    public float attackCooldown = 1f;     // Cooldown serangan
    public float stunDuration = 1f;       // Durasi stun
    public float knockbackForce = 5f;     // Kekuatan knockback

    [Header("Visual Settings")]
    public Color normalColor = Color.white;    // Warna normal
    public Color alertColor = Color.red;       // Warna alert
    public Color stunColor = Color.blue;       // Warna stun
} 