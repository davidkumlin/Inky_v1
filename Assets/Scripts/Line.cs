using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;

    public List<Vector2> SprayedPoints { get; private set; } = new List<Vector2>();

    [SerializeField] private ParticleSystem _particleSystem;
    public float lineWidth = 0.3f ; // Default line width
    
    public static float lineDamage = 1f; // Static variable to hold the line width
    
    private List<Vector2> _points = new List<Vector2>();

    public bool OnWall { get; private set; } = false;

    [SerializeField] public bool onwallprefab;

    [SerializeField] private float maxWidth = 0.4f;
    [SerializeField] private float minWidth = 0.25f;
    [SerializeField] private float duration = 1.0f;

    private float timeElapsed = 0f;
    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;

        // Set the initial width of the LineRenderer
        _renderer.startWidth = lineWidth;
        _renderer.endWidth = lineWidth;
        Debug.Log(lineDamage);
    }


    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;

    }

    void Update()
    {
        if (onwallprefab)
        {
            UpdateLineWidth();
            //Debug.Log(lineDamage);
        }

    }
    private void UpdateLineWidth()
    {
        // Update the elapsed time
        timeElapsed += Time.deltaTime;

        // Calculate the new width using Mathf.Lerp
        float newWidth = Mathf.Lerp(maxWidth, minWidth, Mathf.PingPong(timeElapsed / duration, 1.0f));

        // Set the new width of the LineRenderer
        _renderer.startWidth = newWidth;
        _renderer.endWidth = newWidth;
    }

    public void SetPosition(Vector2 pos)
    {

        if (!CanAppend(pos)) return;

        _points.Add(pos);
        SprayedPoints.Add(pos);

        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos);

        

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

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), pos) > DrawManager_2.RESOLUTION;
    }
}