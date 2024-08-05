using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linecontroller : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Texture[] textures;

    private int animationStep;

    [SerializeField] private float fps = 15f;
    private float fpsCounter;

    private ColorManager colorManager;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        colorManager = FindObjectOfType<ColorManager>();

        if (colorManager != null)
        {
            colorManager.RegisterLineRenderer(lineRenderer);
            // Set initial color
            Color currentColor = colorManager.GetCurrentColor();
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;
        }
        else
        {
            Debug.LogWarning("ColorManager not found in the scene.");
        }
    }

    private void Update()
    {
        fpsCounter += Time.deltaTime;

        if (fpsCounter >= 1f / fps)
        {
            animationStep++;
            if (animationStep == textures.Length)
                animationStep = 0;

            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);

            fpsCounter = 0f;
        }
    }
}
