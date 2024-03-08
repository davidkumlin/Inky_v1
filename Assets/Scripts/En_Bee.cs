using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class En_Bee : Enemy
{
    [SerializeField] SpriteRenderer gfx;
    [SerializeField] Sprite N_sprite; // North
    [SerializeField] Sprite E_sprite; // East
    [SerializeField] Sprite S_sprite; // South
    [SerializeField] Sprite W_sprite; // West

    [SerializeField] SpriteRenderer reactionFX; // Reaction sprite
    [SerializeField] Sprite react_chase;
    [SerializeField] Sprite react_confused;
    public Transform unitPos;
    

    protected override void Start()
    {

        speed = 7;
        originalSpeed = speed; // Store the original speed
        health = 50;
        base.Start();
        PatrolUnit = true;
        GuardUnit = false;
        Chasebool = false;
        Confusedbool = false;
        ChaseCall = false;
        if (PatrolUnit)
        {
            Debug.Log("Bee is Patrolunit");
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(speed);
        // Update sprite based on movement direction
        UpdateSprite();
        UpdateReaction();
    }

    protected override void Chase()
    {
        // Call the base Chase method from the Enemy class
        base.Chase();
        speed = 11;
        ChaseCall = true;
        Debug.Log(ChaseCall);

    }
    protected override void Attack()
    {
        base.Attack();
        Debug.Log("Bee_Attack");
    }
    protected override void Confused()
    {
        base.Confused();
        Chasebool = false;
        
    }
    private void UpdateReaction()
    {
        // Set the reaction sprite
        Sprite fxSprite = null;
        reactionFX.sprite = fxSprite;

        // Assign sprite based on conditions
        if (Chasebool)
        {
            fxSprite = react_chase;
            Debug.Log("Updating reaction to chase");
        }
       
        else if (Confusedbool)
        {
            fxSprite = react_confused;
            Debug.Log("Updating reaction to confused");
        }
        else
        {
            fxSprite = null; // No reaction sprite if not chasing
        }

       
    }

    private void UpdateSprite()
    {
        // Get the velocity of the unit
        Vector2 velocity = unitRb.velocity;

        // Determine the magnitude of the X and Y components
        float xMagnitude = Mathf.Abs(velocity.x);
        float yMagnitude = Mathf.Abs(velocity.y);

        // Set the default sprite
        Sprite newSprite = null;

        // Check if the movement is primarily in the north or south direction
        if (yMagnitude > xMagnitude)
        {
            if (velocity.y > 0)
                newSprite = N_sprite; // North
            else
                newSprite = S_sprite; // South
        }
        // Otherwise, check if the movement is primarily in the east or west direction
        else if (xMagnitude > yMagnitude)
        {
            if (velocity.x > 0)
                newSprite = E_sprite; // East
            else
                newSprite = W_sprite; // West
        }

        // Set the sprite
        if (newSprite != null)
        {
            // Assuming your En_Bee class has a SpriteRenderer component
            gfx.sprite = newSprite;
        }
    }
}