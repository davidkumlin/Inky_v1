using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimMovement : MonoBehaviour
{
    //Version 08/02 V2
    // Controlls over aim movement
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    [SerializeField] Rigidbody2D aimRb; // Reference to the child object's Rigidbody2D
    [SerializeField] Rigidbody2D playerRb; // Reference to the player's Rigidbody2D
    public Vector2 CurrentAim; // Make currentAim accessible and assignable outside the script
    private Vector2 lastValidPosition; // Store the last valid position of the aim



    //Controlls over the feeling
    [SerializeField] private float maxDistance = 5f; // Maximum distance the aim can move away from the body
    [SerializeField] public float smoothing = 0.1f; // Smoothing factor
    [SerializeField] public float aimspeed = 5f; // Movement speed

    private Vector2 initialOffset; // Initial offset between player and aim

    //FX controlls
    private SpriteRenderer spriteRenderer;
    private Animator aimAnimator;
    [SerializeField] private DrawManager drawManager;// Reference to DrawManager
    public bool IsDrawing { get; private set; } = false; // Property to expose IsDrawing
    public Sprite idleCrosshair;

    private PlayerMovement playerMovement; // Reference to PlayerMovement script
    private bool OnWall;


    private void Awake()
    {
        input = new CustomInput();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aimAnimator = GetComponent<Animator>();
        // Get the PlayerMovement component from the player GameObject
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    private void Start()
    {


        GameManager.OnWallChanged += OnWallStatus;

        if (playerRb == null || aimRb == null)
        {
            Debug.LogError("Player or Aim Rigidbody2D reference not assigned in the inspector.");
        }

        // Disable the Collider2D on the player
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // Calculate the initial offset between player and aim
        initialOffset = aimRb.position - playerRb.position;

        // Set the last valid position initially to the aim's starting position
        lastValidPosition = aimRb.position;
    }
    void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
    }
    private void Update()
    {
        if (playerMovement != null)
        {
            // Update OnWall based on the value from PlayerMovement
            OnWall = playerMovement.OnWall;

            if (OnWall)
            {
                // If OnWall is true, set the animator trigger
                aimAnimator.SetTrigger("OnWall");
                aimAnimator.SetBool("OnWallBool", true);
            }
            if (OnWall == false)
            {
                // If OnWall is true, set the animator trigger
                aimAnimator.SetTrigger("OnWall");
                aimAnimator.SetBool("OnWallBool", false);
            }
        }
    }
    public void SetPlayerMovement(PlayerMovement pm)
    {
        playerMovement = pm;
    }
    public float GetAimSpeed()
    {
        return aimspeed;
    }
    public float GetSmoothing()
    {
        return smoothing;
    }

    public void SetSmoothing(float newSmoothing)
    {
        smoothing = newSmoothing;
    }
    // Methods to update variables from UI sliders
    public void UpdateAimSpeed(float newAimSpeed)
    {
        aimspeed = newAimSpeed;
    }

    public void UpdateSmoothing(float newSmoothing)
    {
        smoothing = newSmoothing;
    }
    public void UpdateAimPosition(Vector2 desiredPosition)
    {
        aimRb.MovePosition(desiredPosition);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Aim.performed += OnAimPerformed;
        input.Player.Aim.canceled += OnAimCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Aim.performed -= OnAimPerformed;
        input.Player.Aim.canceled -= OnAimCancelled;
    }



    private void FixedUpdate()
    {
        if (playerRb == null || aimRb == null)
        {
            return;
        }

        Vector2 desiredPosition;

        if (playerMovement != null && playerMovement.OnWall)
        {
            Vector2 aimMoveVector = input.Player.Movement.ReadValue<Vector2>();
            desiredPosition = (Vector2)aimRb.position + aimMoveVector * maxDistance;
        }
        else
        {
            Vector2 playerMoveVector = input.Player.Aim.ReadValue<Vector2>();
            desiredPosition = (Vector2)playerRb.position + playerMoveVector * maxDistance;
        }

        float distanceFromPlayer = Vector2.Distance(playerRb.position, desiredPosition);

        if (distanceFromPlayer > maxDistance)
        {
            Vector2 direction = (desiredPosition - (Vector2)playerRb.position).normalized;
            desiredPosition = (Vector2)playerRb.position + direction * maxDistance;
        }

        aimRb.MovePosition(Vector2.Lerp(aimRb.position, desiredPosition, smoothing * Time.fixedDeltaTime * aimspeed));
        CurrentAim = aimRb.position;

        Animate();
    }


    private void Animate()
    {
        IsDrawing = drawManager.ActiveSpray;

        if (IsDrawing == true)
        {
            aimAnimator.SetTrigger("Draw");
        }
        else
        {
            aimAnimator.SetTrigger("Idle");
            spriteRenderer.sprite = idleCrosshair;
        }



    }

    private void OnAimPerformed(InputAction.CallbackContext value)
    {
        // Read the raw value of the right stick if not on the wall
        if (!OnWall)
        {
            moveVector = value.ReadValue<Vector2>();
        }
    }

    private void OnAimCancelled(InputAction.CallbackContext value)
    {
        // Reset the moveVector only if not on the wall
        if (!OnWall)
        {
            moveVector = Vector2.zero;
        }
    }
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (playerMovement != null && playerMovement.OnWall)
        {
            // Read the raw value of the left stick input for aim movement when on the wall
            moveVector = value.ReadValue<Vector2>();
        }
    }
}