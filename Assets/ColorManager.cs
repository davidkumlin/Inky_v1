using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{

    [SerializeField] private Color color1 = Color.white;
    [SerializeField] private Color color2 = Color.black;
    private Color currentColor;
    private bool isColor1Active = true;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<TrailRenderer> trailRenderers = new List<TrailRenderer>();

    void Start()
    {
        currentColor = color1;
        ApplyColor();
    }

    void Update()
    {
        // Example input to toggle color (replace with your actual input handling)
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleColor();
        }
    }

    public void ToggleColor()
    {
        isColor1Active = !isColor1Active;
        currentColor = isColor1Active ? color1 : color2;
        ApplyColor();
    }

    private void ApplyColor()
    {
        foreach (var lineRenderer in lineRenderers)
        {
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;
        }

        foreach (var particleSystem in particleSystems)
        {
            var mainModule = particleSystem.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(currentColor);
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = currentColor;
        }

        foreach (var trailRenderer in trailRenderers)
        {
            trailRenderer.startColor = currentColor;
            trailRenderer.endColor = currentColor;
        }
    }

    public Color GetCurrentColor()
    {
        return currentColor;
    }

    public void RegisterLineRenderer(LineRenderer lineRenderer)
    {
        if (!lineRenderers.Contains(lineRenderer))
        {
            lineRenderers.Add(lineRenderer);
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;
        }
    }

    public void RegisterParticleSystem(ParticleSystem particleSystem)
    {
        if (!particleSystems.Contains(particleSystem))
        {
            particleSystems.Add(particleSystem);
            var mainModule = particleSystem.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(currentColor);
        }
    }

    public void RegisterSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        if (!spriteRenderers.Contains(spriteRenderer))
        {
            spriteRenderers.Add(spriteRenderer);
            spriteRenderer.color = currentColor;
        }
    }

    public void RegisterTrailRenderer(TrailRenderer trailRenderer)
    {
        if (!trailRenderers.Contains(trailRenderer))
        {
            trailRenderers.Add(trailRenderer);
            trailRenderer.startColor = currentColor;
            trailRenderer.endColor = currentColor;
        }
    }
}
