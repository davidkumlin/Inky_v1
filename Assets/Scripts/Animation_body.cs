using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_body : MonoBehaviour
{
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private AimMovement aimMovement;
    private SpriteRenderer spriteRenderer;
    private Animator bodyAnimator;

    public GameObject idleFront; // Assign your idle sprite in the Inspector
    public GameObject RB_Arm; // Assign your north-east facing sprite in the Inspector
    public GameObject RF_Arm; // Assign your south-east facing sprite in the Inspector
    public GameObject LF_Arm; // Assign your south-west facing sprite in the Inspector
    public GameObject LB_Arm; // Assign your north-west facing sprite in the Inspector

    private GameObject _IK = null;
    private float speed;
    private Vector2 currentAim;
    private bool OnWall;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bodyAnimator = GetComponent<Animator>();

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
    private void Update()
    {
        float step = speed * Time.deltaTime;

        // Only perform the search if the _IK is null
        if (_IK == null)
        {
            _IK = GameObject.Find("_IK_idle_front_hand");
            if (_IK == null)
            {
                Debug.LogError("GameObject with name '_IK_idle_front_hand' not found.");
                return;
            }
            else
            {
                Debug.Log("GameObject found: " + _IK.name);
            }
        }

        if (aimMovement != null && aimMovement.CurrentAim != null)
        {
            currentAim = aimMovement.CurrentAim;
            _IK.transform.position = Vector2.Lerp(_IK.transform.position, currentAim, step);
        }

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
        if (OnWall == false)
        {

            bodyAnimator.SetBool("OnWall", false);
        }
        if (playerMovement != null && !OnWall)
        {
            Vector2 moveVector = playerMovement.moveVector; // Access moveVector from PlayerMovement script
            idleFront.SetActive(true);
            // Debug.Log("Move Vector: " + moveVector);

            if (moveVector.x > 0 && moveVector.y > 0)
            {
                bodyAnimator.SetTrigger("NorthEast"); //RB
                RB_Arm.SetActive(true);
                RF_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);
                idleFront.SetActive(false);

                _IK = RB_Arm.transform.Find("_IK_RB_Hand").gameObject; // Find the child GameObject named "_IK_RB_Hand"
            }
            else if (moveVector.x > 0 && moveVector.y < 0)
            {
                bodyAnimator.SetTrigger("SouthEast"); //RF
                RF_Arm.SetActive(true);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);
                idleFront.SetActive(false);

                _IK = RB_Arm.transform.Find("_IK_RF_Hand").gameObject;
            }
            else if (moveVector.x < 0 && moveVector.y < 0)
            {
                bodyAnimator.SetTrigger("SouthWest"); //LF
                RF_Arm.SetActive(false);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(true);
                LB_Arm.SetActive(false);
                idleFront.SetActive(false);

            }
            else if (moveVector.x < 0 && moveVector.y > 0)
            {
                bodyAnimator.SetTrigger("NorthWest"); //LB
                RF_Arm.SetActive(false);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(true);
                idleFront.SetActive(false);
            }
            else
            {
                bodyAnimator.SetTrigger("IdleSouth");
                idleFront.SetActive(true);
                RF_Arm.SetActive(false);
                RB_Arm.SetActive(false);
                LF_Arm.SetActive(false);
                LB_Arm.SetActive(false);
                // Check if idlefront has the child object
                Transform childIK = idleFront.transform.Find("_IK_idle_front_hand");
                if (childIK != null)
                {
                    _IK = childIK.gameObject;
                }
                else
                {
                    Debug.LogError("_IK_idle_front_hand child object not found in idleFront.");
                }

            }
        }
        else
        {
            Debug.LogError("PlayerMovement script not found!");
        }
    }
}
