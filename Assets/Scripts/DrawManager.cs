using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class DrawManager : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab; // Assign your prefab here
    public const float RESOLUTION = .1f;
    private GameObject currentLine;
    private AimMovement aimMovement;
    private PlayerMovement playerMovement;
    //private PaintableObject paintableObject; // Reference to the PaintableObject script
    private List<PaintableObject> paintableObjects = new List<PaintableObject>(); // List to store all PaintableObject instances
    private CustomInput input;

    // Example method call in DrawManager.cs
    public PaintableObject paintableObject;
    //bool isInsideMask = paintableObject.IsAimInsideSpriteMask(aimPosition, spriteMask);
    // Public bool to track if spraying is active
    public bool ActiveSpray { get; private set; } = false;
    public float sDamage;

    void Start()
    {
        aimMovement = FindObjectOfType<AimMovement>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        // paintableObject = FindObjectOfType<PaintableObject>(); // Assign the PaintableObject reference
        // Find all PaintableObject instances in the scene and add them to the list
        PaintableObject[] allPaintableObjects = FindObjectsOfType<PaintableObject>();
        paintableObjects.AddRange(allPaintableObjects);

        input = new CustomInput();
        input.Enable();
        input.Player.Spray.started += OnSprayStarted;
        input.Player.Spray.canceled += OnSprayCanceled;
    }

    private void OnDestroy()
    {
        input.Disable();
    }

    void Update()
    {
        Debug.Log(sDamage);
        if (aimMovement != null && input.Player.Spray.ReadValue<float>() > 0.1f)
        {
            Vector2 aimPos = aimMovement.CurrentAim;
            // Check if the aim position is inside the bounds and outside the sprite mask for any PaintableObject
            bool isInsideAnyObject = paintableObjects.Any(obj => obj != null && obj.IsAimInsideSpriteMask(aimPos));
            // Check if the player is in the paint space
            if (isInsideAnyObject && paintableObjects.Any(obj => obj != null && obj.IsInPaintSpace))
            {
                if (isInsideAnyObject)
                {
                    if (currentLine == null)
                    {
                        // If no current line, create a new one at the current Aim position
                        currentLine = Instantiate(linePrefab, aimPos, Quaternion.identity);

                        // Set the sorting order for the line
                        SetSortingOrder(0, currentLine); // Use sorting order 0 for all lines
                    }

                    // Update the position of the current line
                    currentLine.GetComponent<Line>().SetPosition(aimPos);
                    // Set ActiveSpray to true when spraying starts
                    ActiveSpray = true;
                    SprayDamage();
                }
                else
                {
                    // If the aim position is outside the colored area of the sprite or outside the bounds, finalize the current line
                    FinalizeCurrentLine();
                    ActiveSpray = false;
                }
            }
            else
            {
                // If the "Spray" action is not held down, finalize the current line
                FinalizeCurrentLine();
                ActiveSpray = false;
            }
        }
    }

    void SprayDamage()
    {
        Line.lineDamage = sDamage;

        // Check if aimMovement is not null
        if (playerMovement != null)
        {
            // Access the currentPaintableObject from the playerMovement
            PaintableObject paintableObject = playerMovement.ActiveWall.GetComponent<PaintableObject>();


            // Check if paintableObject is not null
            if (paintableObject != null)
            {
                // Apply damage to the paintableObject
                playerMovement.ActiveWall.TakeDamage(Line.lineDamage);
            }
            else
            {
                Debug.LogWarning("No current paintable object set!");
            }
        }
        else
        {
            Debug.LogWarning("AimMovement script or GameObject reference not set!");
        }
    }



private void OnSprayStarted(InputAction.CallbackContext context)
    {
        // Ensure there is no existing current line when starting a new one
        if (currentLine != null)
        {
            // Destroy(currentLine);
        }


        // Create a new line at the current Aim position
        Vector2 aimPos = aimMovement.CurrentAim;
        currentLine = Instantiate(linePrefab, aimPos, Quaternion.identity);

        // Set the sorting order for the new line
        SetSortingOrder(0, currentLine); // Use sorting order 0 for all lines
    }

    private void OnSprayCanceled(InputAction.CallbackContext context)
    {
        // Set ActiveSpray to false when spraying is canceled
        ActiveSpray = false;

        // Finalize the current line when the "Spray" action is canceled
        FinalizeCurrentLine();
    }

    private void SetSortingOrder(int sortingOrder, GameObject obj)
    {
        // Set the sorting order for all renderers in the object and its children
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers)
        {
            renderer.sortingOrder = sortingOrder;
        }
    }

    private void FinalizeCurrentLine()
    {
        // Check if a current line exists before finalizing
        if (currentLine != null)
        {
            // Set currentLine to null, signaling that we're not drawing anymore
            currentLine = null;
        }
    }
}