using UnityEngine;

// Handle player skill configuration
// Script ngatur konfigurasi skill player
[CreateAssetMenu(fileName = "PlayerSkillConfig", menuName = "Game/Player Skill Config")]
public class PlayerSkillConfig : ScriptableObject
{
    [Header("Roar Skill")]
    public float roarCooldown = 5f;           // Cooldown skill
    public float roarDamage = 20f;            // Damage skill
    public float roarRange = 10f;             // Jangkauan skill
    public float roarKnockbackForce = 5f;     // Kekuatan knockback
    public float roarStunDuration = 2f;       // Durasi stun
    public GameObject roarEffectPrefab;       // Efek visual

    [Header("Stealth Skill")]
    public float stealthDuration = 5f;            // Durasi stealth
    public float stealthSpeedMultiplier = 0.5f;   // Pengali kecepatan
    public float stealthCooldown = 10f;           // Cooldown skill
    public GameObject stealthEffectPrefab;        // Efek visual

    [Header("Movement Settings")]
    public float walkSpeed = 5f;          // Kecepatan jalan
    public float runSpeed = 8f;           // Kecepatan lari
    public float jumpForce = 10f;         // Kekuatan lompat
    public float dashSpeed = 15f;         // Kecepatan dash
    public float dashDuration = 0.5f;     // Durasi dash
    public float dashCooldown = 2f;       // Cooldown dash
} 