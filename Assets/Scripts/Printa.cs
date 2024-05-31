using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Printa : MonoBehaviour
{
    private string initialDialogText = "Be whatchful ahead of the Beez...";
    private string subsequentDialogText = "...and other villains in the area!";
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

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasInteractedwithPrinta)
        {
            Debug.Log("printa");// Activate Froggy image and initial dialogue text in HUD
            hud.ShowPrinta();
            hud.ShowDialogue(initialDialogText);
            hasMetPrinta = true;
            Checkpoint();
            StartCoroutine(ShowSubsequentDialogueAfterDelay());
        }
    }

    private IEnumerator ShowSubsequentDialogueAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (!hasInteractedwithPrinta && hasMetPrinta)
        {
            Debug.Log("printa2");// Update dialogue text for subsequent interactions
            hud.ShowDialogue(subsequentDialogText);
            hud.ShowPrinta();
            hasInteractedwithPrinta = true; // Set hasInteracted to true to prevent further interactions
            StartCoroutine(CloseDialogueAfterDelay());

        }
       
    }
    private IEnumerator CloseDialogueAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds


        CloseDialogue();
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