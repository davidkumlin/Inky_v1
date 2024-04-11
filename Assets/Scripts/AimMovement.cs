using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class AimMovement : MonoBehaviour
{
    //Version 09/02 V3
    // Controlls over aim movement
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    [SerializeField] Rigidbody2D aimRb; // Reference to the child object's Rigidbody2D
    [SerializeField] Rigidbody2D playerRb; // Reference to the player's Rigidbody2D
    public Vector2 CurrentAim; // Make currentAim accessible and assignable outside the script
    private Vector2 lastValidPosition; // Store the last valid position of the aim
    private Vector2 startingLocalPosition;
    private Vector2 IK_pos;
    private Vector2 lastIKPosition;
    private Animation_body aniBody;
    //Controlls over the feeling
    [SerializeField] private float maxDistance = 5f; // Maximum distance the aim can move away from the body
    [SerializeField] public float smoothing = 0.1f; // Smoothing factor
    [SerializeField] public float aimspeed = 5f; // Movement speed
    

    private Vector2 initialOffset; // Initial offset between player and aim
    PositionConstraint aimPositionConstraint;

    //FX controlls
    private SpriteRenderer spriteRenderer;
    
    private Animator aimAnimator;
    private string currentState;

    const string Idle = "Idle";
    const string OnWall_In = "OnWall_In";
    const string OnWall_Idle = "OnWall_Idle";
    const string OnWall_Alert = "OnWall_Alert";
    const string OnWall_Hide = "OnWall_Hide";
    const string OnWall_UnHide = "OnWall_UnHide";
    const string OffWall = "OffWall";


    //[SerializeField] private DrawManager drawManager;// Reference to DrawManager
    public bool IsDrawing { get; private set; } = false; // Property to expose IsDrawing
    public Sprite idleCrosshair;

    private PlayerMovement playerMovement; // Reference to PlayerMovement script
    private bool OnWall;
    private bool ChaseCall;
    private List<PaintableObject> paintableObjectsList;
    // New field to store the current PaintableObject the aim is inside
    public PaintableObject currentPaintableObject;
    private bool StayHidden = false;
    private bool LastOnWall = false;
    private bool AlertMode = false;

    SoundPlayer soundPlayer;



    private void Awake()
    {
        input = new CustomInput();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aimAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        aniBody = GetComponentInParent<Animation_body>();
        aimPositionConstraint = GetComponent<PositionConstraint>();
    }
    private void Start()
    {
        soundPlayer = GetComponentInChildren<SoundPlayer>();

        if (aniBody != null && aniBody._IK_pos != null)
        {
            // Set IK_pos using the local position of _IK
            IK_pos = aniBody._IK_pos;
        }
        else
        {
            Debug.LogError("IK transform or Animation_body component is missing!");
        }

       
    GameManager.OnWallChanged += OnWallStatus;
        startingLocalPosition = aimRb.transform.localPosition;

        if (playerRb == null || aimRb == null)
        {
            Debug.LogError("Player or Aim Rigidbody2D reference not assigned in the inspector.");
        }

        // Get all PaintableObject scripts in the scene
        PaintableObject[] allPaintableObjects = FindObjectsOfType<PaintableObject>();
        paintableObjectsList = new List<PaintableObject>(allPaintableObjects);

        // Disable the Collider2D on the player
        Collider2D aimCollider = GetComponent<Collider2D>();
        if (aimCollider != null)
        {
            aimCollider.enabled = false;
        }

        // Calculate the initial offset between player and aim
        initialOffset = aimRb.position - playerRb.position;

        // Set the last valid position initially to the aim's starting position
        lastValidPosition = aimRb.position;
    }
    void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("AM" + OnWall);
        if (OnWall == true)
        {
            Collider2D aimCollider = GetComponent<Collider2D>();
            aimCollider.enabled = true;
            aimPositionConstraint.constraintActive = true;
            StartCoroutine(DisableConstraintWithDelay(0.5f)); // Adjust the delay as needed
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask; //.VisibleOutsideMask or .None

        }
        else
        {
            Collider2D aimCollider = GetComponent<Collider2D>();
            aimCollider.enabled = false;
            aimPositionConstraint.constraintActive = false;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
           // playerMovement.transform.SetParent(null);


        }
    }

    private IEnumerator DisableConstraintWithDelay(float delay)
    {
     
        yield return new WaitForSeconds(delay);
        //playerMovement.transform.SetParent(this.transform);
    
        aimPositionConstraint.constraintActive = false;

    }
    
    private void Update()
    {
        aimAnimator.SetBool("OnWallBool", OnWall);
        aimAnimator.SetBool("OnWall", OnWall);
        if (playerMovement != null)
        {
            // Update OnWall based on the value from PlayerMovement
            OnWall = playerMovement.OnWall;

            if (OnWall && !AlertMode)
            {
                LastOnWall = true;
                // If OnWall is true, set the animator trigger
                ChangeAnimationState("OnWall_IN");

                //if distance to the player on the x is further away than 2,aimPositionConstraint.constraintActive = true; 
                float distanceX = Mathf.Abs(playerMovement.transform.position.x - transform.position.x);
                bool hasInverted = false;
                // Check if distance is further away than 2 units
                if (distanceX > 1)
                {
                    
                    if (!hasInverted)
                    {
                        Debug.Log(distanceX);
                    playerMovement.InvertMovement();
                        hasInverted = true;
                    }
                }
                else
                {
                    if (hasInverted)
                    {
                        hasInverted = false;
                    }

                }
                if (Enemy.ChaseCall == true)
                {
                    ChangeAnimationState("OnWall_Alert");
                    AlertMode = true;
                    {

                        if (Enemy.ChaseCall == false)
                        {
                            ChangeAnimationState("OnWall_UnHide");

                            AlertMode = false;
                        }
                    }
                }

            }


        }
        else if (OnWall && AlertMode)
        {
            LastOnWall = true;
            // If OnWall is true, set the animator trigger
            ChangeAnimationState("OnWall_Alert");
            if (Enemy.ChaseCall == false)
            {
                ChangeAnimationState("OnWall_UnHide");

                AlertMode = false;
            }
        }
        else if (!OnWall && LastOnWall)
            {
                
                ChangeAnimationState("OffWall");
                LastOnWall = false;
               

            }
            else if (!OnWall)
            {
                ChangeAnimationState("Idle");
               
                
             }
    }
    // Variable to check if the aim is inside any sprite mask
    //Debug.Log("Am/Update" + aimInsideMask);
    // Debug.Log(currentPaintableObject);
    

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


        Vector2 aimMoveVector = Vector2.zero; // Define aimMoveVector outside of the if-else blocks

        if (playerMovement != null && playerMovement.OnWall)
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
            desiredPosition = (Vector2)playerRb.position + playerMoveVector * maxDistance;
            aimMoveVector = aimRb.transform.localPosition;
            //Debug.Log("AM" + CurrentAim);
            // Other code...
        }

        // Move the aim to the desired position
        aimRb.MovePosition(Vector2.Lerp(aimRb.position, desiredPosition, smoothing * Time.fixedDeltaTime * aimspeed));
        CurrentAim = aimRb.position;



        Animate();
    }

    // Method to update the last position of the IK target
    public void UpdateLastIKPosition(Vector2 position)
    {
        lastIKPosition = position;
    }
    bool triggerdSpray = false;
    void SpraySound()
    {
        if (!triggerdSpray)
        {
            if (!OnWall)
            {

           // Debug.Log("Spraysound");
            soundPlayer.PlaySound(0);
            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Bee State", 0);
            triggerdSpray = true;
            }

        }
    }
    private void Animate()
    {
       // IsDrawing = drawManager.ActiveSpray;

        if (IsDrawing == true)
        {
            aimAnimator.SetTrigger("Draw");
            SpraySound();
        }
        else
        {
            aimAnimator.SetTrigger("Idle");
            spriteRenderer.sprite = idleCrosshair;
            triggerdSpray = false;
            soundPlayer.StopSound(0);
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
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }


        //play animation
        aimAnimator.Play(newState);
        StartCoroutine(ResetAnimatorState());
        currentState = newState;
        Debug.Log(newState);
    }
    IEnumerator ResetAnimatorState()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.1f);

        // Reset animator state
        aimAnimator.speed = 0;
        yield return null; // Yield at least one frame to apply the change
        aimAnimator.speed = 1;
    }
    bool isAnimationPlaying(Animator aimAnimator, string stateName)
    {
        if (aimAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            aimAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

