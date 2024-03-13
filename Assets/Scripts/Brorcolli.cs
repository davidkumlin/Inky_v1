using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brorcolli : MonoBehaviour
{
    public Rigidbody2D unitRb;
    [SerializeField] Transform player;
    PlayerMovement playref = null;
    private float DistanceToPlayer; // how far away is the player
    [SerializeField] private float CuttDistance = 5f;
    [SerializeField] private float resetDistance = 15f;
    public bool OnWall { get; private set; } = false;
    public Transform unitPos;
    bool isAtk = false;
   
    public Animator arm_ani;
    [SerializeField] private GameObject idle_arm;
    private string currentState;
    


    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;

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
    void Update()
    {
        Vector2 directionToPlayer = playref.transform.position - transform.position;
        DistanceToPlayer = directionToPlayer.magnitude; // Calculate distance to player
        //Debug.Log(DistanceToPlayer); // Log the distance for debugging purposes
        Debug.DrawRay(transform.position, directionToPlayer.normalized * DistanceToPlayer, Color.red); // Draw a debug ray

        // Check if the distance to the player is within the chase distance
        if (DistanceToPlayer < CuttDistance)
        {
            if (!isAtk)
            {
            Attack();
            }
        }
    }
    void Attack()
    {
        idle_arm.SetActive(false);
    }
}
