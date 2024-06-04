using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ 
    public float gravityScale = 1f;
    public Animator animator;
    private P_Stats pstats;
    private Rigidbody2D rb;
    [SerializeField] private float Damage;
    void Start()
    {
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("PlayerMovement component not found");
        }
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    public void Launch()
    {
        // Add any additional launch behavior if needed
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.gravityScale = 0;
            animator.SetTrigger("Hit");
            // Add any additional hit behavior if needed
        }
       if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb.gravityScale = 0;
            animator.SetTrigger("Hit");
            Attack();
        }
    }
    void Attack()
    {
        pstats.hp -= Damage;
        pstats.Damage();
       
    }
    void Destroy()
    {
        Destroy(gameObject); // Destroy the GameObject containing this script
    }
}