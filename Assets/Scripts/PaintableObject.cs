using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    //Version 12/02 V1
    public SpriteMask spriteMask;
    public Texture2D maskTexture;
    [SerializeField] public Collider2D paintSpace;
    [SerializeField] private float PaintHP;
    public int originalPaintHP;
    public UnityEvent OnFullyBombed;
    [SerializeField] public bool fullyBombed = false;
    public bool pointsAdded = false;

    private GameObject self;





    // Public property to check if the player is in the paint space
    public bool IsInPaintSpace { get; private set; }

    // Public property to get the bounds of the sprite mask
    public Bounds SpriteMaskBounds { get; private set; }

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
                //Debug.Log("PO-PaintableObject found: " + obj.gameObject.name);
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
        PaintHP = originalPaintHP;

    }





    public void TakeDamage(float lineDamage)
    {
       // Debug.Log(PaintHP);
        PaintHP -= Line.lineDamage; // Subtract the line width (damage) from PaintHP
        if (PaintHP <= 0) // Check if PaintHP is less than or equal to zero
        {
            PaintHP = 0; // Ensure HP doesn't go negative
            fullyBombed = true; // Set fullyBombed to true
            //Debug.Log("fullyBombed!");
            if (!pointsAdded) // Check if points haven't been added yet
            {
                pointsAdded = true; // Set pointsAdded flag to true
                OnFullyBombed.Invoke(); // Invoke the event to notify HUD
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered Trigger: " + other.gameObject.name);

        // Check if the entering collider is either the player or the paintSpace
        if (other.CompareTag("Player") || other == paintSpace)
        {
            // Set IsInPaintSpace to true when the player enters the paintSpace collider
            IsInPaintSpace = true;
            Debug.Log("PO-paintspace" + IsInPaintSpace);
            other.gameObject.GetComponent<P_Inky>().paintableObject = this;
            
            
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
           //Debug.Log("PO-paintspace NOT" + IsInPaintSpace);

           
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

        // Calculate the bounds of the sprite mask in local space
        Bounds spriteMaskLocalBounds = spriteMask.sprite.bounds;

        // Convert local point to texture coordinates
        float textureX = (localPoint.x + spriteMaskLocalBounds.size.x / 2) / spriteMaskLocalBounds.size.x;
        float textureY = (localPoint.y + spriteMaskLocalBounds.size.y / 2) / spriteMaskLocalBounds.size.y;

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

            // If the pixel is inside the mask, update the bounds
            if (isInside)
            {
                // Define the bounds based on the condition
                spriteMaskLocalBounds = new Bounds(localPoint, Vector3.zero);
            }

            // Debugging whether the aim is inside the mask
            //Debug.Log("PO-Is aim inside sprite mask? " + isInside);
            //gör denna till hoppa av o på väggjäveln!!!!
            return isInside;
        }

        return false;
    }

    [ContextMenu("Create edges")]
    public void createEdgeCollider()
    {
        if (TryGetComponent<PolygonCollider2D>(out var polygon))
        {
            for (int i = 0; i < polygon.pathCount; i++)
            {
                var edgecollider = gameObject.AddComponent<EdgeCollider2D>();
                var pointList= new List<Vector2>(polygon.GetPath(i));
                pointList.Add(pointList[0]);

                edgecollider.SetPoints(pointList);
                edgecollider.edgeRadius = .2f;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }
    }



}