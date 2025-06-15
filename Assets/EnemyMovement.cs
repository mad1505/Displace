using UnityEngine;

// Handle gerakan musuh dasar
// Script ngatur patrol sederhana antara dua titik
public class EnemyMovement : MonoBehaviour
{
    [Header("Pengaturan Gerakan")]
    public float moveSpeed = 3f;         // Kecepatan patrol
    public float pointA = -5f;           // Titik patrol kiri
    public float pointB = 5f;            // Titik patrol kanan
    private bool movingToB = true;       // Status arah gerak

    void Start()
    {
        // Set rotasi awal ke kanan
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (movingToB)
        {
            // Gerak ke kanan
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, 0);

            // Cek titik B
            if (transform.position.x >= pointB)
            {
                movingToB = false;
            }
        }
        else
        {
            // Gerak ke kiri
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 180f, 0);

            // Cek titik A
            if (transform.position.x <= pointA)
            {
                movingToB = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Visual debug area patrol
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(pointA, transform.position.y, 0), 0.3f);
        Gizmos.DrawWireSphere(new Vector3(pointB, transform.position.y, 0), 0.3f);
        Gizmos.DrawLine(
            new Vector3(pointA, transform.position.y, 0),
            new Vector3(pointB, transform.position.y, 0)
        );
    }
}