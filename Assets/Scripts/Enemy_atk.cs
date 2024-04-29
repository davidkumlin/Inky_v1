using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_atk : MonoBehaviour
{
    [SerializeField] Transform pinkypos;
    P_Inky pinky = null;
    P_Stats pstats = null;
    private float DistanceToPlayer; // how far away is the player
    public float maxDamageDistance; // Maximum distance for maximum damage
    public float minDamageDistance; // Minimum distance for minimum damage
    public float maxDamage; // Maximum damage to deal
    public float minDamage ; // Minimum damage to deal
    private Brorcolli brorC;
    public bool shouldResetAtk = false;


    void Start()
    {
        brorC = FindObjectOfType<Brorcolli>();
        pinky = FindObjectOfType<P_Inky>();
        if (pinky == null)
        {
            Debug.LogError("P_Inky not found");
        }
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("P_stats not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(shouldResetAtk);
    }

    void Slash()
    {
        Debug.Log("slash");
        float distanceToPlayer = Vector3.Distance(transform.position, pinkypos.position);
        float lerpFactor = Mathf.InverseLerp(maxDamageDistance, minDamageDistance, DistanceToPlayer);
        float damage = Mathf.Lerp(maxDamage, minDamage, lerpFactor);
        if (brorC.inBox)
        {
        pstats.hp -= damage;
        Debug.Log("Cutt" + damage);
        }
    }

    void Sting()
    {
        Debug.Log("Stinged!");

        float distanceToPlayer = Vector3.Distance(transform.position, pinkypos.position);

        float lerpFactor = Mathf.InverseLerp(maxDamageDistance, minDamageDistance, DistanceToPlayer);
        float damage = Mathf.Lerp(maxDamage, minDamage, lerpFactor);
        pstats.hp -= damage;

        Debug.Log("Sting" + damage);
    }


    void ResetArm()
    {
        
        
        shouldResetAtk = true;
        
    }

}
