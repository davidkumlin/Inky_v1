using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drip : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float range1 = 1f;
    [SerializeField] private float range2 = 4f;
    public float destroyDelay = 100f; // Delay before destroying the object

    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private Color initialColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        var colorManager = FindObjectOfType<ColorManager>();
        if (colorManager != null)
        {
            initialColor = colorManager.GetCurrentColor();
            ApplyInitialColor();
        }
        else
        {
            Debug.LogWarning("ColorManager not found in the scene.");
        }

        StartCoroutine(TurnOffGravityDelayed());
        StartCoroutine(DestroyAfterDelay());
    }

    private void ApplyInitialColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = initialColor;
        }

        if (trailRenderer != null)
        {
            trailRenderer.startColor = initialColor;
            trailRenderer.endColor = initialColor;
        }
    }

    IEnumerator TurnOffGravityDelayed()
    {
        float gravityOffDelay = Random.Range(range1, range2);
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
