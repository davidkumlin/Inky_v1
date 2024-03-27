using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class P_Wally : MonoBehaviour
{
    private CustomInput input = null;

    [SerializeField] private P_Stats pstats;
    [SerializeField] private P_Inky pinky;

    [SerializeField] private DrawManager_2 drawManager;// Reference to DrawManager

    public bool inkyActive = true;
    public Vector2 whereIsInky;
    public bool wallyActive = false;
    public Vector2 whereIsWally;
    private Rigidbody2D wallyRb;
    public Vector2 CurrentAim;

    public bool isFacingRight;
    [SerializeField] public Rigidbody2D aimRb;

    public Vector2 moveVector { get; private set; } = Vector2.zero;
    [SerializeField] public float smoothing = 0.1f; // Smoothing factor
    [SerializeField] public float aimspeed = 5f; // Movement speed

    private List<PaintableObject> paintableObjectsList;
    public PaintableObject paintableObject;
    public PaintableObject currentPaintableObject;
    public PaintableObject ActiveWall;
    private bool aimInsideMask = false;
    private bool canToggleOnWall = true;
    public bool IsDrawing { get; private set; } = false; // Property to expose IsDrawing
    private bool OnWall;
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    private string currentState;

    const string Idle = "Idle";
    const string OnWall_In = "OnWall_In";
    const string OnWall_Idle = "OnWall_Idle";
    const string OnWall_Alert = "OnWall_Alert";
    const string OnWall_Hide = "OnWall_Hide";
    const string OnWall_UnHide = "OnWall_UnHide";
    const string OffWall = "OffWall";
    // Start is called before the first frame update

    private void Awake()
    {
        input = new CustomInput(); // Instantiate CustomInput
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
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


    }
    void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CurrentAim = pstats.aimPos;
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
        WallyTime();
    }

    public void WallyTime()
    {
        if (OnWall && wallyRb != null)
        {
            // Calculate the desired position based on wallyMoveVector
            Vector2 desiredPosition = (Vector2)wallyRb.position +  moveVector * smoothing * Time.fixedDeltaTime * aimspeed;

            // Move the wallyRb to the desired position
            wallyRb.MovePosition(desiredPosition);

            // Set the position of aimRb to the position of wallyRb
            aimRb.MovePosition(wallyRb.position);

        }
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.OnWall.started += OnWallStarted;
        input.Player.Jump.performed += OnJumpPerformed;
     

    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.OnWall.started -= OnWallStarted;
        input.Player.Jump.performed -= OnJumpPerformed;
        

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

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {

    }
    private void OnWallStarted(InputAction.CallbackContext value)
    {

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
}
