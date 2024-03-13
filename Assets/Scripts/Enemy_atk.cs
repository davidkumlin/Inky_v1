using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_atk : MonoBehaviour
{
    [SerializeField] Transform player;
    PlayerMovement playref = null;
    private float DistanceToPlayer; // how far away is the player
    public float maxDamageDistance = 5f; // Maximum distance for maximum damage
    public float minDamageDistance = 10f; // Minimum distance for minimum damage
    public float maxDamage = 100f; // Maximum damage to deal
    public float minDamage = -10f; // Minimum damage to deal
    // Start is called before the first frame update
    void Start()
    {
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
        
    }

    void Slash()
    {
      //  Debug.Log("slash");
    }

    void Sting()
    {
        Debug.Log("Stinged!");

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        float lerpFactor = Mathf.InverseLerp(maxDamageDistance, minDamageDistance, DistanceToPlayer);
        float damage = Mathf.Lerp(maxDamage, minDamage, lerpFactor);
        playref.hp -= damage;

    }
}
