using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    private P_Inky pinky;
    private inky_animation inkyani;

    [SerializeField] private Transform NorthHand;
    [SerializeField] private Transform WesthHand;
    [SerializeField] private Transform EastHand;

    [SerializeField] private Transform aimpos;

    [SerializeField] private ParticleSystem sprayfx;



    // Start is called before the first frame update
    void Start()
    {
        pinky = GetComponent<P_Inky>();
        inkyani = GetComponent<inky_animation>();
        // Set the initial position of the spray emitter to NorthHand
        
    }



// Update is called once per frame
void Update()
    {
        sprayfx.transform.position = NorthHand.position;
        // Calculate the direction towards the aimpos
        Vector3 direction = (aimpos.position - aimpos.position).normalized;

        // Rotate the emitter to point towards aimpos
        sprayfx.transform.rotation = Quaternion.LookRotation(direction);
    }
}
