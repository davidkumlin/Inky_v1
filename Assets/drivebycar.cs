using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drivebycar : MonoBehaviour
{

    [SerializeField] skjutvaggen skjutvaggen;
    
    [SerializeField] Collider2D shotarea;
    [SerializeField] GameObject gunny;
    [SerializeField] GameObject shootinggunny;
    private bool shootaz = false;
    public bool carstarted = false;
    [SerializeField] private P_Stats pstats;
    private float speed = -6f;

    public Rigidbody2D unitRb;
    private float Damage = 100f;
    public bool OnWall { get; private set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        skjutvaggen = FindObjectOfType<skjutvaggen>();
        shotarea = GetComponent<Collider2D>(); 
        //CarStart = GetComponentInChildren<BoxCollider2D>();
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("P_Inky not found");
        }
    }

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("PM" + OnWall);

    }
    // Update is called once per frame
    void Update()
    {
        runCar();
    }

private void runCar()
    {
        if (skjutvaggen.brordead)
        {
            if (!shootaz)
            gunny.SetActive(true);


        }

        // Calculate movement vector
        float horizontalMovement = speed;
        if (carstarted)
        {
            // Apply movement to the Rigidbody
            Vector2 movement = new Vector2(horizontalMovement, unitRb.velocity.y);
            unitRb.velocity = movement;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && skjutvaggen.brordead)
        {
            Debug.Log("pang pang");

            gunny.SetActive(false);
            shootaz = true;
            shootinggunny.SetActive(true);
            speed = -8f;
        }
    }

}


