using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brorcolli : MonoBehaviour
{
    public Rigidbody2D unitRb;
    [SerializeField] Transform pinkypos;
    P_Stats pstats = null;
    P_Inky pinky = null;
    AimMovement aimRef;
    private float DistanceToPlayer; // how far away is the player
    [SerializeField] private float CuttDistance = 5f;
    [SerializeField] private float resetDistance = 15f;
    public bool OnWall { get; private set; } = false;
    public Transform unitPos;
    bool isAtk = false;
    public bool isbrordead = false;
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
        pinky = FindObjectOfType<P_Inky>();
        if (pinky == null)
        {
            Debug.LogError("P_Inky not found");
        }
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("P_stats not found");
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!OnWall)
            {
                inBox = true;
                Attack();
            }
        }
    }
    void Attack()
    {
        if (!OnWall && inBox)
        {
            idle_arm.SetActive(false);
            Cut_arm.SetActive(true);
        }
        else
        {
            StopAttack();
        }

    }
    void StopAttack()
    {
        idle_arm.SetActive(true);
        Cut_arm.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inBox = false;
        Debug.Log(inBox);
        if (cutArmScript.shouldResetAtk == true)
        {
            StopAttack();
        }
    }
    void Update()
    {
        
        if (pinky.IsDrawing)
        {
            Vector2 sprayPosition = pinky.CurrentAim;

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
                    if (bounds.Contains(sprayPosition) && !OnWall)
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
        Debug.Log("brorcoli facesprayed");
        alivebro.SetActive(false);
        ChangeAnimationState(bror_death);
        isbrordead = true;
        
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
