using System.Collections;
using UnityEngine;

// Handle patrol musuh dasar
// Script ngatur gerakan patrol antara dua titik
public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;           // Titik patrol kiri
    public Transform pointB;           // Titik patrol kanan
    public float speed = 2f;           // Kecepatan patrol
    public float waitTime = 2f;        // Waktu tunggu di titik

    private Transform targetPoint;     // Titik tujuan saat ini
    private bool isWaiting = false;    // Status sedang tunggu

    void Start()
    {
        // Setup awal
        targetPoint = pointA;
        transform.eulerAngles = new Vector3(0, 180f, 0);
    }

    void Update()
    {
        if (!isWaiting)
        {
            // Gerak ke titik tujuan
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // Cek sampai di titik
            if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                StartCoroutine(WaitAtPoint());
            }
        }
    }

    IEnumerator WaitAtPoint()
    {
        // Mulai tunggu
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Ganti titik tujuan
        if (targetPoint == pointA)
        {
            targetPoint = pointB;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            targetPoint = pointA;
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }

        isWaiting = false;
    }
}