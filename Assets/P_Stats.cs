using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class P_Stats : MonoBehaviour
{

    [SerializeField] public GameObject inkyObj;
    [SerializeField] private P_Inky pinky;
    [SerializeField] private inky_animation inkyAni;
    private Vector2 inkyPos;
    public bool inkyActive;
    
   
    public Vector2 aimPos;
    

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 cameraOffset;

    public float hp = 100f;
    public float maxHp = 100f;

    public bool OnWall { get; private set; } = false;


        private void Awake()
    {
        
         pinky = FindObjectOfType<P_Inky>();
         inkyAni = FindObjectOfType<inky_animation>();
        

    }

    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        mainCamera = Camera.main;
    }
    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("P_stats" + OnWall);
    }
    // Update is called once per frame
    void Update()
    {
        
        aimPos = pinky.CurrentAim;
        inkyPos = pinky.whereIsInky;
        
        ActiveUnit();
    }

    public void Damage()
    {
        Debug.Log("Damage");
        if (inkyAni.takinDamage == false)
        {
        inkyAni.takinDamage = true;

            if (hp <= 0)
            {
                inkyAni.dying = true;
                pinky.moveSpeed = 0;
            }

        }
    }

    public void ResetLevel()
    {
        // Reset the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ActiveUnit()
    {
        if (!OnWall)
        {
            inkyActive = true;
            
            


        }
        else
        {
            inkyActive = true;
            
            
        }
        
     }
       
    
    private void LateUpdate()
    {
        {
            // Determine which character is active
            Transform activeCharacter = null;
            if (pinky.inkyActive)
            {
                activeCharacter = pinky.transform;
            }
            /*else if (pwally.wallyActive)
            {
                activeCharacter = pwally.transform;
            }*/

            // Follow the active character
            if (activeCharacter != null)
            {
                Vector3 targetPosition = activeCharacter.position + cameraOffset; 
                targetPosition.z = mainCamera.transform.position.z; // Maintain the same z position
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
        }
    }
   
}
