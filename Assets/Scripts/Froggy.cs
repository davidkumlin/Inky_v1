using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Froggy : MonoBehaviour
{
    private string initialDialogText = "JAAMAAAN! Youré Inky right?\nPress (A)";
    private string subsequentDialogText = "Use Right Stick to aim and Right Trigger\n to Spray the wall! Press(A) to close";
    [SerializeField] private HUD hud;

    private bool hasInteractedwithFroggy = false;
    private bool hasMetFroggy = false;
    private InputAction interactAction;

    private void OnEnable()
    {
        // Enable the interact action
        interactAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the interact action
        interactAction.Disable();
    }

    private void Awake()
    {
        // Get a reference to the interact action from the input system
        interactAction = new InputAction(binding: "<Gamepad>/buttonSouth"); // Change this binding to match your interact button
        interactAction.performed += ctx => OnInteract(ctx);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasInteractedwithFroggy)
        {
            Debug.Log("groggy froggy");// Activate Froggy image and initial dialogue text in HUD
            hud.ShowFroggy();
            hud.ShowDialogue(initialDialogText);
            hasMetFroggy = true;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!hasInteractedwithFroggy && hasMetFroggy)
        {
            // Update dialogue text for subsequent interactions
            hud.ShowDialogue(subsequentDialogText);
            hud.ShowFroggy();
            hasInteractedwithFroggy = true; // Set hasInteracted to true to prevent further interactions
        }
        else if (hasInteractedwithFroggy && hasMetFroggy)
        {
            // Close dialogue
            CloseDialogue();
        }
    }

    // Method to close dialogue and hide Froggy image
    private void CloseDialogue()
    {
        hud.HideFroggy();
        hud.HideDialogue();
        
    }
}
