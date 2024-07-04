using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snaek : MonoBehaviour
{
    [SerializeField] GameObject SnaekHead;
    [SerializeField] GameObject tail;
    [SerializeField] GameObject wall;
    private PaintableObject wallPaintableObject;
    private Animator tailAnimator;
    private bool doTheThis = false;
    private bool bombed =false;
    private Vector3 targetPosition;
    private Froggy froggy;
    private BoxCollider2D triggerbox;
    private string initialDialogText = "JAAAMAAAAAN!";
    [SerializeField] private HUD hud;
    private bool hasMetSnaek = false;
    void Start()
    {
        // Get the PaintableObject component attached to the wall GameObject
        wallPaintableObject = wall.GetComponent<PaintableObject>();

        // Get the Animator component attached to the tail GameObject
        tailAnimator = tail.GetComponent<Animator>();

        froggy = FindObjectOfType<Froggy>();


        // Store the target position for the SnaekHead
        targetPosition = SnaekHead.transform.position + new Vector3(0f, 3.2f, 0f);

        triggerbox = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (!bombed)
        { 
        // Check if the wall has been fully bombed
        if (wallPaintableObject != null && wallPaintableObject.fullyBombed)
        {
            Debug.Log("skogsmuren bombed!");

            if (!doTheThis)
            {
                doTheThis = true;

                // Activate the tail GameObject
                tail.SetActive(true);

                // Play the animation on the tail GameObject
                tailAnimator.Play("snaek_tail");

                // Move the SnaekHead to the target position over 2 seconds
                StartCoroutine(MoveSnaekHead(targetPosition, 3f));
            }
        }
        }
    }


    IEnumerator MoveSnaekHead(Vector3 targetPosition, float duration)
    {
        Vector3 initialPosition = SnaekHead.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the current position based on time
            float t = elapsedTime / duration;
            SnaekHead.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the SnaekHead reaches the target position exactly
        SnaekHead.transform.position = targetPosition;
        if (SnaekHead.transform.position == targetPosition)
        {
            Debug.Log("snaekhead at place");
            triggerbox.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (froggy.readyToMeetSnaek)
        {

            if (collision.CompareTag("Player") && !hasMetSnaek)
            {
                Debug.Log("snaek");// Activate Froggy image and initial dialogue text in HUD
                hud.ShowSnaek();
                hud.ShowDialogue(initialDialogText);
                hasMetSnaek = true;
                // Start the coroutine to wait for 2 seconds before showing the subsequent dialogue
                StartCoroutine(CloseDialogueAfterDelay());
            }
        }
    }

    private IEnumerator CloseDialogueAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (hasMetSnaek)
        {
            // Close dialogue
            CloseDialogue();
        }
    }

    // Method to close dialogue and hide Froggy image
    private void CloseDialogue()
    {
        hud.HideSnaek();
        hud.HideDialogue();
        bombed = true;

    }
}