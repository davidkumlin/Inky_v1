using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borgbeer : MonoBehaviour

{
    [SerializeField] P_Stats pstats; // Reference to the PlayerMovement script
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private string currentState;
    const string borgbeer_actived = "borgbeer_actived";
    const string Out = "Out";
    [SerializeField] private float Healpoints;

    // Start is called before the first frame update
    void Start()
    {
        pstats = FindObjectOfType<P_Stats>(); // Find the PlayerMovement component in the scene
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (pstats == null)
        {
            Debug.LogError("PlayerMovement component not found");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        ChangeAnimationState(borgbeer_actived);
    }

    void Gulped()
    {
        Debug.Log("gulp!");
        if (pstats != null)
        {
            P_Stats.hp += Healpoints;
        }
        
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        //play animation
        animator.Play(newState);

        currentState = newState;
        Debug.Log(newState);
    }
}
