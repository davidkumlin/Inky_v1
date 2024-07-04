using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pokey : MonoBehaviour
{
    [SerializeField] P_Stats pstats; // Reference to the PlayerMovement script
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private string currentState;
    const string pokey_atk = "pokey_atk";
    [SerializeField] private float Damage;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Attack();
        ChangeAnimationState(pokey_atk);
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
        //Debug.Log(newState);
    }
    private bool hasAtk = false;
    private void Attack()
    {
        if (!hasAtk)
        {

            pstats.Damage(Damage);
            ToggleCooldown();
        }
    }
    private IEnumerator ToggleCooldown()
    {

        yield return new WaitForSeconds(0.2f);
        hasAtk = false;
    }
}
