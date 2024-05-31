using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratman : MonoBehaviour
{
    [SerializeField] GameObject wall;
    private PaintableObject wallPaintableObject;
    private bool doTheThis = false;
    [SerializeField] private P_Inky pinky;
    private Animator Animator;
    private string currentState;

    const string ratman_dice = "ratman_dice";

    void Start()
    {
        Animator = GetComponent<Animator>();
        // Get the PaintableObject component attached to the wall GameObject
        wallPaintableObject = wall.GetComponent<PaintableObject>();
    }

    // Update is called once per frame
    void Update()
    {// Check if the wall has been fully bombed
        if (wallPaintableObject != null && wallPaintableObject.fullyBombed)
        {
            Debug.Log("trollsten bombed");


            if (!doTheThis)
            {
                ChangeAnimationState(ratman_dice);
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
        Animator.Play(newState);

        currentState = newState;
        //Debug.Log(newState);
    }

}
