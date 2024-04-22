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
    private Vector3 targetPosition;
    void Start()
    {
        // Get the PaintableObject component attached to the wall GameObject
        wallPaintableObject = wall.GetComponent<PaintableObject>();

        // Get the Animator component attached to the tail GameObject
        tailAnimator = tail.GetComponent<Animator>();

        // Store the target position for the SnaekHead
        targetPosition = SnaekHead.transform.position + new Vector3(0f, 3.2f, 0f);
    }
    void Update()
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
                tailAnimator.Play("snaek_tail_0");

                // Move the SnaekHead to the target position over 2 seconds
                StartCoroutine(MoveSnaekHead(targetPosition, 3f));
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
    }
}