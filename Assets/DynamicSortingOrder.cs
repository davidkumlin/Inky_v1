using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int sortingOrderAbove = -5;
    [SerializeField] private string sortingLayerAbove = "FG";
    [SerializeField] private int sortingOrderBelow = 1;
    [SerializeField] private string sortingLayerBelow = "Default";
    [SerializeField] private Transform referencePoint;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (!referencePoint)
        {
            referencePoint = transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }
    }

    void Update()
    {
        if (playerTransform.position.y > referencePoint.position.y)
        {
            spriteRenderer.sortingOrder = sortingOrderAbove;
            spriteRenderer.sortingLayerName = sortingLayerAbove;
        }
        else
        {
            spriteRenderer.sortingOrder = sortingOrderBelow;
            spriteRenderer.sortingLayerName = sortingLayerBelow;
        }
    }

    void OnDrawGizmos()
    {
        if (referencePoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(referencePoint.position + Vector3.left * 0.5f, referencePoint.position + Vector3.right * 0.5f);
        }
    }
}
