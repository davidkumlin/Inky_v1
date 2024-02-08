using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sprayFXtrail : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private GameObject startPosition;
    [SerializeField] private GameObject endPosition;

    private CustomInput input;

    void Start()
    {
        input = new CustomInput();
        input.Enable();
        input.Player.Spray.started += OnSprayStarted;
        input.Player.Spray.canceled += OnSprayCanceled;
    }

    private void OnDestroy()
    {
        input.Disable();
    }

    void Update()
    {
        if (input.Player.Spray.ReadValue<float>() > 0.1f)
        {
            UpdateTrailRenderer();
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }

    private void OnSprayStarted(InputAction.CallbackContext context)
    {
        trailRenderer.Clear();
        trailRenderer.emitting = true;
    }

    private void OnSprayCanceled(InputAction.CallbackContext context)
    {
        trailRenderer.emitting = false;
    }

    private void UpdateTrailRenderer()
    {
        trailRenderer.Clear();

        // Set the starting position
        if (startPosition != null)
        {
            trailRenderer.transform.position = startPosition.transform.position;
        }

        // Set the end position
        if (endPosition != null)
        {
            trailRenderer.transform.LookAt(endPosition.transform);
        }
    }
}
