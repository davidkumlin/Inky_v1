using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SopJumpa : MonoBehaviour
{
    [SerializeField] GameObject wall;
    private PaintableObject wallPaintableObject;
    private bool doTheThis = false;
    [SerializeField] private float moveForce;
    public Rigidbody2D unitRb;

    private Animator animator;
    private string currentState;
    //Animation states
    //idle
    const string idle = "idle";
    const string Sopjumpa_getup = "Sopjumpa_getup";

    // Start is called before the first frame update
    void Start()
    {
        // Get the PaintableObject component attached to the wall GameObject
        wallPaintableObject = wall.GetComponent<PaintableObject>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {// Check if the wall has been fully bombed
        if (wallPaintableObject != null && wallPaintableObject.fullyBombed)
        {
            Debug.Log("Megakakel bombed!");

            if (!doTheThis)
            {
                ChangeAnimationState(Sopjumpa_getup);
                doTheThis = true;

            }
        }
    }
    private void Move()
    {
        {
            // Apply force in the left direction
            unitRb.AddForce(Vector2.right * moveForce, ForceMode2D.Force);
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
