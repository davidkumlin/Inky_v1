using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim_fx : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    public bool OnWall { get; private set; } = false;
    private bool hasSubscribed = false;
    private string currentState;
    private P_Inky pinky;
    const string BigSplat = "BigSplat";
    const string Painting = "Painting";
    const string BigDrips = "BigDrips";
    const string FullyPainted = "FullyPainted";
    private inky_animation inkyani;

    void Start()
    {
        inkyani = FindObjectOfType<inky_animation>();
        if (inkyani == null)
        {
            Debug.LogError("inkyani component not found");
        }
        pinky = FindObjectOfType<P_Inky>();
        if (pinky == null)
        {
            Debug.LogError("pINKY component not found");
        }
        GameManager.OnWallChanged += OnWallStatus;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        PaintableObject[] paintableObjects = FindObjectsOfType<PaintableObject>();
        foreach (PaintableObject paintableObject in paintableObjects)
        {
            paintableObject.OnFullyBombed.AddListener(Bigshine);
        }
    }

    // Update is called once per frame
    void Update()
    {

        ani.SetBool("OnWall", OnWall);
       
        if (pinky.IsDrawing && pinky.aimInsideMask)
        {
            ani.SetBool("Spray", true);
            //Debug.Log("spraytime fx");
        }
        else
        {
            ani.SetBool("Spray", false);
            
        }
    }
    void Bigshine()
    {
        Debug.Log("plingeling");
        ani.SetTrigger("FullyPainted");
    }
    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        //Debug.Log("PM" + OnWall);

    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }


        //play animation
        ani.Play(newState);
        StartCoroutine(ResetAnimatorState());
        currentState = newState;
        Debug.Log(newState);
    }
    IEnumerator ResetAnimatorState()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.1f);

        // Reset animator state
        ani.speed = 0;
        yield return null; // Yield at least one frame to apply the change
        ani.speed = 1;
    }
    bool isAnimationPlaying(Animator animatorm, string stateName)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
