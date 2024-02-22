using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float speed;
    protected float originalSpeed;
    protected float health;
    PlayerMovement playref = null;
    [SerializeField] private GameObject inky;
    [SerializeField] Transform player;
    public Rigidbody2D unitRb;

    [SerializeField] GameObject unitPath;
    protected En_patrolpath patrolpath;
    protected int currentIndex = 0; // Define currentIndex as a member variable

    //types of enemies
    protected bool PatrolUnit;
    protected bool GuardUnit;

    private float DistanceToPlayer; // how far away is the player
    [SerializeField] private float ChaseDistance = 10f;

    protected bool OnWall;
    protected bool Chasebool;
    protected bool Confusedbool;

    private bool shouldIgnoreCollisions = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;

        originalSpeed = speed; // Store the original speed

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

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        Debug.Log("ENemy " + OnWall);

    }
    protected virtual void Patrol()
    {
        Confusedbool = false;
        speed = originalSpeed;

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
    protected virtual void Chase()
    {
        //Debug.Log("EN_chasing");

        Chasebool = true;
        // Your existing implementation of Chase method
        if (playref != null)
        {
            // Calculate the direction towards the player
            Vector2 directionToPlayer = (playref.transform.position - transform.position).normalized;

            // Move the bee towards the player using velocity
            unitRb.velocity = directionToPlayer * speed;
            if (DistanceToPlayer < 3)
            {
                if (!OnWall)
                {
                    Attack();
                }
                
            }
            else if (OnWall)
            {
                Confused();
            }
        }



    }

    protected virtual void Attack()
    {

    }

    protected virtual void Confused()
    {
        Chasebool = false;
        Confusedbool = true;
        Vector2 directionToPlayer = (playref.transform.position - transform.position).normalized;
        unitRb.velocity = -directionToPlayer * speed;
        if (DistanceToPlayer > ChaseDistance)
        {
           ToggleCooldown();
           Patrol();
        }


    }

    private IEnumerator ToggleCooldown()
    {
        yield return new WaitForSeconds(2f);
    }
}


