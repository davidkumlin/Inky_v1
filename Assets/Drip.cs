using System.Collections;
using UnityEngine;

public class Drip : MonoBehaviour
{
    private Rigidbody rb;
    
    public float destroyDelay = 100f; // Delay before destroying the object

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(TurnOffGravityDelayed());
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator TurnOffGravityDelayed()
    {
        float gravityOffDelay = Random.Range(1f, 4f);
        // Wait for the specified delay before turning off gravity
        yield return new WaitForSeconds(gravityOffDelay);

        // Turn off gravity
        if (rb != null)
        {
            rb.useGravity = false;
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        // Wait for the specified delay before destroying the object
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the game object
        Destroy(gameObject);
    }
}