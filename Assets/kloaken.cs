using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kloaken : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [SerializeField] GameObject rewardSoda;
    private PaintableObject wallPaintableObject;
    private bool doTheThis = false;
    [SerializeField] private P_Inky pinky;
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
            Debug.Log("kloaken!");

            
            if (!doTheThis)
            {
                doTheThis = true;
                activateReward();
            }
        }
    }
    void activateReward()
    {
        rewardSoda.SetActive(true);
    }
}
