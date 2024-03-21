using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Yeti : MonoBehaviour
{
    private string Yetitalk = "Use Left trigger to hide in the wall or \nin the bush... Press (A) på close";
    [SerializeField] private HUD hud;

    private bool hasInteractedwithYeti = false;
    private bool hasMetYeti = false;
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
        if (collision.CompareTag("Player") && !hasInteractedwithYeti && !hasMetYeti)
        {
            Debug.Log("yeti");
            // Activate Yeti image and initial dialogue text in HUD
            hud.ShowYeti();
            hud.ShowDialogue(Yetitalk);
            hasMetYeti = true;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!hasInteractedwithYeti && hasMetYeti)
        {
                        
            CloseDialogue();
        }
    }

    // Method to close dialogue and hide image
    private void CloseDialogue()
    {
        hud.HideYeti();
        hud.HideDialogue();
        hasInteractedwithYeti = true; // Set hasInteracted to true to prevent further interactions
    }
}
