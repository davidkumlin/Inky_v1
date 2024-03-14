using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brorcolli : MonoBehaviour
{
    public Rigidbody2D unitRb;
    [SerializeField] Transform player;
    PlayerMovement playref = null;
    AimMovement aimRef;
    private float DistanceToPlayer; // how far away is the player
    [SerializeField] private float CuttDistance = 5f;
    [SerializeField] private float resetDistance = 15f;
    public bool OnWall { get; private set; } = false;
    public Transform unitPos;
    bool isAtk = false;
    public Animator body_ani;
    public Animator arm_ani;
    [SerializeField] private GameObject idle_arm;
    [SerializeField] private GameObject Cut_arm;
    [SerializeField] GameObject FaceSpray;
    private string currentState;
    private Enemy_atk cutArmScript;
    public bool inBox = false;
    [SerializeField] GameObject alivebro;

    const string bror_death = "bror_death";
    const string Bro_idle = "Bro_idle";

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        cutArmScript = Cut_arm.GetComponent<Enemy_atk>();
        Cut_arm.SetActive(false);
        aimRef = FindObjectOfType<AimMovement>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playref = playerObject.GetComponent<PlayerMovement>();
            if (playref == null)
            {
                Debug.LogError("PlayerMovement component not found on the player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
        
        unitRb = GetComponent<Rigidbody2D>();
        if (unitRb != null)
            Debug.Log(unitRb.name + " Rigidbody2D found");
        else
            Debug.Log("Rigidbody2D not found on " + name);
    }
    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("PM" + OnWall);

    }
    // Update is called once per frame
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!OnWall)
        {
        inBox = true;
        Attack();
        }
    }
    void Attack()
    {
        idle_arm.SetActive(false);
        Cut_arm.SetActive(true);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inBox = false;
        if (cutArmScript.shouldResetAtk == true)
        {
            idle_arm.SetActive(true);
            Cut_arm.SetActive(false);

        }
    }
    void Update()
    {
        if (aimRef != null && aimRef.IsDrawing)
        {
            Vector2 sprayPosition = aimRef.CurrentAim;

            if (FaceSpray != null)
            {
                // Get the SpriteRenderer component from the FaceSpray GameObject
                SpriteRenderer spriteRenderer = FaceSpray.GetComponent<SpriteRenderer>();

                // Check if the SpriteRenderer component is not null
                if (spriteRenderer != null)
                {
                    // Get the bounds of the SpriteRenderer
                    Bounds bounds = spriteRenderer.bounds;

                    // Check if the spray position is inside the bounds of the FaceSpray
                    if (bounds.Contains(sprayPosition))
                    {
                        // Player is spraying onto the broccoli, so kill it
                        KillBroccoli();
                    }
                }
            }
        }
    }

    private void KillBroccoli()
    {
        Debug.Log("kill");
        alivebro.SetActive(false);
        ChangeAnimationState(bror_death);
        
        
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }


        //play animation
        body_ani.Play(newState);

        currentState = newState;
        //Debug.Log(newState);
    }

}
