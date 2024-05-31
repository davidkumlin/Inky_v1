using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Yeti : MonoBehaviour
{
    private string Yetitalk = "Herrow friend, whatch out some nasty creeps around here";
    [SerializeField] private HUD hud;

    private bool hasInteractedwithYeti = false;
    private bool hasMetYeti = false;
    //private InputAction interactAction;

   

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
    private void OnTriggerExit2D(Collider2D collision)
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
