using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public SpriteMask spriteMask;
    public Texture2D maskTexture;
    [SerializeField] public Collider2D paintSpace;

    // Public property to check if the player is in the paint space
    public bool IsInPaintSpace { get; private set; }



    public bool FullyPaintedObject { get; private set; } = false;

    void Start()
    {

        // Find all PaintableObject scripts at runtime
        PaintableObject[] paintableObjects = FindObjectsOfType<PaintableObject>();

        if (paintableObjects.Length == 0)
        {
            Debug.LogWarning("No PaintableObject scripts found in the scene.");
        }
        else
        {
            foreach (PaintableObject obj in paintableObjects)
            {
                Debug.Log("PaintableObject found: " + obj.gameObject.name);
                // You can perform any additional operations with each PaintableObject here
            }
        }

        spriteMask = GetComponent<SpriteMask>();
        if (spriteMask != null)
        {
            maskTexture = spriteMask.sprite.texture;
        }
        else
        {
            Debug.LogError("SpriteMask component not found!");
        }

        // Initialize IsInPaintSpace to false
        IsInPaintSpace = false;
        //Debug.Log("PO-" + IsInPaintSpace);

    }
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Entered Trigger: " + other.gameObject.name);

        // Check if the entering collider is either the player or the paintSpace
        if (other.CompareTag("Player") || other == paintSpace)
        {
            // Set IsInPaintSpace to true when the player enters the paintSpace collider
            IsInPaintSpace = true;
            //Debug.Log("PO-" + IsInPaintSpace);
        }
    }



    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Exited Trigger: " + other.gameObject.name);

        // Check if the entering collider is either the player or the paintSpace
        if (other.CompareTag("Player") || other == paintSpace)
        {
            // Set IsInPaintSpace to false when the player exits the paintSpace collider
            IsInPaintSpace = false;
            //Debug.Log("PO-" + IsInPaintSpace);
        }
    }



    public bool IsAimInsideSpriteMask(Vector2 aimPos)
    {
        if (maskTexture == null)
        {
            Debug.LogError("Mask texture not found!");
            return false;
        }

        // Convert the world point to local space
        Vector2 localPoint = transform.InverseTransformPoint(aimPos);

        // Convert local point to texture coordinates
        float textureX = (localPoint.x + spriteMask.sprite.bounds.size.x / 2) / spriteMask.sprite.bounds.size.x;
        float textureY = (localPoint.y + spriteMask.sprite.bounds.size.y / 2) / spriteMask.sprite.bounds.size.y;

        // Convert texture coordinates to pixel coordinates
        int pixelX = Mathf.FloorToInt(textureX * maskTexture.width);
        int pixelY = Mathf.FloorToInt(textureY * maskTexture.height);

        // Check if the pixel is within the mask's bounds
        if (pixelX >= 0 && pixelX < maskTexture.width && pixelY >= 0 && pixelY < maskTexture.height)
        {
            // Check if the alpha value of the pixel is above a certain threshold (considered as inside the mask)
            Color pixelColor = maskTexture.GetPixel(pixelX, pixelY);
            float alphaThreshold = 0.1f; // Adjust the threshold to consider only colored pixels
            bool isInside = pixelColor.a > alphaThreshold;

            //Debug.Log("Is Aim Inside Sprite Mask: " + isInside); 

            return isInside;
        }

        // If the pixel is outside the mask's bounds, consider it outside the sprite mask
        //Debug.Log("Aim Position Outside Sprite Mask Bounds"); 
        return false;
    }

}