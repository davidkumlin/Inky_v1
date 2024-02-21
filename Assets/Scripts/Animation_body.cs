using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animation_body : MonoBehaviour
{
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private AimMovement aimMovement;
    private SpriteRenderer spriteRenderer;
    private Animator bodyAnimator;
    public UnityEvent myevent;
    [SerializeField] private GameObject idleFront; // Assign your idle sprite in the Inspector
    [SerializeField] private GameObject RB_Arm; // Assign your north-east facing sprite in the Inspector
    [SerializeField] private GameObject RF_Arm; // Assign your south-east facing sprite in the Inspector
    [SerializeField] private GameObject LF_Arm; // Assign your south-west facing sprite in the Inspector
    [SerializeField] private GameObject LB_Arm; // Assign your north-west facing sprite in the Inspector

    [SerializeField] private GameObject IK_idle;
    [SerializeField] private GameObject IK_RB;
    [SerializeField] private GameObject IK_RF;
    [SerializeField] private GameObject IK_LF;
    [SerializeField] private GameObject IK_LB;

    private GameObject _IK = null;
    public Vector2 _IK_pos;
    private float speed = 2f;
    private Vector2 currentAim;
    private bool OnWall;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bodyAnimator = GetComponent<Animator>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found.");
        }
        else
        {
            Debug.Log("playerMovement component found on: ");
        }
        // Attempt to find AimMovement component by name
        GameObject aimGameObject = GameObject.Find("Aim");
        if (aimGameObject != null)
        {
            aimMovement = aimGameObject.GetComponent<AimMovement>();
        }

        if (aimMovement == null)
        {
            Debug.LogError("AimMovement component not found.");
        }
        else
        {
            Debug.Log("AimMovement component found on: " + aimMovement.gameObject.name);
        }
    }

    private void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
    }
    void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        Debug.Log("AM" + OnWall);
       
    }
    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        // Debug log to track _IK_pos during runtime
        //Debug.Log("_IK_pos: " + _IK_pos);
        // Only perform the search if the _IK is null
        if (_IK == null)
        {
            _IK = IK_idle;
            //Debug.Log("Initialized _IK to IK_idle");
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found.");
        }
        else
        {
            Debug.Log("playerMovement component found");
        }

        if (aimMovement != null && aimMovement.CurrentAim != null)
        {
            currentAim = aimMovement.CurrentAim;
            // Store the local position of _IK
            _IK_pos = _IK.transform.localPosition;
            // Convert the currentAim to local space
            Vector3 currentAimLocal = _IK.transform.InverseTransformPoint(currentAim);
            _IK.transform.localPosition = Vector2.Lerp(_IK.transform.localPosition, currentAimLocal, step);
        }
        // Debug logs for debugging purposes
        // Debug.Log("IK Name: " + _IK.name);
        //Debug.Log("Current Aim: " + currentAim);
        Animate();
    }


    private void Animate()
    {
        if (OnWall == true)
        {
            bodyAnimator.SetTrigger("Splat");
            bodyAnimator.SetBool("OnWall", true);
            RB_Arm.SetActive(false);
            RF_Arm.SetActive(false);
            LF_Arm.SetActive(false);
            LB_Arm.SetActive(false);
            idleFront.SetActive(false);
        }
       
        else if (playerMovement != null && !OnWall) // Check playerMovement first
        {
            Vector2 moveVector = playerMovement.moveVector; // Access moveVector from PlayerMovement script
            idleFront.SetActive(true);
            // Debug.Log("Move Vector: " + moveVector);
            bodyAnimator.SetBool("OnWall", false);

            if (moveVector.x > 0 && moveVector.y > 0) // Moving up and right (Northeast)
            {
                bodyAnimator.SetTrigger("NorthEast"); //RB
                RB_Arm.SetActive(true);
                RF_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);
                idleFront.SetActive(false);

                _IK = IK_RB;
            }
            else if (moveVector.x > 0) // Moving right
            {
                bodyAnimator.SetTrigger("NorthEast"); //RB
                RB_Arm.SetActive(true);
                RF_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);
                idleFront.SetActive(false);

                _IK = IK_RB;
            }
            else if (moveVector.x > 0 && moveVector.y < 0) // Moving down and right (Southeast)
            {
                bodyAnimator.SetTrigger("SouthEast"); //RF
                RF_Arm.SetActive(true);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);
                idleFront.SetActive(false);

                _IK = IK_RF;
            }
            else if (moveVector.x < 0 && moveVector.y < 0) // Moving down and left (Southwest)
            {
                bodyAnimator.SetTrigger("SouthWest"); //LF
                RF_Arm.SetActive(false);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(true);
                LB_Arm.SetActive(false);
                idleFront.SetActive(false);

                _IK = IK_LF;
            }
            else if (moveVector.x < 0 && moveVector.y > 0)
            {
                bodyAnimator.SetTrigger("NorthWest"); //LB
                RF_Arm.SetActive(false);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(true);
                idleFront.SetActive(false);

                _IK = IK_LB;
            }
            else if (moveVector.x < 0) // Moving left
            {
                bodyAnimator.SetTrigger("NorthWest"); //LB
                RF_Arm.SetActive(false);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(true);
                idleFront.SetActive(false);

                _IK = IK_LB;
            }
            else
            {
                bodyAnimator.SetTrigger("IdleSouth");
                idleFront.SetActive(true);
                RF_Arm.SetActive(false);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);

                _IK = IK_idle;
            }
        }
        else
        {
            Debug.LogError("PlayerMovement script not found!");
            return; // Exit the method early
        }
    }
}
