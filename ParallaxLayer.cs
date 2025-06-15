using UnityEngine;

// Handle parallax effect
// Script ngatur efek parallax
public class ParallaxLayer : MonoBehaviour
{
    [Header("Pengaturan Paralaks")]
    [Tooltip("Semakin kecil nilainya, semakin lambat bergeraknya (background = 0.1, foreground = 1)")]
    [Range(0f, 1f)] public float parallaxFactor = 0.5f;    // Faktor kecepatan parallax

    [Tooltip("Jika true, paralaks juga akan berlaku untuk gerakan vertikal")]
    public bool parallaxY = false;                         // Enable parallax vertikal

    [Tooltip("Seberapa halus pergerakan paralaks (0 = langsung, 1 = lambat)")]
    [Range(0f, 1f)] public float smoothSpeed = 0f;         // Kecepatan smooth parallax

    [Tooltip("Referensi kamera utama (cinemachine virtual cam tetap akan gerakkan main camera)")]
    public Transform cameraTransform;                       // Referensi kamera

    private Vector3 lastCameraPosition;                     // Posisi kamera terakhir
    private Vector3 initialLayerPosition;                   // Posisi awal layer

    void Start()
    {
        // Setup kamera
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // Set posisi awal
        lastCameraPosition = cameraTransform.position;
        initialLayerPosition = transform.position;
    }

    void LateUpdate()
    {
        // Hitung delta kamera
        Vector3 cameraDelta = cameraTransform.position - lastCameraPosition;

        // Hitung perpindahan layer
        float deltaX = cameraDelta.x * parallaxFactor;
        float deltaY = parallaxY ? cameraDelta.y * parallaxFactor : 0;

        // Set posisi target
        Vector3 targetPosition = new Vector3(
            transform.position.x + deltaX,
            transform.position.y + deltaY,
            transform.position.z
        );

        // Update posisi layer
        if (smoothSpeed > 0)
        {
            // Smooth movement
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1 - Mathf.Pow(1 - smoothSpeed, Time.deltaTime * 60));
        }
        else
        {
            // Direct movement
            transform.position = targetPosition;
        }

        // Update posisi kamera
        lastCameraPosition = cameraTransform.position;
    }
}