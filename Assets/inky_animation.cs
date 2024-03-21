using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inky_animation : MonoBehaviour
{
    [SerializeField] private P_Stats pstats;
    [SerializeField] private P_Wally pwally;
    [SerializeField] private P_Inky pinky;
    
    private SpriteRenderer spriteRenderer;
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


    //FX
    const string Splat = "Splat";
    const string In_body = "In_body";
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
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (!OnWall && !HasPlayedBodyIn)
        {
            ChangeAnimationState(In_body);

        }
        else
        {

         Vector2 moveVector = pinky.moveVector; // Access moveVector from pinky script

        if (moveVector.x > 0 && pinky.isInPaintSpace) // Moving right
        {

             ChangeAnimationState(R_NE);

        }
        else if (moveVector.x < 0 && pinky.isInPaintSpace) // Moving left
        {

            ChangeAnimationState(R_NW);

            }
            if (moveVector.x > 0 && !pinky.isInPaintSpace) // Moving right
            {

                ChangeAnimationState(R_SE);

            }
            else if (moveVector.x < 0 && !pinky.isInPaintSpace) // Moving left
            {

                ChangeAnimationState(R_SW);

            }


            if (moveVector.x == 0 && pinky.isInPaintSpace)
        {

            ChangeAnimationState(I_N);
        }
        else if (moveVector.x == 0 && !pinky.isInPaintSpace)
        {
            ChangeAnimationState(I_S);

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
