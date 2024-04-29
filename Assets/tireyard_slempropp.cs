using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tireyard_slempropp : MonoBehaviour
{
    [SerializeField] GameObject wall;
    private PaintableObject wallPaintableObject;
    private bool doTheThis = false;
   
    private Animator animator;
    private string currentState;
    //Animation states
    //idle
    const string idle = "idle";
    const string plopp = "plopp";

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
            Debug.Log("tireyard bombed!");

            if (!doTheThis)
            {
                ChangeAnimationState(plopp);
                doTheThis = true;
                
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
}
