using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_basic : MonoBehaviour
{
    [SerializeField] private P_Stats pstats;
    [SerializeField] private P_Inky pinky;
    public Rigidbody2D unitRb;
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private string currentState;
    [SerializeField] private BoxCollider2D Collider2D;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hp;
    [SerializeField] private float Damage;

    public bool changeDirection = false;
    private float DistanceToPlayer;
    //Animation states

    //const string 1 = "1";
    //const string 2 = "2";
    //const string 3 = "3";
    //const string 4 = "4";
    //const string 5 = "5";

    //bools
    private bool isFacingRight;
    private bool isGrounded;
    private bool isJumping;
    private bool isAtk;
    public bool OnWall { get; private set; } = false;
    // Start is called before the first frame update
    private void Awake()
    {
        pinky = FindObjectOfType<P_Inky>();
        if (pinky == null)
        {
            Debug.LogError("P_Inky not found");
        }
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("PStats not found");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;

    }
    void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("inky_anim" + OnWall);

    }

    private void FixedUpdate()
    {
        
        Detection();
        CheckGrounded();
        Move();
    }

    private void Move()
    {

        // Calculate movement vector
        float horizontalMovement = speed;

        // Check if changeDirection is true and invert the horizontal movement direction if needed
        if (changeDirection)
        {
            horizontalMovement *= -1f; // Invert the horizontal movement
        }

        // Apply movement to the Rigidbody
        Vector2 movement = new Vector2(horizontalMovement, unitRb.velocity.y);
        unitRb.velocity = movement;

        if (movement.x > 0)
        {
            isFacingRight = true;
            spriteRenderer.flipX = true;
        }
        else if (movement.x < 0)
        {
            isFacingRight = false;
        spriteRenderer.flipX = false;

        }

        if (isGrounded && !isJumping)
        {

            // Change animation state based on movement
            //ChangeAnimationState("Walk");
        }
    }

    private void Detection()
    {
        Vector2 directionToPlayer = pinky.transform.position - transform.position;
        DistanceToPlayer = directionToPlayer.magnitude;
        if (!isAtk)
        {
            //Debug.Log(DistanceToPlayer);
            if (DistanceToPlayer < 8)
            {
                StartATK();
            }
            if (DistanceToPlayer > 50)
            {
                changeDirection = true;
                if (changeDirection && DistanceToPlayer > 50)
                {
                    changeDirection = false;
                }
            }
        }
    }
    void StartATK()
    {
        isAtk = true;
        Jump();
        Debug.Log("StartATK");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
        }

    }
    private void Attack()
    {
        P_Stats.hp -= Damage;
    }

    private void Jump()
    {
        if (!isJumping && isGrounded)
        {
            isJumping = true;


            unitRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            Debug.Log("jump");
            
        }
    }


    private void CheckGrounded()
    {

        isGrounded = IsGrounded();
    
    }

    private bool IsGrounded()
    {
        // Define the length of the raycast
        float raycastLength = 0.9f;

        // Define the offset from the object's position to start the raycast
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - 0.1f);

        // Cast a ray downwards from the defined origin
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastLength, LayerMask.GetMask("Ground"));

        // Draw a debug line to visualize the raycast
        Debug.DrawRay(raycastOrigin, Vector2.down * raycastLength, Color.blue);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            // Object is grounded
            return true;
        }

        // Object is not grounded
        return false;
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }


        //play animation
        animator.Play(newState);

        currentState = newState;
        //Debug.Log(newState);
    }


    void Death()
    {
        speed = 0f;
        hp = 0F;

        Destroy(gameObject);

    }
}
