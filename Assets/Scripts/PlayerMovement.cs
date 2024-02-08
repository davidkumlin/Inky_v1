using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private CustomInput input = null;
    public Vector2 moveVector { get; private set; } = Vector2.zero;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private AimMovement aimMovement;
    [SerializeField] public float maxDistance = 5f;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    //For the ON WALL mechanics
    public bool OnWall { get; private set; } = false;
    public bool InHiding { get; private set; } = false;
    public bool Alerted { get; private set; } = false;

    private List<PaintableObject> paintableObjectsList;

    private bool canToggleOnWall = true;

    private void Awake()
    {
        input = new CustomInput();
        aimMovement = GetComponentInChildren<AimMovement>();
    }

    private void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;

        PaintableObject[] allPaintableObjects = FindObjectsOfType<PaintableObject>();
        paintableObjectsList = new List<PaintableObject>(allPaintableObjects);

        if (paintableObjectsList.Count == 0)
        {
            Debug.LogWarning("No PaintableObject scripts found in the scene.");
        }
        else
        {
            foreach (PaintableObject po in paintableObjectsList)
            {
                //     Debug.Log("PaintableObject found: " + po.gameObject.name);
            }
        }
    }

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
    }

    private void FixedUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            cameraPosition.x = transform.position.x;
            cameraPosition.y = transform.position.y;
            mainCamera.transform.position = cameraPosition;
        }

        if (!OnWall)
        {
            rb.velocity = moveVector * moveSpeed;
        }
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.OnWall.started += OnWallStarted;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.OnWall.started -= OnWallStarted;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void OnWallStarted(InputAction.CallbackContext value)
    {
        if (canToggleOnWall)
        {
            if (paintableObjectsList == null || paintableObjectsList.Count == 0)
            {
                Debug.LogWarning("No PaintableObject scripts found in the scene.");
                return;
            }

            // Example: Check if any of the paintable objects have the aim inside the sprite mask
            foreach (PaintableObject po in paintableObjectsList)
            {
                if (!po.IsAimInsideSpriteMask(aimMovement.CurrentAim))
                {
                    continue;
                }
                OnWall = !OnWall;
                GameManager.Instance.OnWall = OnWall;
                if (OnWall)
                {
                    if (playerSpriteRenderer != null)
                        playerSpriteRenderer.enabled = false;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    if (playerSpriteRenderer != null)
                        playerSpriteRenderer.enabled = true;
                }
                StartCoroutine(ToggleCooldown());
                return;
            }

            Debug.Log("Cannot set OnWall: Aim is not inside the sprite mask of any PaintableObject.");
        }
    }

    private IEnumerator ToggleCooldown()
    {
        canToggleOnWall = false;
        yield return new WaitForSeconds(1f);
        canToggleOnWall = true;
    }
}