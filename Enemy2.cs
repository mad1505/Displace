using UnityEngine;

// Handle musuh tipe 2 (patrol dengan deteksi tepi)
// Script ngatur gerakan patrol dan pengecekan lingkungan
public class Enemy2 : MonoBehaviour
{
    [Header("Gerakan")]
    public float moveSpeed = 1.5f;           // Kecepatan patrol
    public float patrolDistance = 15f;       // Jarak patrol maksimal
    private float startX;                    // Posisi awal patrol
    private bool movingRight = true;         // Arah gerakan saat ini
    public float turnDelay = 0.5f;           // Waktu jeda sebelum berbalik
    private float turnTimer;                 // Timer untuk jeda
    private bool isWaiting = false;          // Status sedang jeda
    private float lastTurnTime;              // Waktu terakhir berbalik

    [Header("Deteksi Tepi")]
    public Transform groundCheck;            // Titik cek tanah
    public float groundCheckRadius = 0.2f;   // Radius cek tanah
    public LayerMask groundLayer;            // Layer untuk tanah
    public Transform wallCheck;              // Titik cek dinding
    public float wallCheckDistance = 0.2f;   // Jarak cek dinding

    [Header("Komponen")]
    private Rigidbody2D rb;                  // Komponen fisik
    private Animator anim;                   // Komponen animasi
    private SpriteRenderer spriteRenderer;   // Komponen sprite
    private bool isGrounded;                 // Status di tanah
    private bool isWallAhead;                // Status ada dinding

    [Header("Enemy Config")]
    public EnemyConfig enemyConfig;          // Konfigurasi musuh

    void Start()
    {
        // Setup komponen
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Setup posisi dan timer
        startX = transform.position.x;
        turnTimer = turnDelay;
        lastTurnTime = Time.time;

        // Validasi komponen
        if (rb == null) Debug.LogError("Rigidbody2D tidak ditemukan!");
        if (anim == null) Debug.LogError("Animator tidak ditemukan!");
        if (spriteRenderer == null) Debug.LogError("SpriteRenderer tidak ditemukan!");
        if (groundCheck == null) Debug.LogError("Ground Check tidak diatur!");
        if (wallCheck == null) Debug.LogError("Wall Check tidak diatur!");
    }

    void Update()
    {
        // Cek kondisi lingkungan
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isWallAhead = Physics2D.Raycast(wallCheck.position, 
            movingRight ? Vector2.right : Vector2.left, 
            wallCheckDistance, 
            groundLayer);

        // Cek jarak patrol
        float currentDistance = Mathf.Abs(transform.position.x - startX);
        bool shouldTurn = currentDistance >= patrolDistance || isWallAhead || !isGrounded;
        bool canTurn = Time.time - lastTurnTime >= turnDelay;

        // Proses berbalik
        if (!isWaiting && shouldTurn && canTurn)
        {
            StartTurn();
        }

        // Update jeda
        if (isWaiting)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0)
            {
                CompleteTurn();
            }
        }

        // Update animasi
        UpdateAnimation();
    }

    void StartTurn()
    {
        isWaiting = true;
        turnTimer = turnDelay;
        rb.velocity = Vector2.zero;
    }

    void CompleteTurn()
    {
        isWaiting = false;
        movingRight = !movingRight;
        spriteRenderer.flipX = !movingRight;
        lastTurnTime = Time.time;
    }

    void UpdateAnimation()
    {
        if (anim != null)
        {
            float speed = isWaiting ? 0f : Mathf.Abs(rb.velocity.x);
            anim.SetFloat("speed", speed);
        }
    }

    void FixedUpdate()
    {
        if (!isWaiting)
        {
            float direction = movingRight ? 1f : -1f;
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        }
    }

    void OnDrawGizmos()
    {
        // Visual debug area cek
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(wallCheck.position, 
                wallCheck.position + (movingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
        }

        // Visual area patrol
        Gizmos.color = Color.blue;
        Vector3 leftPoint = transform.position;
        leftPoint.x = startX - patrolDistance;
        Vector3 rightPoint = transform.position;
        rightPoint.x = startX + patrolDistance;
        Gizmos.DrawLine(leftPoint, rightPoint);
    }
}