using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Version 08/02 V2
    private CustomInput input = null;
    public Vector2 moveVector { get; private set; } = Vector2.zero;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 cameraOffset;
    private Vector3 OnWallCameraOffset = new Vector3(0, 5, 0);
    [SerializeField] private float moveSpeed = 10f;
    private float superSpeed = 20f;
    [SerializeField] private AimMovement aimMovement;
    [SerializeField] public float maxDistance = 5f;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
   

    public float hp = 100f;
    public float maxHp = 100f;



    //For the ON WALL mechanics
    public bool OnWall { get; private set; } = false;
   
    public PositionConstraint PlayerPositionConstraint;

    private List<PaintableObject> paintableObjectsList;
    public PaintableObject paintableObject;
    public PaintableObject ActiveWall;

    private bool aimInsideMask = false;
    private bool isInPaintSpace = false;
    private bool canToggleOnWall = true;
    bool isFacingRight = true;

    private void Awake()
    {
        input = new CustomInput();
        aimMovement = GetComponentInChildren<AimMovement>();
        PlayerPositionConstraint = GetComponent<PositionConstraint>();
    }

    private void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        mainCamera = Camera.main;

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
                //     Debug.Log("PaintableObject found: " + po.gameObject.name);
            }
        }
    }

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("PM" + OnWall);

    }

    private void FixedUpdate()
    {
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
            if (po.IsAimInsideSpriteMask(aimMovement.CurrentAim))
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





        if (!OnWall)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            // Move the player normally
            rb.velocity = moveVector * moveSpeed;

            PlayerPositionConstraint.constraintActive = false;

            if (moveVector.x > 0) // Moving right
            {
                isFacingRight = true;
            }
            else if (moveVector.x < 0) // Moving left
            {
                isFacingRight = false;
            }
        }
        else //OnWall
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerOnWall");
            // Only move the player along the Y-axis while on the wall
            rb.velocity = new Vector2(moveVector.x * moveSpeed * (isFacingRight ? 1 : -1), 0);



            if (moveVector.x > 0) // Moving right
            {
                isFacingRight = true;
            }
            else if (moveVector.x < 0) // Moving left
            {
                isFacingRight = false;
            }


            // Reduce health while on the wall
            hp -= Time.deltaTime; // Decrease health by 1 per second


        }
        
        if (OnWall && !isInPaintSpace)
        {
            InvertMovement();
            Debug.Log("inverting");
        }
        else if (OnWall && isInPaintSpace)
        {
            NormMovement();
            Debug.Log("Re_inverting");
        }
        if (hp < 0)
        {
            Restart();
        }
    }
        public void Restart()
        {
            Time.timeScale = 1f;
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private bool hasInverted = false;
    public void InvertMovement()
    {
        if (!hasInverted)
        {

            isFacingRight = !isFacingRight;
            hasInverted = true;
        }


        // Code to reverse movement direction or flip player sprite
    }

    public void NormMovement()
    {
        bool newIsFacingRight = true;
        isFacingRight = newIsFacingRight;
        newIsFacingRight = isFacingRight;
        hasInverted = false;
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 cameraPosition;

            if (OnWall)
            {
                // If the player is on the wall, set the camera position to the aim position
                cameraPosition = (Vector3)aimMovement.CurrentAim + OnWallCameraOffset;
            }
            else
            {
                // If the player is not on the wall, set the camera position to the player's position
                cameraPosition = transform.position + cameraOffset;
            }

            // Keep the same z position as the camera
            cameraPosition.z = mainCamera.transform.position.z;

            // Update the camera position
            mainCamera.transform.position = cameraPosition;
        }
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.OnWall.started += OnWallStarted;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.OnWall.started -= OnWallStarted;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
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
                        newPosition.x = aimMovement.CurrentAim.x;
                        transform.position = newPosition;
                       
                   
                    rb.velocity = Vector2.zero;
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