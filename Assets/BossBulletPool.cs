using UnityEngine;
using System.Collections.Generic;

// Handle object pooling untuk peluru boss
// Script ngatur pembuatan, penyimpanan, dan penggunaan ulang peluru boss
public class BossBulletPool : MonoBehaviour
{
    public static BossBulletPool Instance { get; private set; }

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;        // Prefab peluru yang bakal di-pool
    public int poolSize = 20;              // Jumlah peluru di pool
    public float bulletSpeed = 10f;        // Kecepatan peluru
    public float bulletLifetime = 3f;      // Waktu hidup peluru

    [Header("Shooting Pattern")]
    public float topBulletOffset = 1f;     // Offset posisi peluru atas
    public float bottomBulletOffset = -1f; // Offset posisi peluru bawah
    public float middleBulletOffset = 0f;  // Offset posisi peluru tengah

    private List<GameObject> bulletPool;   // List buat nyimpen semua peluru
    private Transform player;              // Reference ke player

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bulletPool = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Buat pool peluru di awal
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewBullet();
        }
    }

    // Buat peluru baru dan masukin ke pool
    private void CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        bulletPool.Add(bullet);
    }

    // Ambil peluru dari pool yang belum aktif
    public GameObject GetBullet()
    {
        // Cari peluru yang belum aktif
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // Buat baru kalo gak ada yang kosong
        CreateNewBullet();
        return bulletPool[bulletPool.Count - 1];
    }

    // Kembalikan peluru ke pool
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    // Hitung posisi peluru berdasarkan pattern tembakan
    public Vector3 GetBulletPosition(Vector3 basePosition, int shotIndex)
    {
        float verticalOffset = 0f;
        
        // Set offset berdasarkan posisi tembakan
        switch (shotIndex)
        {
            case 0: // Tembak atas
                verticalOffset = topBulletOffset;
                break;
            case 1: // Tembak tengah
                verticalOffset = middleBulletOffset;
                break;
            case 2: // Tembak bawah
                verticalOffset = bottomBulletOffset;
                break;
        }

        return new Vector3(basePosition.x, basePosition.y + verticalOffset, basePosition.z);
    }
} 