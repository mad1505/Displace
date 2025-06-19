using UnityEngine;
using System.Collections.Generic;

// Handle roar object pooling
// Script ngatur pool objek roar
public class PlayerRoarPool : MonoBehaviour
{
    public static PlayerRoarPool Instance { get; private set; }

    [Header("Roar Settings")]
    public GameObject roarPrefab;                    // Prefab roar
    public int poolSize = 10;                        // Ukuran pool

    private List<GameObject> roarPool;               // List objek roar

    private void Awake()
    {
        // Setup singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePool();
    }

    private void InitializePool()
    {
        // Create pool
        roarPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
            CreateNewRoar();
    }

    private void CreateNewRoar()
    {
        // Create new roar object
        GameObject roar = Instantiate(roarPrefab);
        roar.SetActive(false);
        roarPool.Add(roar);
    }

    public GameObject GetRoar()
    {
        // Get inactive roar
        foreach (GameObject roar in roarPool)
        {
            if (!roar.activeInHierarchy)
                return roar;
        }

        // Create new if none available
        CreateNewRoar();
        return roarPool[roarPool.Count - 1];
    }

    public void ReturnRoar(GameObject roar)
    {
        // Return roar to pool
        roar.SetActive(false);
    }
} 