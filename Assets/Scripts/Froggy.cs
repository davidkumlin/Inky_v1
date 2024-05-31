using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Froggy : MonoBehaviour
{
    private string initialDialogText = "JAAMAAAN!";
    private string subsequentDialogText = "Ivé never seen a liquidman before!\n Spray that wall! ";
    [SerializeField] private HUD hud;
    
    private bool hasInteractedwithFroggy = false;
    private bool hasMetFroggy = false;
    public bool readyToMeetSnaek = false;


 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasInteractedwithFroggy)
        {
            Debug.Log("groggy froggy");// Activate Froggy image and initial dialogue text in HUD
            hud.ShowFroggy();
            hud.ShowDialogue(initialDialogText);
            hasMetFroggy = true;
            // Start the coroutine to wait for 2 seconds before showing the subsequent dialogue
            StartCoroutine(ShowSubsequentDialogueAfterDelay());
        }
    }
    private IEnumerator ShowSubsequentDialogueAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (!hasInteractedwithFroggy && hasMetFroggy)
        {
            // Update dialogue text for subsequent interactions
            hud.ShowDialogue(subsequentDialogText);
            hud.ShowFroggy();
            hasInteractedwithFroggy = true; // Set hasInteracted to true to prevent further interactions
            StartCoroutine(CloseDialogueAfterDelay());
        }
        
    }
    private IEnumerator CloseDialogueAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (hasInteractedwithFroggy && hasMetFroggy)
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
        readyToMeetSnaek = true;
    }
}
