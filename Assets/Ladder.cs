using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private P_Inky pinky;

    private void Start()
    {
        pinky = FindObjectOfType<P_Inky>();
        if (pinky == null)
        {
            Debug.Log("No inky");
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (pinky != null)
        {
        pinky.OnLadder = true;
        Debug.Log("Inky entered Ladder");
        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (pinky != null)
            {
            pinky.OnLadder = false;
            Debug.Log("Inky exit Ladder");
            }
    }
}
