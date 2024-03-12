using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    //Bee Stats
    private float speed =7f;
    private float orginalSpeed;
    private float health = 50f;
    private float damage;
    public Rigidbody2D unitRb;
    [SerializeField] Transform player;
    PlayerMovement playref = null;
    private float DistanceToPlayer; // how far away is the player
    [SerializeField] private float ChaseDistance = 20f;
    [SerializeField] private float resetDistance = 15f;

    

    //Reactions
    private bool ChaseBool;
    private bool Confusedbool;
    
    public bool OnWall { get; private set; } = false;
    private bool shouldIgnoreCollisions = false;
    bool isAtk = false;
    //patrollpath
    public Transform unitPos;
    [SerializeField] GameObject unitPath;
    private En_patrolpath patrolpath;
    protected int currentIndex = 0;

    [SerializeField] SpriteRenderer gfx;
    public Animator gfx_ani;
    private string currentState;
    private string LastState;
    //animation states
    const string Bee_Idle_W = "Bee_Idle_W";
    const string Bee_Idle_S = "Bee_Idle_S";
    const string Bee_Idle_N = "Bee_Idle_N";
    const string Bee_Atk_W = "Bee_Atk_W";
    const string Bee_Atk_S = "Bee_Atk_S";
    const string Bee_Atk_N = "Bee_Atk_N";

    //Reactions
    [SerializeField] private SpriteRenderer reactionFX; // Reaction sprite
    [SerializeField] private Sprite react_chase;
    [SerializeField] private Sprite react_confused;
    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        orginalSpeed = speed; // Store the original speed
       
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playref = playerObject.GetComponent<PlayerMovement>();
            if (playref == null)
            {
                Debug.LogError("PlayerMovement component not found on the player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
        unitRb = GetComponent<Rigidbody2D>();
        if (unitRb != null)
            Debug.Log(unitRb.name + " Rigidbody2D found");
        else
            Debug.Log("Rigidbody2D not found on " + name);

        // Get the En_patrolpath component from the unitPath GameObject
        if (unitPath != null)
            patrolpath = unitPath.GetComponent<En_patrolpath>();
    }

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("PM" + OnWall);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionToPlayer = playref.transform.position - transform.position;
        DistanceToPlayer = directionToPlayer.magnitude; // Calculate distance to player
        //Debug.Log(DistanceToPlayer); // Log the distance for debugging purposes
        Debug.DrawRay(transform.position, directionToPlayer.normalized * DistanceToPlayer, Color.red); // Draw a debug ray
       
        // Check if the distance to the player is within the chase distance
        if (DistanceToPlayer < ChaseDistance)
        {
            if (!Confusedbool)
            {
                Chase();
            }


        }
        else
        {
            // If the player is out of range, continue patrolling
            Patrol();

        }
    }

    private void FixedUpdate()
    {
        // Update sprite based on movement direction
        Animation();
        UpdateReaction();
    }
    private void Animation()
    {
        // Get the velocity of the unit
        Vector2 velocity = unitRb.velocity;

        // Determine the magnitude of the X and Y components
        float xMagnitude = Mathf.Abs(velocity.x);
        float yMagnitude = Mathf.Abs(velocity.y);
        if (!isAtk)
        {
        // Check if the movement is primarily in the north or south direction
        if (yMagnitude > xMagnitude)
        {
            if (velocity.y > 0)
            {
                LastState = "N";
                ChangeAnimationState(Bee_Idle_N); // North
                gfx.flipX = false;
            }
            else 
            {
                LastState = "S";
                ChangeAnimationState(Bee_Idle_S); // South
                gfx.flipX = false;
            }
        }
        // Otherwise, check if the movement is primarily in the east or west direction
        else if (xMagnitude > yMagnitude)
        {
            if (velocity.x > 0)
            {
                LastState = "E";
                ChangeAnimationState(Bee_Idle_W); // East
                                                  // Flip the sprite on the X-axis
                gfx.flipX = true;
            }
            else
            {
                LastState = "W";
                ChangeAnimationState(Bee_Idle_W); // West
                                                  // Ensure the sprite is not flipped when facing west
                gfx.flipX = false;
            }
        }
        }

    }
    private void UpdateReaction()
    {
        // Set the reaction sprite
        
        reactionFX.sprite = null;

        // Assign sprite based on conditions
        if (ChaseBool)
        {
            reactionFX.sprite = react_chase;
            Debug.Log("Updating reaction to chase");
        }

        else if (Confusedbool)
        {
            reactionFX.sprite = react_confused;
            Debug.Log("Updating reaction to confused");
        }
        else
        {
            reactionFX.sprite = null; // No reaction sprite if not chasing
        }


    }
    void Patrol()
    {
        Confusedbool = false;
        speed = orginalSpeed;

        if (patrolpath == null)
        {
            Debug.LogError("En_patrolpath component not found on unitPath GameObject.");
            return;
        }

        // Get the next patrol point based on the current index
        En_patrolpath.PathPoint nextPoint = patrolpath.GetNextPathPoint(currentIndex);

        // Calculate direction towards the next patrol point
        Vector2 direction = (nextPoint.Position - (Vector2)transform.position).normalized;

        // Move the enemy towards the next patrol point
        unitRb.velocity = direction * speed;

        // Check if the enemy is very close to the next patrol point
        if (Vector2.Distance(transform.position, nextPoint.Position) < 1f)
        {
            // Update the current index to move to the next point
            currentIndex = nextPoint.Index;
        }
    }
    void Chase()
    {
        Debug.Log("BEE chasing!");

        // Your existing implementation of Chase method
        if (playref != null)
        {
            // Calculate the direction towards the player
            Vector2 directionToPlayer = (playref.transform.position - transform.position).normalized;

            // Move the bee towards the player using velocity
            unitRb.velocity = directionToPlayer * speed;
            if (DistanceToPlayer < 5)
            {
                if (!OnWall)
                {
                    isAtk = true;
                    Attack();
                    Debug.Log("attack!");
                }

            }
            if (OnWall)
            {
                Confused();
            }
        }



    }

    
    void Attack()
    {

        // Check if not already attacking
        if (isAtk)
        {
            MoveTowardsScript moveScript = transform.parent.GetComponentInChildren<MoveTowardsScript>();
            if (moveScript != null)
            {
                // Successfully found the component, now you can modify its properties
                moveScript.speed = 0;
                moveScript.cap = 0;
            }
            else
            {
                // Log an error message if the component is not found
                Debug.LogError("MoveTowardsScript component not found on the GameObject.");
            }


            if (LastState == "N")
            {
                ChangeAnimationState(Bee_Atk_W);
            }
            else if (LastState == "S")
            {
                ChangeAnimationState(Bee_Atk_S);
            }
            else if (LastState == "E")
            {
                ChangeAnimationState(Bee_Atk_W);
                gfx.flipX = true;

            }
            else if (LastState == "W")
            {
                ChangeAnimationState(Bee_Atk_W);

            }
          
            Death();
            moveScript.dead = true;
        }
        
        

        
    }
    
    void Confused()
    {
        speed = 4;
        ChaseBool = false;
        Confusedbool = true;
        // Get the next patrol point based on the current index
        En_patrolpath.PathPoint nextPoint = patrolpath.GetNextPathPoint(currentIndex);

        // Calculate direction towards the next patrol point
        Vector2 direction = (nextPoint.Position - (Vector2)transform.position).normalized;

        // Move the enemy towards the next patrol point
        unitRb.velocity = direction * speed;

        // Check if the enemy is very close to the next patrol point
        if (Vector2.Distance(transform.position, nextPoint.Position) < 1f)
        {
            // Update the current index to move to the next point
            currentIndex = nextPoint.Index;
        }
        if (DistanceToPlayer > resetDistance)
        {
            Debug.Log("Return to patrol");
            ReturnToPatrol();
        }


    }
    void Death()
    {
        speed = 0f;
        health = 0F;
        
        Destroy(gameObject);

    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        //play animation
        gfx_ani.Play(newState);
        currentState = newState;
        //Debug.Log(newState);
        

    }
    protected virtual void ReturnToPatrol()
    {

        ToggleCooldown();

    }

    private IEnumerator ToggleCooldown()
    {
        yield return new WaitForSeconds(2f);
    }
}
