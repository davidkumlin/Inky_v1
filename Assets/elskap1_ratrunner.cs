using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elskap1_ratrunner : MonoBehaviour
{
    [SerializeField] GameObject wall;
    private PaintableObject wallPaintableObject;
    private bool doTheThis = false;
    [SerializeField] private P_Inky pinky;
    private float DistanceToPlayer;

    //ratstuff
    [SerializeField] private float speed;
    public Rigidbody2D unitRb;

    void Start()
    {
        // Get the PaintableObject component attached to the wall GameObject
        wallPaintableObject = wall.GetComponent<PaintableObject>();
    }

    // Update is called once per frame
    void Update()
    {// Check if the wall has been fully bombed
        if (wallPaintableObject != null && wallPaintableObject.fullyBombed)
        {
            Debug.Log("skogsmuren bombed!");

            Detection();
            if (!doTheThis)
            {
                doTheThis = true;
                Move();
            }
        }
    }
    private void Move()
    {

        // Calculate movement vector
        float horizontalMovement = speed;


        // Apply movement to the Rigidbody
        Vector2 movement = new Vector2(horizontalMovement, unitRb.velocity.y);
        unitRb.velocity = movement;
    }
    private void Detection()
    {
        Vector2 directionToPlayer = pinky.transform.position - transform.position;
        DistanceToPlayer = directionToPlayer.magnitude;
        if (doTheThis)
        {
           
            if (DistanceToPlayer > 100)
            {
                Destroy(gameObject);
            }
        }
    }
}
