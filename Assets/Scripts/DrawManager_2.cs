using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class DrawManager_2 : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject OnWallLinePrefab;
    public const float RESOLUTION = .1f;
    private GameObject currentLine;
     
    private P_Inky pinky;
    //private PaintableObject paintableObject; // Reference to the PaintableObject script
    private List<PaintableObject> paintableObjects = new List<PaintableObject>(); // List to store all PaintableObject instances
    private CustomInput input;

    // Example method call in DrawManager.cs
    public PaintableObject paintableObject;
    //bool isInsideMask = paintableObject.IsAimInsideSpriteMask(aimPosition, spriteMask);
    // Public bool to track if spraying is active
    public bool OnWall { get; private set; } = false;
    public bool ActiveSpray { get; private set; } = false;
    public float sDamage;

  //Sounds



    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        
        pinky = FindObjectOfType<P_Inky>();
        // paintableObject = FindObjectOfType<PaintableObject>(); // Assign the PaintableObject reference
        // Find all PaintableObject instances in the scene and add them to the list
        PaintableObject[] allPaintableObjects = FindObjectsOfType<PaintableObject>();
        paintableObjects.AddRange(allPaintableObjects);

        input = new CustomInput();
        input.Enable();
        input.Player.Spray.started += OnSprayStarted;
        input.Player.Spray.canceled += OnSprayCanceled;

        // Create instances of FMOD sound events
     
    }
    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        Debug.Log("DRawmanager" + OnWall);

    }


    private void OnDestroy()
    {
        input.Disable();
        // Release FMOD sound instances
      
    }

    void Update()
    {
        if (OnWall)
        {
            // Check if the player is on the wall
            Vector2 aimPos = pinky.CurrentAim;

            bool isInsideAnyObject = paintableObjects.Any(obj => obj != null && obj.IsAimInsideSpriteMask(aimPos));
            if (isInsideAnyObject && paintableObjects.Any(obj => obj != null && obj.IsInPaintSpace))
            {
                GameObject linePrefabToUse = OnWallLinePrefab;

                if (isInsideAnyObject)
                {
                    if (currentLine == null)
                    {
                        // If no current line, create a new one using the appropriate line prefab
                        currentLine = Instantiate(linePrefabToUse, aimPos, Quaternion.identity);

                        // Set the sorting order for the line
                        SetSortingOrder(0, currentLine); // Use sorting order 0 for all lines
                    }

                    // Update the position of the current line
                    currentLine.GetComponent<Line>().SetPosition(aimPos);
                    // Set ActiveSpray to true when spraying starts
                    ActiveSpray = true;
                    SprayDamage();
                    // Check if the spray start sound is not playing
                }
            }
            else
            {
                // If the aim position is outside the colored area of the sprite or outside the bounds, finalize the current line
                FinalizeCurrentLine();
                ActiveSpray = false;
            }
        }
        else if (pinky != null && input.Player.Spray.ReadValue<float>() > 0.1f)
        {
            // Check if the player is spraying while not on the wall
            Vector2 aimPos = pinky.CurrentAim;
            bool isInsideAnyObject = paintableObjects.Any(obj => obj != null && obj.IsAimInsideSpriteMask(aimPos));
            if (isInsideAnyObject && paintableObjects.Any(obj => obj != null && obj.IsInPaintSpace))
            {
                GameObject linePrefabToUse = linePrefab;

                if (isInsideAnyObject)
                {
                    if (currentLine == null)
                    {
                        // If no current line, create a new one using the appropriate line prefab
                        currentLine = Instantiate(linePrefabToUse, aimPos, Quaternion.identity);

                        // Set the sorting order for the line
                        SetSortingOrder(0, currentLine); // Use sorting order 0 for all lines
                    }

                    // Update the position of the current line
                    currentLine.GetComponent<Line>().SetPosition(aimPos);
                    // Set ActiveSpray to true when spraying starts
                    ActiveSpray = true;
                    SprayDamage();
                    // Check if the spray start sound is not playing
                }
                else
                {
                    // If the aim position is outside the colored area of the sprite or outside the bounds, finalize the current line
                    FinalizeCurrentLine();
                    ActiveSpray = false;
                }
            }
        }
        else
        {
            // If the "Spray" action is not held down, finalize the current line
            FinalizeCurrentLine();
            ActiveSpray = false;
        }
    }



    void SprayDamage()
    {
        Line.lineDamage = sDamage;

        // Check if aimMovement is not null
        if (pinky != null)
        {
            // Access the currentPaintableObject from the playerMovement
            PaintableObject paintableObject = pinky.ActiveWall.GetComponent<PaintableObject>();


            // Check if paintableObject is not null
            if (paintableObject != null)
            {
                // Apply damage to the paintableObject
                pinky.ActiveWall.TakeDamage(Line.lineDamage);
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
        Vector2 aimPos = pinky.CurrentAim;
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