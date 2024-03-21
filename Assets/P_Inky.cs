using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class P_Inky : MonoBehaviour
{
    private CustomInput input = null;

    //P_Stuff
    [SerializeField] private P_Stats pstats;
    [SerializeField] private P_Wally pwally;
    private inky_animation inkyani;
    
    [SerializeField] public float maxDistance = 5f;

    public bool inkyActive;
    public Vector2 whereIsInky;
    public bool wallyActive;
    public Vector2 whereIsWally;

    //movement
    public bool isFacingRight;
    [SerializeField] private Rigidbody2D inkyRb;
    [SerializeField] private Rigidbody2D aimRb;
    private bool OnLadder = false;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 20f; 
    private Vector2 currentVelocity = Vector2.zero;

    public Vector2 moveVector { get; private set; } = Vector2.zero;
    [SerializeField] private float moveSpeed = 10f;
    // Jumping
    [SerializeField] private float jumpForce = 500f;
    private bool isGrounded;

    //aim
    [SerializeField] public float smoothing = 0.1f; // Smoothing factor
    [SerializeField] public float aimspeed = 5f; // Movement speed
    private Vector2 initialOffset; // Initial offset between player and aim
    private Vector2 lastValidPosition;
    private Vector2 startingLocalPosition;
    [SerializeField] private DrawManager_2 drawManager;// Reference to DrawManager
    public bool IsDrawing { get; private set; } = false; // Property to expose IsDrawing
    public Sprite idleCrosshair;
    public Vector2 CurrentAim;

    //Wall_stuff

    private List<PaintableObject> paintableObjectsList;
    public PaintableObject paintableObject;
    public PaintableObject currentPaintableObject;
    public PaintableObject ActiveWall;
    private bool aimInsideMask = false;
    private bool canToggleOnWall = true;
    public bool isInPaintSpace = false;
    public bool OnWall { get; private set; } = false;

    // Start is called before the first frame update

    private void Awake()
    {
        input = new CustomInput(); // Instantiate CustomInput
        {
            Debug.Log("no input");
        }
        inkyani = GetComponent<inky_animation>();
    }


    void Start()
    {
       
        GameManager.OnWallChanged += OnWallStatus;
        inkyActive = true;
        startingLocalPosition = aimRb.transform.localPosition;

        if (inkyRb == null || aimRb == null)
        {
            Debug.LogError("Player or Aim Rigidbody2D reference not assigned in the inspector.");
        }

        // Get all PaintableObject scripts in the scene
        PaintableObject[] allPaintableObjects = FindObjectsOfType<PaintableObject>();
        paintableObjectsList = new List<PaintableObject>(allPaintableObjects);
        if (paintableObjectsList.Count == 0)
        {
            Debug.LogWarning("No PaintableObject scripts found in the scene.");
        }
        else
        {
            foreach (PaintableObject po in paintableObjectsList)
            {
                Debug.Log("PaintableObject found: " + po.gameObject.name);
            }
        }

        // Calculate the initial offset between player and aim
        initialOffset = aimRb.position - inkyRb.position;

        // Set the last valid position initially to the aim's starting position
        lastValidPosition = aimRb.position;
    }
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        Move();
        CheckGrounded();

        whereIsInky = inkyRb.position;
        //Debug.Log(hp);
        if (paintableObject)
        {

            isInPaintSpace = paintableObject.IsInPaintSpace;
            //Debug.Log("PM body" + isInPaintSpace);
        }
        else
        {
            isInPaintSpace = false;

        }

        foreach (PaintableObject po in paintableObjectsList)
        {
            if (po.IsAimInsideSpriteMask(CurrentAim))
            {
                aimInsideMask = true;
                //Debug.Log("PM-Aim is inside sprite mask of " + po.gameObject.name);
                ActiveWall = po;
                //Debug.Log(ActiveWall + po.name);
                break;
            }
            else
            {
                aimInsideMask = false;

                //Debug.Log("AM-Aim is NOT inside sprite mask of " + po.gameObject.name);
            }
        }
        if (inkyRb == null || aimRb == null)
        {
            return;
        }

        Vector2 desiredPosition;


        Vector2 aimMoveVector = Vector2.zero; // Define aimMoveVector outside of the if-else blocks

        if (inkyRb != null && OnWall)
        {
            // If on a wall, calculate movement based on the left stick (Movement)
            aimMoveVector = input.Player.Movement.ReadValue<Vector2>();
            desiredPosition = (Vector2)aimRb.position + aimMoveVector * maxDistance * smoothing * Time.fixedDeltaTime * aimspeed;
            // Debug.Log("Calculated Movement based on Left Stick: " + desiredPosition);
        }
        else
        {
            // If outside or not on a wall, use the default movement logic with the right stick (Aim)
            Vector2 playerMoveVector = input.Player.Aim.ReadValue<Vector2>();
            desiredPosition = (Vector2)inkyRb.position + playerMoveVector * maxDistance;
            aimMoveVector = aimRb.transform.localPosition;
            //Debug.Log("AM" + CurrentAim);
            // Other code...
        }

        // Move the aim to the desired position
        aimRb.MovePosition(Vector2.Lerp(aimRb.position, desiredPosition, smoothing * Time.fixedDeltaTime * aimspeed));
        CurrentAim = aimRb.position;



       
    }

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("P_inky" + OnWall);
    }

    

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.OnWall.started += OnWallStarted;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Aim.performed -= OnAimPerformed;
        input.Player.Aim.canceled -= OnAimCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.OnWall.started -= OnWallStarted;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Aim.performed += OnAimPerformed;
        input.Player.Aim.canceled += OnAimCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        //Debug.Log("Movement performed: " + moveVector); // Add debug log
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void Move()
    {
        if (!inkyActive || OnWall)
        {
            // If not active or on wall, don't move
            moveVector = Vector2.zero;
            currentVelocity = Vector2.zero; // Reset current velocity when not moving
            return;
        }

        Vector2 targetVelocity = Vector2.zero;

        if (!OnLadder)
        {
            // Restrict movement to only the X-axis when not on a ladder
            targetVelocity.x = moveVector.x * moveSpeed;
        }
        else
        {
            // Allow free movement when on a ladder
            targetVelocity = moveVector * moveSpeed;
        }

        // Apply acceleration
        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, acceleration * Time.deltaTime);
        currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, targetVelocity.y, acceleration * Time.deltaTime);

        // Apply velocity to Rigidbody
        inkyRb.MovePosition(inkyRb.position + currentVelocity * Time.deltaTime);

        // Check if the player is trying to stop
        if (moveVector == Vector2.zero)
        {
            // Decelerate quickly if no movement input
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0f, deceleration * Time.deltaTime);
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, 0f, deceleration * Time.deltaTime);
        }

        if (moveVector.x > 0) // Moving right
        {
            isFacingRight = true;
        }
        else if (moveVector.x < 0) // Moving left
        {
            isFacingRight = false;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (isGrounded)
        {
            inkyRb.AddForce(Vector2.up * jumpForce);
        }
    }

    private void CheckGrounded()
    {

        isGrounded = IsGrounded();
    }

    private bool IsGrounded()
    {
        //Debug.Log(isGrounded);
        LayerMask groundLayer = LayerMask.GetMask("Ground");               
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(
        transform.position,   // Center of the capsule
        GetComponent<CapsuleCollider2D>().size,   // Size of the capsule
        CapsuleDirection2D.Vertical,   // Direction of the capsule
        0f,   // Angle of rotation (0 degrees)
        groundLayer   // Layer mask to filter colliders
        );

        // Check if any colliders were found (indicating the player is grounded)
        return colliders.Length > 0;
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
    private void OnWallStarted(InputAction.CallbackContext value)
    {
        if (canToggleOnWall)
        {
            if (paintableObjectsList == null || paintableObjectsList.Count == 0)
            {
                Debug.LogWarning("No PaintableObject scripts found in the scene.");
                return;
            }

            // Now use isInPaintSpace in your condition
            if (aimInsideMask && isInPaintSpace)
            {

                OnWall = !OnWall;
                GameManager.Instance.OnWall = OnWall;
                if (OnWall)
                {
                    // Set the player's position to match the aim's X-axis position
                    Vector3 newPosition = transform.position;
                    newPosition.x = CurrentAim.x;
                    transform.position = newPosition;


                    aimRb.velocity = Vector2.zero;
                }

                StartCoroutine(ToggleCooldown());
                return;
            }

            Debug.Log("Cannot set OnWall: Aim is not inside the sprite mask of any PaintableObject.");
        }
    }

    private IEnumerator ToggleCooldown()
    {
        canToggleOnWall = false;
        yield return new WaitForSeconds(1f);
        canToggleOnWall = true;
    }
}


