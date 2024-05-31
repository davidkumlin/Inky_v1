using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staevet : MonoBehaviour
{
    [SerializeField] GameObject wall;
    private PaintableObject wallPaintableObject;
    private bool doTheThis = false;
    private Animator Animator;
    private bool onground;
    [SerializeField] GameObject skyblocker;
    [SerializeField] private P_Stats pstats;
    private float Damage = 100f;
    void Start()
    {
        // Get the PaintableObject component attached to the wall GameObject
        wallPaintableObject = wall.GetComponent<PaintableObject>();
        // Get the Animator component attached to the tail GameObject
        Animator = GetComponent<Animator>();
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("P_Inky not found");
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Check if the wall has been fully bombed
        if (wallPaintableObject != null && wallPaintableObject.fullyBombed)
        {
            Debug.Log("skogsmuren bombed!");

            if (!doTheThis)
            {
                

                Destroy(skyblocker);
                doTheThis = true;

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (doTheThis)
        {


            // Check if the collision is with the player
            if (collision.gameObject.CompareTag("Ground"))
            {
                // Play the animation on the tail GameObject
                Animator.SetBool("landed", true);
                onground = true;
            }
           
            if (collision.gameObject.CompareTag("Player") && !onground)
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
       
        pstats.hp -= Damage;
        pstats.Damage();
    }

}