using UnityEngine;

// Handle player movement
// Script ngatur gerakan player
public class PlayerMovement : MonoBehaviour
{
    [Header("Gerakan")]
    public float moveSpeed = 5f;                    // Kecepatan jalan
    private float horizontalInput = 0f;             // Input horizontal
    private float horizontalMove = 0f;              // Nilai gerakan

    [Header("Lompatan")]
    public float jumpForce = 12f;                   // Kekuatan lompat
    public float fallMultiplier = 2.5f;             // Gravitasi jatuh
    public float lowJumpMultiplier = 2f;            // Gravitasi lompat rendah
    public float jumpTime = 0.35f;                  // Durasi lompat
    private float jumpTimeCounter;                  // Timer lompat
    private bool isJumping = false;                 // Status lompat

    [Header("Ground Check")]
    public Transform groundCheck;                   // Posisi cek tanah
    public float groundCheckRadius = 0.2f;          // Radius cek tanah
    public LayerMask groundLayer;                   // Layer tanah
    private bool isGrounded;                        // Status di tanah
    private bool wasGrounded;                       // Status tanah sebelumnya

    [Header("Status")]
    private bool isCrouching = false;               // Status stealth
    private bool isStealthingUp = false;            // Status bangun stealth
    public float stealthUpDuration = 0.5f;          // Durasi bangun
    private float stealthUpTimer = 0f;              // Timer bangun
    private bool wasStealthWalking = false;         // Status stealth jalan
    private bool wasStealthIdle = false;            // Status stealth diam
    private bool isBlockedAbove = false;            // Status terhalang

    [Header("Stand Up Check")]
    public float standUpCheckHeight = 1f;           // Tinggi cek halangan
    public LayerMask obstacleLayer;                 // Layer halangan
    public Color blockedCheckColor = Color.red;     // Warna terhalang
    public Color clearCheckColor = Color.green;     // Warna aman

    private Rigidbody2D rb;                         // Rigidbody
    private Animator anim;                          // Animator
    private SpriteRenderer spriteRenderer;          // Sprite renderer

    private BoxCollider2D boxCollider;              // Collider
    private Vector2 originalColliderSize;           // Ukuran collider awal
    private Vector2 originalColliderOffset;         // Offset collider awal
    public float crouchColliderHeight = 0.5f;       // Tinggi collider stealth
    private Vector2 crouchColliderOffset;           // Offset collider stealth

    public PlayerSkillConfig skillConfig;           // Konfigurasi skill

    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Setup collider
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            originalColliderSize = boxCollider.size;
            originalColliderOffset = boxCollider.offset;
            
            float originalBottom = originalColliderOffset.y - (originalColliderSize.y * 0.5f);
            float newCenterY = originalBottom + (crouchColliderHeight * 0.5f);
            newCenterY -= 0.05f;
            
