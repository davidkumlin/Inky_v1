using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inky_animation : MonoBehaviour
{
    [SerializeField] private P_Stats pstats;
    [SerializeField] private P_Wally pwally;
    [SerializeField] private P_Inky pinky;

    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    [Header("Arms")]
    [SerializeField] private GameObject IS_Arm; 
    [SerializeField] private GameObject NS_Arm;
    [SerializeField] private GameObject RB_Arm; 
    [SerializeField] private GameObject RF_Arm; 
    [SerializeField] private GameObject LF_Arm; 
    [SerializeField] private GameObject LB_Arm; 
    [SerializeField] private GameObject S_Arm;
    [SerializeField] private GameObject N_Arm;
    [Header("IK_targets")]
    [SerializeField] private GameObject IK_IS;
    [SerializeField] private GameObject IK_NS;
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

    //animation
    private bool OnWall;
    private bool isFacingRight;
    public Animator animator;
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
    //Jump
    public bool JumpStarted = false;
    public bool hasJumped = false;
    const string Jump_up = "Jump_up";
    const string Jump_air = "Jump_air";

    //OnWall
    const string OnWall_IN = "OnWall_IN";
    const string OnWall_Idle = "OnWall_Idle";
    const string OnWall_Alert = "OnWall_Alert";
    const string OnWall_Hide = "OnWall_Hide";
    const string OnWallI_Hidden = "OnWall_Hidden";
    const string OnWallI_UnHide = "OnWall_UnHide";

    //FX
    const string Splat = "Splat";
    const string In_body = "In_body";
    const string In_body_paintspace = "In_body_paintspace";
    const string In_body_notpaintspace = "In_body_notpaintspace";
    private bool HasPlayedBodyIn = false;
    // Start is called before the first frame update
    private void Awake()
    {
        pinky = GetComponent<P_Inky>();
        if (pinky == null)
        {
            Debug.LogError("P_Inky not found");
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
    private void Update()
    {
        isFacingRight = pinky.isFacingRight;
    }

    public void Jump()
    {
        JumpStarted = true;
        ChangeAnimationState(Jump_up);
        IS_Arm.SetActive(false);
        NS_Arm.SetActive(false);
        RB_Arm.SetActive(false);
        RF_Arm.SetActive(false);
        LF_Arm.SetActive(false);
        LB_Arm.SetActive(false);
        N_Arm.SetActive(false);
        S_Arm.SetActive(false);
        
        
        if (!isFacingRight)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void MidAir()
    {
        hasJumped = true;
        ChangeAnimationState(Jump_air);
       
    }
    public void Landing()
    {
        if (pinky.isInPaintSpace)
        {
            ChangeAnimationState(In_body_paintspace);
        }
        else if (!pinky.isInPaintSpace)
        {
            ChangeAnimationState(In_body_notpaintspace);
        }
    }

    void Landed()
    {
        JumpStarted = false;
        hasJumped = false;
    }
    
    private bool hasPlayedIn = false;
    private bool hasPlayedSplat = false;
    void OnWallAnimations()
    {
        

        if (!hasPlayedSplat)
        {
            ChangeAnimationState(Splat);
            IS_Arm.SetActive(false);
            NS_Arm.SetActive(false);
            RB_Arm.SetActive(false);
            RF_Arm.SetActive(false);
            LF_Arm.SetActive(false);
            LB_Arm.SetActive(false);
            N_Arm.SetActive(false);
            S_Arm.SetActive(false);

            _IK = null;
        }

        if (!hasPlayedIn && hasPlayedSplat)
        {
            ChangeAnimationState(OnWall_IN);
        }
        else if (hasPlayedSplat && hasPlayedIn)
        {
            ChangeAnimationState(OnWall_Idle);
        }

    }

  

    void OnWallChecks()
    {
        if (!hasPlayedSplat)
        {
             hasPlayedSplat = true;

        }

        if (!hasPlayedIn)
        {
            hasPlayedIn = true;
        }
    }
    void FixedUpdate()
    {
        isGrounded = pinky.OnGround;
        float step = speed * Time.deltaTime;
        if (OnWall)
        {
            OnWallAnimations();
        }
        if (!JumpStarted)
        {
            if (!OnWall)
            {
                //reset Onwall stuff
                hasPlayedIn = false;
                hasPlayedSplat = false;

                if (!HasPlayedBodyIn)
                {
                    ChangeAnimationState(In_body);
                    IS_Arm.SetActive(false);
                    NS_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    RF_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    S_Arm.SetActive(false);


                }
                /*else if (!isGrounded || !pinky.OnLadder)
                {
                    ChangeAnimationState(Jump_air);
                    IS_Arm.SetActive(false);
                    NS_Arm.SetActive(false);
                    RB_Arm.SetActive(false);
                    RF_Arm.SetActive(false);
                    LF_Arm.SetActive(false);
                    LB_Arm.SetActive(false);
                    N_Arm.SetActive(false);
                    S_Arm.SetActive(false);

                    
                }*/
                else //Movement

                {
                
                    if (_IK == null)
                    {
                        _IK = IK_IS;
                        //Debug.Log("Initialized _IK to IK_idle");
                    }
                    if (_IK != null)
                    {

                        currentAim = pinky.CurrentAim;
                        _IK_pos = _IK.transform.localPosition;
                        // Convert the currentAim to local space
                        Vector3 currentAimLocal = _IK.transform.InverseTransformPoint(currentAim);
                        _IK.transform.localPosition = Vector2.Lerp(_IK.transform.localPosition, currentAimLocal, step);

                        Vector2 moveVector = pinky.moveVector; // Access moveVector from pinky script
                        
                        if (moveVector.x > 0 && pinky.isInPaintSpace) // Moving right
                        {

                            ChangeAnimationState(R_NE);
                            IS_Arm.SetActive(false);
                            NS_Arm.SetActive(false);
                            RB_Arm.SetActive(true);
                            RF_Arm.SetActive(false);
                            LF_Arm.SetActive(false);
                            LB_Arm.SetActive(false);
                            N_Arm.SetActive(false);
                            S_Arm.SetActive(false);

                            _IK = IK_RB;
                        }
                        else if (moveVector.x < 0 && pinky.isInPaintSpace) // Moving left
                        {

                            ChangeAnimationState(R_NW);
                            IS_Arm.SetActive(false);
                            NS_Arm.SetActive(false);
                            RB_Arm.SetActive(false);
                            RF_Arm.SetActive(false);
                            LF_Arm.SetActive(false);
                            LB_Arm.SetActive(true);
                            N_Arm.SetActive(false);
                            S_Arm.SetActive(false);

                            _IK = IK_LB;

                        }
                        if (moveVector.x > 0 && !pinky.isInPaintSpace) // Moving right
                        {

                            ChangeAnimationState(R_SE);
                            IS_Arm.SetActive(false);
                            NS_Arm.SetActive(false);
                            RB_Arm.SetActive(false);
                            RF_Arm.SetActive(true);
                            LF_Arm.SetActive(false);
                            LB_Arm.SetActive(false);
                            N_Arm.SetActive(false);
                            S_Arm.SetActive(false);

                            _IK = IK_RF;

                        }
                        else if (moveVector.x < 0 && !pinky.isInPaintSpace) // Moving left
                        {

                            ChangeAnimationState(R_SW);
                            IS_Arm.SetActive(false);
                            NS_Arm.SetActive(false);
                            RB_Arm.SetActive(false);
                            RF_Arm.SetActive(false);
                            LF_Arm.SetActive(true);
                            LB_Arm.SetActive(false);
                            N_Arm.SetActive(false);
                            S_Arm.SetActive(false);

                            _IK = IK_LF;

                        }
                        else if (moveVector.y < 0 && pinky.OnLadder) //South
                        {
                            ChangeAnimationState(R_S); //LB
                            RF_Arm.SetActive(false);
                            RB_Arm.SetActive(false);
                            LF_Arm.SetActive(false);
                            LB_Arm.SetActive(false);
                            S_Arm.SetActive(true);
                            N_Arm.SetActive(false);

                            _IK = IK_S;
                        }
                        else if (moveVector.y > 0 && pinky.OnLadder) //north
                        {
                            ChangeAnimationState(R_N); //LB
                            RF_Arm.SetActive(false);
                            RB_Arm.SetActive(false);
                            LF_Arm.SetActive(false);
                            LB_Arm.SetActive(false);
                            S_Arm.SetActive(false);
                            N_Arm.SetActive(true);

                            _IK = IK_N;
                        }
                        else if (moveVector.x == 0 && pinky.isInPaintSpace)
                        {

                            ChangeAnimationState(I_N);
                            IS_Arm.SetActive(false);
                            NS_Arm.SetActive(false);
                            RB_Arm.SetActive(false);
                            RF_Arm.SetActive(false);
                            LF_Arm.SetActive(false);
                            LB_Arm.SetActive(false);
                            N_Arm.SetActive(true);
                            S_Arm.SetActive(false);

                            _IK = IK_N;
                        }
                        else if (moveVector.x == 0 && !pinky.isInPaintSpace)
                        {
                            ChangeAnimationState(I_S);
                            IS_Arm.SetActive(false);
                            NS_Arm.SetActive(false);
                            RB_Arm.SetActive(false);
                            RF_Arm.SetActive(false);
                            LF_Arm.SetActive(false);
                            LB_Arm.SetActive(false);
                            N_Arm.SetActive(false);
                            S_Arm.SetActive(true);

                            _IK = IK_S;

                        }

                    }
                }
            }
           
           
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
        Debug.Log(newState);
    }
    void In_body_change()
    {

        HasPlayedBodyIn = true;
        Debug.Log("inbodychangedHappend");
    }
}
