using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_body : MonoBehaviour
{
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private SpriteRenderer spriteRenderer;

    public Sprite idleSprite; // Assign your idle sprite in the Inspector
    public Sprite northEastSprite; // Assign your north-east facing sprite in the Inspector
    public Sprite southEastSprite; // Assign your south-east facing sprite in the Inspector
    public Sprite southWestSprite; // Assign your south-west facing sprite in the Inspector
    public Sprite northWestSprite; // Assign your north-west facing sprite in the Inspector

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>(); // Use GetComponentInParent to find the script on the parent
        spriteRenderer = GetComponent<SpriteRenderer>(); // Assuming the SpriteRenderer is on the same GameObject as this script
    }


    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (playerMovement != null)
        {
            Vector2 moveVector = playerMovement.moveVector; // Access moveVector from PlayerMovement script

           // Debug.Log("Move Vector: " + moveVector);

            if (moveVector.x > 0 && moveVector.y > 0)
            {
                spriteRenderer.sprite = northEastSprite;
            }
            else if (moveVector.x > 0 && moveVector.y < 0)
            {
                spriteRenderer.sprite = southEastSprite;
            }
            else if (moveVector.x < 0 && moveVector.y < 0)
            {
                spriteRenderer.sprite = southWestSprite;
            }
            else if (moveVector.x < 0 && moveVector.y > 0)
            {
                spriteRenderer.sprite = northWestSprite;
            }
            else
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
        else
        {
            Debug.LogError("PlayerMovement script not found!");
        }
    }
}
