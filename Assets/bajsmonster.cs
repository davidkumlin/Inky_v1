using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bajsmonster : MonoBehaviour
{

    [SerializeField] private P_Inky pinky;
    public Rigidbody2D unitRb;
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private string currentState;
    [SerializeField] private CircleCollider2D mainCollider;




    //Animation states

    const string bajsmonster_atk = "bajsmonster_atk";
    //const string goblin_atk = "goblin_atk";
    //const string 3 = "3";
    //const string 4 = "4";
    //const string 5 = "5";

    //bools
    private float resettime = 2f;
    private bool isAtk;
    private bool hasAtk;
    private bool atk = false;
    public bool OnWall { get; private set; } = false;
    // Start is called before the first frame update
    private void Awake()
    {
        pinky = FindObjectOfType<P_Inky>();
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

    }// Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            {
                Debug.Log("Start attack sequence");
                // If player collides with main collider
                Attack();

            }
        }

        void Attack()
        {
            if (!hasAtk)
            {
                ChangeAnimationState(bajsmonster_atk);
                hasAtk = true;
                atk = true; // Set atk to true to enable damage
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
    }
   
}
