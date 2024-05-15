using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class DebugPolygonCollider : MonoBehaviour
{
    [SerializeField] private Color debugColor = Color.blue;
    [SerializeField] private float debugThickness = 0.1f;

    private void OnDrawGizmos()
    {
        DrawDebugLines();
    }

    private void OnDrawGizmosSelected()
    {
        DrawDebugLines();
    }

    private void DrawDebugLines()
    {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider != null)
        {
            Gizmos.color = debugColor;
            Gizmos.matrix = transform.localToWorldMatrix;

            for (int i = 0; i < polygonCollider.pathCount; i++)
            {
                Vector2[] points = polygonCollider.GetPath(i);

                for (int j = 0; j < points.Length; j++)
                {
                    int nextIndex = (j + 1) % points.Length;
                    Gizmos.DrawLine(points[j], points[nextIndex]);
                }
            }
        }
    }
}