            crouchColliderOffset = new Vector2(originalColliderOffset.x, newCenterY);
        }

        // Check animator
        if (anim == null)
            Debug.LogError("Animator tidak ditemukan!");
    }

    void Update()
    {
        // Check ground
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset jump on landing
        if (!wasGrounded && isGrounded)
        {
            isJumping = false;
            anim.SetBool("isJumping", false);
        }

        // Get input
        horizontalInput = Input.GetAxis("Horizontal");
        horizontalMove = horizontalInput;

        // Jump system
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isCrouching && !isStealthingUp)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("isJumping", true);
        }

        // Hold jump
        if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.W)) && jumpTimeCounter > 0 && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }

        // Release jump
        if (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W))
            jumpTimeCounter = 0;

        // Toggle stealth
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isStealthingUp)
        {
            if (isCrouching)
            {
                if (CanStandUp())
                {
                    bool isStealthWalking = anim.GetBool("isStealth") && !anim.GetBool("isStealthIdle");
                    bool isStealthIdle = anim.GetBool("isStealth") && anim.GetBool("isStealthIdle");

                    if (isStealthWalking || isStealthIdle)
                    {
                        isStealthingUp = true;
                        stealthUpTimer = stealthUpDuration;
                        anim.SetBool("isStealthUp", true);
                        wasStealthWalking = isStealthWalking;
                        wasStealthIdle = isStealthIdle;
                    }
                    else
                    {
                        isCrouching = false;
                        anim.SetBool("isStealth", false);
                        UpdateColliderForCrouch();
                    }
                }
            }
            else
            {
                isCrouching = true;
                anim.SetBool("isStealth", true);
                UpdateColliderForCrouch();
            }
        }

        // Check obstacle
        if (isCrouching)
            isBlockedAbove = !CanStandUp();

        // Update stealth up
        if (isStealthingUp)
        {
            stealthUpTimer -= Time.deltaTime;
            if (stealthUpTimer <= 0)
            {
                isStealthingUp = false;
                isCrouching = false;
                anim.SetBool("isStealth", false);
                anim.SetBool("isStealthIdle", false);
                anim.SetBool("isStealthUp", false);
                UpdateColliderForCrouch();
            }
        }

        // Update animations
        if (isCrouching && isGrounded && !isJumping && !isStealthingUp)
        {
            float stealthSpeed = Mathf.Abs(rb.velocity.x / (moveSpeed * 0.5f));
            anim.SetFloat("stealthSpeed", stealthSpeed);
            anim.SetFloat("speed", 0f);

            if (Mathf.Abs(rb.velocity.x) > 0.01f)
                anim.SetBool("isStealthIdle", false);
            else
                anim.SetBool("isStealthIdle", true);
        }
        else if (!isCrouching && isGrounded && !isJumping && !isStealthingUp)
        {
            anim.SetFloat("speed", Mathf.Abs(rb.velocity.x / moveSpeed));
            anim.SetFloat("stealthSpeed", 0f);
            anim.SetBool("isStealthIdle", false);
        }
        else
        {
            anim.SetFloat("speed", 0f);
            anim.SetFloat("stealthSpeed", 0f);
            anim.SetBool("isStealthIdle", false);
        }

        // Update direction
        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;

        UpdateColliderForCrouch();
    }

    void FixedUpdate()
    {
        // Update movement
        float moveSpeedModifier = (isCrouching || isStealthingUp) ? moveSpeed * 0.5f : moveSpeed;
        rb.velocity = new Vector2(horizontalMove * moveSpeedModifier, rb.velocity.y);

        // Apply gravity
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !Input.GetKey(KeyCode.W))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
    }

    void OnDrawGizmos()
    {
        // Draw ground check
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void UpdateColliderForCrouch()
    {
        // Update collider size
        if (boxCollider == null) return;

        if (isCrouching)
        {
            boxCollider.size = new Vector2(originalColliderSize.x, crouchColliderHeight);
            boxCollider.offset = crouchColliderOffset;
        }
        else
        {
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;
        }
    }

    private bool CanStandUp()
    {
        // Check if can stand
        if (boxCollider == null) return true;

        Vector2 topCenter = new Vector2(
            transform.position.x + boxCollider.offset.x,
            transform.position.y + boxCollider.offset.y + (originalColliderSize.y * 0.5f)
        );

        Vector2 checkSize = new Vector2(boxCollider.size.x * 0.8f, standUpCheckHeight);
        Vector2 checkPosition = topCenter + new Vector2(0, standUpCheckHeight * 0.5f);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPosition, checkSize, 0f, obstacleLayer);
        
        Color debugColor = colliders.Length > 0 ? blockedCheckColor : clearCheckColor;
        DrawCheckBox(checkPosition, checkSize, debugColor);

        return colliders.Length == 0;
    }

    private void DrawCheckBox(Vector2 center, Vector2 size, Color color)
    {
        // Draw debug box
        Vector2 halfSize = size * 0.5f;
        
        Debug.DrawLine(
            center + new Vector2(-halfSize.x, halfSize.y),
            center + new Vector2(halfSize.x, halfSize.y),
            color
        );
        
        Debug.DrawLine(
            center + new Vector2(-halfSize.x, -halfSize.y),
            center + new Vector2(halfSize.x, -halfSize.y),
            color
        );
        
        Debug.DrawLine(
            center + new Vector2(-halfSize.x, -halfSize.y),
            center + new Vector2(-halfSize.x, halfSize.y),
            color
        );
        
        Debug.DrawLine(
            center + new Vector2(halfSize.x, -halfSize.y),
            center + new Vector2(halfSize.x, halfSize.y),
            color
        );
    }
}
