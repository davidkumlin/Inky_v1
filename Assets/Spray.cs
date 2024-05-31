using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    private P_Inky pinky;
    private inky_animation inkyani;

    [SerializeField] private Transform NorthHand;
    [SerializeField] private Transform WestHand;
    [SerializeField] private Transform EastHand;

    [SerializeField] private Transform aimpos;

    [SerializeField] private LineRenderer sprayBeam;

    private Transform ActiveHand;

    // Start is called before the first frame update
    void Start()
    {
        pinky = FindObjectOfType<P_Inky>();
        if(pinky == null)
        {
            Debug.Log("pinky not found bitch");
        }
        inkyani = FindObjectOfType<inky_animation>();
        // Set the initial position of the spray emitter to NorthHand

    }



    // Update is called once per frame
    void Update()
    {
        WhatHand();

        if (pinky.IsDrawing && pinky.aimInsideMask &&!pinky.OnWall)
        {
            Debug.Log("pssch");
            sprayBeam.enabled = true;
           // sprayBeam.transform.position = ActiveHand.position;
            // Calculate the direction towards the aimpos
            //Vector3 direction = (aimpos.position - ActiveHand.position).normalized;

            if (ActiveHand != null)
            {
                sprayBeam.SetPosition(0, ActiveHand.position);
                sprayBeam.SetPosition(1, aimpos.position);
            }
            
        }
        else if (pinky.OnWall)
        {
            // Disable the LineRenderer when not drawing
            sprayBeam.enabled = false;
        }
        else
        {
            sprayBeam.enabled = false;
        }


    }

    private void WhatHand()
    {
        if (inkyani != null)
        {
            // Access the _IK variable from the inky_animation script
            GameObject activeIK = inkyani._IK;

            if (activeIK == inkyani.IK_N)
            {
                ActiveHand = NorthHand;
            }
            else if (activeIK == inkyani.IK_RB)
            {
                ActiveHand = EastHand;
            }
            else if (activeIK == inkyani.IK_LB)
            {
                ActiveHand = WestHand;
            }
            else
            {
                ActiveHand = null;
                Debug.Log("No active hand detected.");
            }
        }
        else
        {
            Debug.LogWarning("Inky animation script reference not set.");
        }
    }

}
