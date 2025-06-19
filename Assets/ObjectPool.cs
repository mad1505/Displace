using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;           // Tag untuk mengidentifikasi pool
        public GameObject prefab;    // Prefab yang akan di-pool
        public int size;            // Jumlah objek dalam pool
    }

    public static ObjectPool Instance;   // Singleton instance

    public List<Pool> pools;            // List pool yang tersedia
    private Dictionary<string, Queue<GameObject>> poolDictionary;  // Dictionary untuk menyimpan pool

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Inisialisasi setiap pool
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Buat objek sesuai size
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Mengambil objek dari pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool dengan tag " + tag + " tidak ditemukan!");
            return null;
        }

        // Ambil objek dari queue
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // Aktifkan objek
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Kembalikan ke queue
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    // Mengembalikan objek ke pool
    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool dengan tag " + tag + " tidak ditemukan!");
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }

    // Mengecek apakah pool memiliki objek yang tersedia
    public bool HasAvailableObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
            return false;

        foreach (GameObject obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
                return true;
        }

        return false;
    }

    // Mendapatkan jumlah objek aktif dalam pool
    public int GetActiveObjectCount(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
            return 0;

        int count = 0;
        foreach (GameObject obj in poolDictionary[tag])
        {
            if (obj.activeInHierarchy)
                count++;
        }

        return count;
    }
} 