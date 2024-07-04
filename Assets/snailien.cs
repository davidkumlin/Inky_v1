using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snailien : MonoBehaviour
{
    [SerializeField] private P_Stats pstats;
    [SerializeField] private P_Inky pinky;
    public Rigidbody2D unitRb;
    [SerializeField] private BoxCollider2D Collider2D;
    [SerializeField] private float hp;
    [SerializeField] private float Damage;
    [SerializeField] private float moveForce;
    private void Awake()
    {
        pinky = FindObjectOfType<P_Inky>();
        if (pinky == null)
        {
            Debug.LogError("P_Inky not found");
        }
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("PStats not found");
        }

    }
        // Update is called once per frame
        void Update()
    {
        
    }

    private void Move()
    {
        {
            // Apply force in the left direction
            unitRb.AddForce(Vector2.left * moveForce, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
        }

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
        
        yield return new WaitForSeconds(0.5f);
        hasAtk = false;
    }
}
