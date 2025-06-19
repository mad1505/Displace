using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

// Optimasi animasi game
// Matiin animasi yang gak kelihatan di kamera, biar game lebih ringan
public class AnimationOptimizer : MonoBehaviour
{
    [Header("Pengaturan Kamera")]
    public CinemachineVirtualCamera virtualCamera; // Kamera virtual
    public float viewMargin = 2f; // Margin area yang masih dianggap "kelihatan"

    [Header("Pengaturan Optimasi")]
    public float checkInterval = 0.1f; // Interval cek animasi (dalam detik)
    public bool debugMode = false; // Mode debug buat liat area "kelihatan"

    // Variabel yang dipake
    private Camera mainCamera;
    private float checkTimer;
    private List<Animator> activeAnimators = new List<Animator>(); // List animator yang aktif
    private Vector2 screenSize;
    private Vector3 cameraPosition;
    private Vector3 lastCameraPosition;

    void Start()
    {
        // Cari kamera
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (virtualCamera == null)
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        // Kalo gak nemu kamera virtual, matiin script
        if (virtualCamera == null)
        {
            Debug.LogError("Kamera Virtual Cinemachine tidak ditemukan!");
            enabled = false;
            return;
        }

        // Simpan posisi awal kamera
        lastCameraPosition = virtualCamera.transform.position;

        // Aktifin semua animator di awal
        Animator[] allAnimators = FindObjectsOfType<Animator>();
        foreach (Animator animator in allAnimators)
        {
            if (animator != null)
            {
                animator.enabled = true;
                activeAnimators.Add(animator);
            }
        }

        UpdateCameraInfo();
    }

    void Update()
    {
        // Cek animasi setiap interval
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            UpdateCameraInfo();
            CheckAnimatorsVisibility();
        }
    }

    void UpdateCameraInfo()
    {
        if (mainCamera == null || virtualCamera == null) return;

        // Update posisi kamera dan ukuran layar
        cameraPosition = virtualCamera.transform.position;

        // Hitung ukuran layar
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;
        screenSize = new Vector2(width, height);

        // Gambar area "kelihatan" kalo debug mode aktif
        if (debugMode)
        {
            DrawDebugView();
        }
    }

    void CheckAnimatorsVisibility()
    {
        // Cek animator yang aktif
        for (int i = activeAnimators.Count - 1; i >= 0; i--)
        {
            Animator animator = activeAnimators[i];
            if (animator == null)
            {
                activeAnimators.RemoveAt(i);
                continue;
            }

            // Cek kelihatan atau enggak
            bool isVisible = IsObjectVisible(animator.transform.position);
            
            // Hidupin kalo kelihatan
            if (isVisible && !animator.enabled)
            {
                animator.enabled = true;
                if (debugMode)
                {
                    Debug.Log($"Mengaktifkan animator: {animator.gameObject.name}");
                }
            }
            // Matiin kalo gak kelihatan
            else if (!isVisible && animator.enabled)
            {
                animator.enabled = false;
                if (debugMode)
                {
                    Debug.Log($"Menonaktifkan animator: {animator.gameObject.name}");
                }
            }
        }

        // Cek animator baru (dari object pooling)
        Animator[] newAnimators = FindObjectsOfType<Animator>();
        foreach (Animator animator in newAnimators)
        {
            if (animator != null && !activeAnimators.Contains(animator))
            {
                bool isVisible = IsObjectVisible(animator.transform.position);
                animator.enabled = isVisible;
                if (isVisible)
                {
                    activeAnimators.Add(animator);
                }
            }
        }
    }

    bool IsObjectVisible(Vector3 position)
    {
        if (mainCamera == null) return false;

        // Ubah posisi dunia ke viewport (0-1)
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);

        // Cek area "kelihatan" dengan margin
        bool isInView = viewportPoint.x >= -viewMargin && 
                       viewportPoint.x <= 1 + viewMargin && 
                       viewportPoint.y >= -viewMargin && 
                       viewportPoint.y <= 1 + viewMargin && 
                       viewportPoint.z > 0;

        // Cek tambahan kalo kelihatan
        if (isInView)
        {
            // Cek jarak ke kamera
            float distanceToCamera = Vector3.Distance(position, cameraPosition);
            
            // Matiin kalo terlalu jauh
            if (distanceToCamera > screenSize.magnitude * 2)
            {
                return false;
            }

            // Cek efek parallax
            ParallaxLayer parallaxLayer = null;
            Transform currentTransform = position != Vector3.zero ? 
                Physics2D.Raycast(position, Vector2.zero, 0.1f).transform : null;
            
            if (currentTransform != null)
            {
                parallaxLayer = currentTransform.GetComponent<ParallaxLayer>();
            }

            // Sesuaikan margin kalo ada parallax
            if (parallaxLayer != null)
            {
                float parallaxMargin = viewMargin * (1 + parallaxLayer.parallaxFactor);
                isInView = viewportPoint.x >= -parallaxMargin && 
                          viewportPoint.x <= 1 + parallaxMargin && 
                          viewportPoint.y >= -parallaxMargin && 
                          viewportPoint.y <= 1 + parallaxMargin;
            }
        }

        return isInView;
    }

    // Gambar area "kelihatan" di game view
    void DrawDebugView()
    {
        if (mainCamera == null) return;

        // Hitung 4 titik sudut area
        Vector3 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(-viewMargin, 1 + viewMargin, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1 + viewMargin, 1 + viewMargin, 0));
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(-viewMargin, -viewMargin, 0));
        Vector3 bottomRight = mainCamera.ViewportToWorldPoint(new Vector3(1 + viewMargin, -viewMargin, 0));

        // Gambar garis hijau
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.DrawLine(bottomLeft, topLeft, Color.green);
    }

    // Gambar area "kelihatan" di editor
    void OnDrawGizmos()
    {
        if (!debugMode || mainCamera == null) return;

        // Sama kayak DrawDebugView, tapi di editor
        Vector3 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(-viewMargin, 1 + viewMargin, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1 + viewMargin, 1 + viewMargin, 0));
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(-viewMargin, -viewMargin, 0));
        Vector3 bottomRight = mainCamera.ViewportToWorldPoint(new Vector3(1 + viewMargin, -viewMargin, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
} 