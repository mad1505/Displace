using UnityEngine;

// Konfigurasi boss yang bisa diatur dari Inspector
// ScriptableObject buat nyimpen setting boss yang bisa dipake ulang
[CreateAssetMenu(fileName = "BossConfig", menuName = "Game/Boss Configuration")]
public class BossConfig : ScriptableObject
{
    [Header("Health Settings")]
    public int maxHealth = 1000;           // HP maksimal boss
    public int stage1Threshold = 700;      // Threshold masuk stage 1 (70% HP)
    public int stage2Threshold = 300;      // Threshold masuk stage 2 (30% HP)

    [Header("Movement Settings")]
    public float moveSpeed = 3f;           // Kecepatan gerak boss
    public float patrolDistance = 5f;      // Jarak patroli
    public float attackRange = 15f;        // Jarak serang

    [Header("Attack Settings")]
    public float stage1AttackRate = 0.5f;  // Rate serang stage 1
    public float stage2AttackRate = 0.3f;  // Rate serang stage 2
    public float stage3AttackRate = 0.2f;  // Rate serang stage 3
    public int stage1BulletsPerBurst = 3;  // Jumlah peluru per burst stage 1
    public int stage2BulletsPerBurst = 5;  // Jumlah peluru per burst stage 2
    public int stage3BulletsPerBurst = 8;  // Jumlah peluru per burst stage 3
    public float verticalSpread = 90f;     // Spread tembakan vertikal
    public float bulletSpeed = 10f;        // Kecepatan peluru
    public float bulletLifetime = 3f;      // Waktu hidup peluru
    public int bulletDamage = 10;          // Damage per peluru
} 