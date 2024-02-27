using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private EdgeCollider2D _collider;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] public float lineWidth = 1; // Default line width
    public static float lineDamage; // Static variable to hold the line width

    private List<Vector2> _points = new List<Vector2>();


    void Start()
    {
        lineWidth = lineDamage;

        _collider.transform.position -= transform.position;

        // Set the initial width of the LineRenderer
        _renderer.startWidth = lineWidth;
        _renderer.endWidth = lineWidth;

        
        
    }

    void Update()
    {
      //  Debug.Log(lineDamage);
    }

    
    public void SetPosition(Vector2 pos)
    {

        if (!CanAppend(pos)) return;

        _points.Add(pos);

        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos);

        _collider.points = _points.ToArray();

        if (_points.Count % 10 == 0)
        {
            // Get the position at the current index
            Vector2 particlePosition = _points[_points.Count - 1];

            // Trigger the particle system at the calculated position
            if (_particleSystem != null)
            {
                _particleSystem.transform.position = particlePosition;
                _particleSystem.Play();
            }
        }
   
    }

    private bool CanAppend(Vector2 pos)
    {
        if (_renderer.positionCount == 0) return true;

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), pos) > DrawManager.RESOLUTION;
    }
}