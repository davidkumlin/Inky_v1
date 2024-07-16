using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DynamicGroundCollider : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private bool isActive = false;
    private float cooldownTimer = 0f;
    [SerializeField] private float cooldownDuration = 1f;

    void Start()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundCollider.enabled = false;
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        // Visualize the collider in the editor
        DebugDrawBox(groundCollider.bounds.center, groundCollider.bounds.size, Color.blue);
    }

    void DebugDrawBox(Vector3 center, Vector3 size, Color color)
    {
        Vector3 halfSize = size * 0.5f;
        Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y);
        Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y);
        Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y);
        Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y);

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }

    public void SetActive(bool active)
    {
        if (cooldownTimer <= 0)
        {
            isActive = active;
            groundCollider.enabled = active; // Enable or disable the collider

            if (!active)
            {
                cooldownTimer = cooldownDuration; // Set cooldown when deactivated
            }
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}
