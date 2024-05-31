using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mozzi_resi : MonoBehaviour
{
    [SerializeField] GameObject wall;

    [SerializeField] GameObject hands;
    [SerializeField] GameObject head;

    private PaintableObject wallPaintableObject;

   
    [SerializeField] private Animator handsAnimator;

    private bool doTheThis = false;
    public bool mozzyready = false;

    private string currentState;
    const string mozzi_hands = "mozzi_hands";

    // Start is called before the first frame update
    void Start()
    {
        wallPaintableObject = wall.GetComponent<PaintableObject>();
   
        
        handsAnimator = GetComponent<Animator>();
       // headAnimator = GetComponentInChildren<Animator>();
       

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the wall has been fully bombed
        if (wallPaintableObject != null && wallPaintableObject.fullyBombed)
        {
            Debug.Log("muren bombed!");

            if (!doTheThis)
            {
                doTheThis = true;

                // Activate the tail GameObject
                
                ChangehandsAnimationState(mozzi_hands);

            }
        }
    }
    void startHead()
    {
        
        head.SetActive(true);
        Debug.Log("head up");
        mozzyready = true;
    }

    
   
    void ChangehandsAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }


        //play animation
        handsAnimator.Play(newState);

        currentState = newState;
        //Debug.Log(newState);
    }
    
}
