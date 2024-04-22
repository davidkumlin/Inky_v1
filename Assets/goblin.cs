using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblin : MonoBehaviour
{
    [SerializeField] private P_Stats pstats;
    [SerializeField] private P_Inky pinky;
    public Rigidbody2D unitRb;
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private string currentState;
    [SerializeField] private BoxCollider2D Collider2D;
    
    
    [SerializeField] private float hp;
    [SerializeField] private float Damage;

    public bool changeDirection = false;
    private float DistanceToPlayer;
    //Animation states

    const string goblin_idle= "goblin_idle";
    const string goblin_atk = "goblin_atk";
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
        resetATK();
    }

    

    private void Detection()
    {
        Vector2 directionToPlayer = pinky.transform.position - transform.position;
        DistanceToPlayer = directionToPlayer.magnitude;
        if (!isAtk && !hasAtk)
        {
            //Debug.Log(DistanceToPlayer);
            if (DistanceToPlayer < 5 && !OnWall)
            {
                StartATK();
            }
            
        }
    }
    void StartATK()
    {
        isAtk = true;
        ChangeAnimationState(goblin_atk);
        Debug.Log("StartATK");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger collider overlaps with the player's collider
        if (other.gameObject.CompareTag("Player"))
        {
            // Call the Attack function to deal damage to the player
            Attack();
        }

    }
    private void Attack()
    {
        if (atk)
        {
        pstats.hp -= Damage;
            atk = true;
        }
        

    }
    private void atkCheck()
    {
        hasAtk = true;
        ChangeAnimationState(goblin_idle);
    }
    private void resetATK()
    {
        

        if (hasAtk)
        {
           resettime -= Time.deltaTime;
            Debug.Log(resettime);

                if (resettime < 0)
                {
                isAtk = false;
                Debug.Log("reset2");
                hasAtk = false;
                resettime = 3f;
                atk = false;
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
        //Debug.Log(newState);
    }


    void Death()
    {
        
        hp = 0F;

        Destroy(gameObject);

    }
}

