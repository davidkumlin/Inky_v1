using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skjutvaggen : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [SerializeField] Brorcolli brorcolli;
    [SerializeField] drivebycar drivebycar;

    private PaintableObject wallPaintableObject;
    private bool doTheThis = false;


    public bool brordead = false;
    private bool shootaz = false;
    private bool carstarted = false;



    void Start()
    {
        
        // Get the PaintableObject component attached to the wall GameObject
        wallPaintableObject = wall.GetComponent<PaintableObject>();
        drivebycar = FindObjectOfType<drivebycar>(); 
        // Get the Animator component attached to the tail GameObject
        brorcolli = FindObjectOfType<Brorcolli>();
        if (brorcolli == null)
        {
            Debug.LogError("brorcolio not found");
        }
      
    }
    


    void Update()
    {
        if (!shootaz)
            {
            isbrordead();

            }
       
        // Check if the wall has been fully bombed
        if (wallPaintableObject != null && wallPaintableObject.fullyBombed)
        {
        Debug.Log("skjutväggen bombed!");

        if (!doTheThis)
        {

            doTheThis = true;

        }
            
         //brorcollistuff
        }
     


    }
    private void isbrordead()
    {
        if (brorcolli.isbrordead)
        {
            brordead = true;
            
            shootaz = true;
        }
    }
    
   

}
