using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_atk : MonoBehaviour
{
    [SerializeField] Transform player;
    PlayerMovement playref = null;
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

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playref = playerObject.GetComponent<PlayerMovement>();
            if (playref == null)
            {
                Debug.LogError("PlayerMovement component not found on the player object.");
            }
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
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float lerpFactor = Mathf.InverseLerp(maxDamageDistance, minDamageDistance, DistanceToPlayer);
        float damage = Mathf.Lerp(maxDamage, minDamage, lerpFactor);
        if (brorC.inBox)
        {
        playref.hp -= damage;
        Debug.Log("Cutt" + damage);
        }
    }

    void Sting()
    {
        Debug.Log("Stinged!");

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        float lerpFactor = Mathf.InverseLerp(maxDamageDistance, minDamageDistance, DistanceToPlayer);
        float damage = Mathf.Lerp(maxDamage, minDamage, lerpFactor);
        playref.hp -= damage;

        Debug.Log("Sting" + damage);
    }


    void ResetArm()
    {
        
        
        shouldResetAtk = true;
        
    }

}
