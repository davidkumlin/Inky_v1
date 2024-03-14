using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Printa : MonoBehaviour
{
    private string initialDialogText = "Be whatchful ahead of the Beez...\nPress (A)";
    private string subsequentDialogText = "...and other villains in the area!\nPress(A) to close";
    [SerializeField] private HUD hud;
    private bool hasMetPrinta = false;
    private bool hasInteractedwithPrinta = false;

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
        if (collision.CompareTag("Player") && !hasInteractedwithPrinta)
        {
            Debug.Log("printa");// Activate Froggy image and initial dialogue text in HUD
            hud.ShowPrinta();
            hud.ShowDialogue(initialDialogText);
            hasMetPrinta = true;
            Checkpoint();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!hasInteractedwithPrinta && hasMetPrinta)
        {
            Debug.Log("printa2");// Update dialogue text for subsequent interactions
            hud.ShowDialogue(subsequentDialogText);
            hud.ShowPrinta();
            hasInteractedwithPrinta = true; // Set hasInteracted to true to prevent further interactions
        }
        else if (hasInteractedwithPrinta && hasMetPrinta)
        {
            // Close dialogue
            CloseDialogue();
        }
    }

    // Method to close dialogue and hide Froggy image
    private void CloseDialogue()
    {
        hud.HidePrinta();
        hud.HideDialogue();
        

    }

    public void Checkpoint()
    {
        Debug.Log("Checkpoint (not done)");
    }
}