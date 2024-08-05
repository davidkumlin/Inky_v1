using UnityEngine;

public class ParticleColorChanger : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ColorManager _colorManager;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _colorManager = FindObjectOfType<ColorManager>();

        if (_colorManager == null)
        {
            Debug.LogError("ColorManager not found in the scene.");
            return;
        }

        _colorManager.RegisterParticleSystem(_particleSystem);

        // Set the initial color of the particle system
        var mainModule = _particleSystem.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(_colorManager.GetCurrentColor());
    }
}
