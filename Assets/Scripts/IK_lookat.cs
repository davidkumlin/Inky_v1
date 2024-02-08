using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IK_lookat : MonoBehaviour
//this is for inkyV1
{
    [SerializeField] public GameObject _IK;
    [SerializeField] public GameObject sprayStart;
    [SerializeField] private LineRenderer sprayLine;
    [SerializeField] private ParticleSystem particleEffect;
    public float lineMaxLength = 5f;

    [SerializeField] private AimMovement aimMovement;
    private CustomInput input;

    public float speed;

    void Start()
    {
        // If sprayLine is not explicitly set, try to get it from the current GameObject
        if (sprayLine == null)
            sprayLine = GetComponent<LineRenderer>();

        // Set the initial positions of the LineRenderer
        UpdateLineRenderer();

        // Initialize AimMovement
        aimMovement = FindObjectOfType<AimMovement>();

        // Initialize input
        input = new CustomInput();
        input.Enable();
    }

    private void OnDestroy()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the "Spray" action is triggered
        if (input.Player.Spray.triggered)
        {
            // Trigger the particle effect at the Aim position
            if (particleEffect != null)
            {
                particleEffect.transform.position = aimMovement.CurrentAim;
                particleEffect.Play();
            }
        }

        // Get the Aim position from AimMovement.cs
        Vector2 aimPos = aimMovement.CurrentAim;

        // Calculate the interpolation factor based on speed and time
        float step = speed * Time.deltaTime;

        // Move the _IK object towards the Aim position
        _IK.transform.position = Vector2.Lerp(_IK.transform.position, aimPos, step);

        // Calculate the distance between sprayStart and Aim position
        float distance = Vector2.Distance(sprayStart.transform.position, aimPos);

        // Set the lineMaxLength property in the shader based on the calculated distance
        if (sprayLine.material != null)
        {
            sprayLine.material.SetFloat("_LineMatLength", distance);
        }

        // Set the positions of the LineRenderer
        UpdateLineRenderer();
    }

    void UpdateLineRenderer()
    {
        if (_IK != null && sprayLine != null && sprayStart != null)
        {
            // Set the starting position of the line to the sprayStart position
            sprayLine.SetPosition(0, sprayStart.transform.position);

            // Calculate the direction from sprayStart to the Aim position
            Vector3 direction = (Vector3)aimMovement.CurrentAim - sprayStart.transform.position;

            // Limit the line length to the specified maximum length
            if (direction.magnitude > lineMaxLength)
            {
                direction = direction.normalized * lineMaxLength;
            }

            // Set the end position of the line based on the calculated direction
            sprayLine.SetPosition(1, sprayStart.transform.position + direction);

            // Enable the LineRenderer
            sprayLine.enabled = true;
        }
    }
}