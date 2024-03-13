using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animation_body : MonoBehaviour
{
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private AimMovement aimMovement;
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    public UnityEvent myevent;
    [SerializeField] private GameObject idleFront; // Assign your idle sprite in the Inspector
    [SerializeField] private GameObject RB_Arm; // Assign your north-east facing sprite in the Inspector
    [SerializeField] private GameObject RF_Arm; // Assign your south-east facing sprite in the Inspector
    [SerializeField] private GameObject LF_Arm; // Assign your south-west facing sprite in the Inspector
    [SerializeField] private GameObject LB_Arm; // Assign your north-west facing sprite in the Inspector
    [SerializeField] private GameObject S_Arm;
    [SerializeField] private GameObject N_Arm;


    [SerializeField] private GameObject IK_idle;
    [SerializeField] private GameObject IK_RB;
    [SerializeField] private GameObject IK_RF;
    [SerializeField] private GameObject IK_LF;
    [SerializeField] private GameObject IK_LB;
    [SerializeField] private GameObject IK_S;
    [SerializeField] private GameObject IK_N;

    private GameObject _IK = null;
    public Vector2 _IK_pos;
    private float speed = 3f;
    private Vector2 currentAim;
    private bool OnWall;
    private bool HasPlayedBodyIn = false;


    private string currentState;
    //Animation states
    //idle
    const string I_N = "I_N";
    const string I_S = "I_S";
    
    //run
    const string R_N = "R_N";
    const string R_NW = "R_NW";
    const string R_NE = "R_NE";
    const string R_SW = "R_SW";
    const string R_SE = "R_SE";
    const string R_S = "R_S";
    //roll
    //FX
    const string Splat = "Splat";
    const string In_body = "In_body";


    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found.");
        }
        else
        {
            Debug.Log("playerMovement component found on: ");
        }
        // Attempt to find AimMovement component by name
        GameObject aimGameObject = GameObject.Find("Aim");
        if (aimGameObject != null)
        {
            aimMovement = aimGameObject.GetComponent<AimMovement>();
        }

        if (aimMovement == null)
        {
            Debug.LogError("AimMovement component not found.");
        }
        else
        {
            Debug.Log("AimMovement component found on: " + aimMovement.gameObject.name);
        }
    }

    private void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        animator = GetComponent<Animator>();
        RB_Arm.SetActive(false);
        RF_Arm.SetActive(false);
        LF_Arm.SetActive(false);
        LB_Arm.SetActive(false);
        N_Arm.SetActive(false);
        S_Arm.SetActive(false);
        idleFront.SetActive(false);
        ChangeAnimationState(In_body);
        
    }
    void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("AM" + OnWall);
       
    }

   
    private void FixedUpdate()
    {
        
        float step = speed * Time.deltaTime;
        // Debug log to track _IK_pos during runtime
        //Debug.Log("_IK_pos: " + _IK_pos);
        // Only perform the search if the _IK is null
        if (_IK == null)
        {
            _IK = IK_idle;
            //Debug.Log("Initialized _IK to IK_idle");
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found.");
        }
        else
        {
           // Debug.Log("playerMovement component found");
        }

        if (aimMovement != null && aimMovement.CurrentAim != null)
        {
            currentAim = aimMovement.CurrentAim;
            // Store the local position of _IK
            _IK_pos = _IK.transform.localPosition;
            // Convert the currentAim to local space
            Vector3 currentAimLocal = _IK.transform.InverseTransformPoint(currentAim);
            _IK.transform.localPosition = Vector2.Lerp(_IK.transform.localPosition, currentAimLocal, step);
        }

    
            if (OnWall == true)
            {
                ChangeAnimationState(Splat);
                RB_Arm.SetActive(false);
                RF_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);
                N_Arm.SetActive(false);
                S_Arm.SetActive(false);
                idleFront.SetActive(false);
                HasPlayedBodyIn = false;

            }
            if (!OnWall && !HasPlayedBodyIn)
            {
                ChangeAnimationState(In_body);
                

            }

            else if (playerMovement != null && !OnWall && HasPlayedBodyIn) // Check playerMovement first
            {
                Vector2 moveVector = playerMovement.moveVector; // Access moveVector from PlayerMovement script

                if (moveVector.x > 0 && moveVector.y > 0) // Moving up and right (Northeast)
                {
                    ChangeAnimationState(R_NE); //RB
                    RB_Arm.SetActive(true);
                    RF_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    idleFront.SetActive(false);

                    _IK = IK_RB;
                }
                else if (moveVector.x > 0) // Moving right
                {
                    ChangeAnimationState(R_SE); //RB
                    RB_Arm.SetActive(true);
                    RF_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    idleFront.SetActive(false);

                    _IK = IK_RB;
                }
                else if (moveVector.x > 0 && moveVector.y < 0) // Moving down and right (Southeast)
                {
                    ChangeAnimationState(R_SE); //RF
                    RF_Arm.SetActive(true);
                    RB_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    idleFront.SetActive(false);

                    _IK = IK_RF;
                }
                else if (moveVector.x < 0 && moveVector.y < 0) // Moving down and left (Southwest)
                {
                    ChangeAnimationState(R_SW); //LF
                    RF_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    LF_Arm.SetActive(true);
                    LB_Arm.SetActive(false);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    idleFront.SetActive(false);

                    _IK = IK_LF;
                }
                else if (moveVector.x < 0 && moveVector.y > 0)
                {
                    ChangeAnimationState(R_NW); //LB
                    RF_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(true);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    idleFront.SetActive(false);

                    _IK = IK_LB;
                }
                else if (moveVector.x < 0) // Moving left
                {
                    ChangeAnimationState(R_NW); //LB
                    RF_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(true);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    idleFront.SetActive(false);

                    _IK = IK_S;
                }
                else if (moveVector.y < 0) //South
                {
                    ChangeAnimationState(R_S); //LB
                    RF_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    S_Arm.SetActive(true);
                    N_Arm.SetActive(false);

                    idleFront.SetActive(false);

                    _IK = IK_S;
                }
                else if (moveVector.y > 0) //north
                {
                    ChangeAnimationState(R_N); //LB
                    RF_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(true);
                    idleFront.SetActive(false);

                    _IK = IK_N;
                }
                else
                {
                    ChangeAnimationState(I_S);
                    idleFront.SetActive(true);
                    RF_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    S_Arm.SetActive(false);
                    N_Arm.SetActive(false);

                    _IK = IK_idle;
                }

            }
            else if (playerMovement != null && !OnWall)
            {
                //Debug.LogError("PlayerMovement script not found!");
                return; // Exit the method early
            }
        
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

    void In_body_change()
    {

        HasPlayedBodyIn = true;
        Debug.Log("inbodychangedHappend");
    }
    bool isAnimationPlaying(Animator animatorm, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
