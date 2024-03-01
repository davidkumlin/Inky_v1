using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim_fx : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    public bool OnWall { get; private set; } = false;

    private string currentState;
    
    const string BigSplat = "BigSplat";
    const string BigDrips = "BigDrips";
    

    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("OnWall", OnWall);

        
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
