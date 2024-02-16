using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float speed;
    protected float originalSpeed;
    protected float health;
    PlayerMovement playref = null;
    
    [SerializeField] Transform player;
    public Rigidbody2D unitRb;

    [SerializeField] GameObject unitPath;
    protected En_patrolpath patrolpath;
    protected int currentIndex = 0; // Define currentIndex as a member variable

    //types of enemies
    protected bool PatrolUnit;
    protected bool GuardUnit;

    [SerializeField] private float lineOfSightDistance = 25f; // Maximum distance of line of sight
    [SerializeField] private Color lineOfSightColor = Color.red; // Color of line of sight gizmo

    protected bool OnWall;
    protected bool Chasebool;
    


    // Start is called before the first frame update
    protected virtual void Start()
    {
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
        

        // Check for line of sight if not on wall
        if (!OnWall)
        {
            // Define the direction towards the player
            Vector2 directionToPlayer = playref.transform.position - transform.position;

            // Cast a ray towards the player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, lineOfSightDistance, LayerMask.GetMask("Obstacle"));
            if (hit.collider != null)
            {
                // Check if the hit collider is the "Bee" collider
                if (hit.collider.name == "Bee")
                {
                    // Ignore collisions with the "Bee" collider
                    Physics2D.IgnoreCollision(hit.collider, GetComponent<Collider2D>());
                }

                // Debug the hit point
                Debug.DrawLine(transform.position, hit.point, Color.red);

                // Print the name of the collider hit
                Debug.Log("Hit collider: " + hit.collider.name);
            }
        
            //Debug.Log("Layer mask value: " + LayerMask.GetMask("Obstacle"));
            // Draw the line of sight gizmo
            Debug.DrawLine(transform.position, transform.position + (Vector3)directionToPlayer.normalized * lineOfSightDistance, lineOfSightColor);

            // Check if the ray hits the player
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Check if the player is within range
                if (Vector2.Distance(playref.transform.position, transform.position) < lineOfSightDistance)
                {
                    Chase();
                }
               
            }
            else
            {
                Patrol();
                //Debug.Log("Patroling");
            }
        }
    }
        private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        Debug.Log("EM" + OnWall);

    }
    protected virtual void Patrol()
    {
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
        }


        Attack();
    }

    protected abstract void Attack();
}


