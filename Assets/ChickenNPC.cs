using UnityEngine;

// Handle NPC ayam yang bisa heal player
// Script ngatur gerakan dan interaksi ayam dengan player
public class ChickenNPC : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;           // Kecepatan gerak ayam
    public float patrolDistance = 3f;      // Jarak patroli
    public float waitTime = 1f;            // Waktu tunggu di ujung patroli

    [Header("Interaction Settings")]
    public float interactionDistance = 2f; // Jarak maksimal interaksi
    public GameObject exclamationMark;      // Prefab tanda seru
    public int healthRestoreAmount = 1;    // Jumlah HP yang diheal
    public KeyCode interactKey = KeyCode.E; // Tombol interaksi

    private float startX;                  // Posisi X awal
    private float targetX;                 // Posisi X target
    private bool movingRight = true;       // Status arah gerak
    private float waitTimer;               // Timer untuk wait
    private bool isWaiting;                // Status sedang wait
    private Animator animator;             // Reference ke animator
    private SpriteRenderer spriteRenderer; // Reference ke sprite renderer
    private GameObject currentExclamationMark; // Instance tanda seru
    private bool canInteract = false;      // Status bisa interaksi
    private PlayerHealth playerHealth;     // Reference ke player health

    // Nama parameter animator
    private static readonly string IS_MOVING = "IsMoving";
    private static readonly string IS_WAITING = "IsWaiting";

    void Start()
    {
        // Setup komponen
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on Chicken NPC!");
            enabled = false;
            return;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // Setup patroli
        startX = transform.position.x;
        targetX = startX + patrolDistance;
        waitTimer = waitTime;

        // Setup tanda seru
        if (exclamationMark != null)
        {
            currentExclamationMark = Instantiate(exclamationMark, transform);
            currentExclamationMark.SetActive(false);
        }
    }

    void Update()
    {
        // Handle wait state
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                movingRight = !movingRight;
                waitTimer = waitTime;
            }
            UpdateAnimationState();
            return;
        }

        // Update posisi
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // Flip sprite sesuai arah
        spriteRenderer.flipX = !movingRight;

        // Cek posisi patroli
        if (movingRight && transform.position.x >= targetX)
        {
            isWaiting = true;
        }
        else if (!movingRight && transform.position.x <= startX)
        {
            isWaiting = true;
        }

        UpdateAnimationState();
        CheckPlayerInteraction();
    }

    // Update state animasi
    void UpdateAnimationState()
    {
        animator.SetBool(IS_MOVING, !isWaiting);
        animator.SetBool(IS_WAITING, isWaiting);
    }

    // Cek interaksi dengan player
    void CheckPlayerInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            
            // Cek jarak interaksi
            if (distanceToPlayer <= interactionDistance)
            {
                if (!canInteract)
                {
                    canInteract = true;
                    ShowExclamationMark();
                }

                // Cek input interaksi
                if (Input.GetKeyDown(interactKey))
                {
                    InteractWithPlayer(player);
                }
            }
            else
            {
                if (canInteract)
                {
                    canInteract = false;
                    HideExclamationMark();
                }
            }
        }
    }

    // Tampilkan tanda seru
    void ShowExclamationMark()
    {
        if (currentExclamationMark != null)
        {
            currentExclamationMark.SetActive(true);
        }
    }

    // Sembunyikan tanda seru
    void HideExclamationMark()
    {
        if (currentExclamationMark != null)
        {
            currentExclamationMark.SetActive(false);
        }
    }

    // Handle interaksi dengan player
    void InteractWithPlayer(GameObject player)
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Heal player
            playerHealth.Heal(healthRestoreAmount);
            
            // Hancurkan ayam
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Cleanup tanda seru
        if (currentExclamationMark != null)
        {
            Destroy(currentExclamationMark);
        }
    }
} 