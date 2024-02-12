using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public PolygonCollider2D OnWallArea;

    

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

    }


    void OnDrawGizmosSelected()
    {
        if (OnWallArea != null)
        {
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // Set the color of the gizmo lines
            Color gizmoColor = Color.red;

            // Draw the edges of the OnWallArea collider with thicker lines
            for (int pathIndex = 0; pathIndex < OnWallArea.pathCount; pathIndex++)
            {
                Vector2[] path = OnWallArea.GetPath(pathIndex);
                for (int i = 0; i < path.Length - 1; i++)
                {
                    Vector3 startPoint = new Vector3(path[i].x, path[i].y, 0f);
                    Vector3 endPoint = new Vector3(path[i + 1].x, path[i + 1].y, 0f);
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawLine(startPoint, endPoint);
                }
                // Draw a line between the last and first points to close the path
                Vector3 lastPoint = new Vector3(path[path.Length - 1].x, path[path.Length - 1].y, 0f);
                Vector3 firstPoint = new Vector3(path[0].x, path[0].y, 0f);
                Gizmos.DrawLine(lastPoint, firstPoint);
            }

            Gizmos.matrix = oldMatrix;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Entered Trigger: " + other.gameObject.name);

        // Check if the entering collider is either the player or the paintSpace
        if (other.CompareTag("Player") || other == paintSpace)
        {
            // Set IsInPaintSpace to true when the player enters the paintSpace collider
            IsInPaintSpace = true;
            //Debug.Log("PO-paintspace" + IsInPaintSpace);           
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
            //Debug.Log("PO-paintspace" + IsInPaintSpace);
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
                edgecollider.edgeRadius = .5f;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }
    }



}