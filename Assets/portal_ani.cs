using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal_ani : MonoBehaviour
{
    public yodasoda yodasoda;
    public elf_ani elf;
    public Animator animator;
    private string currentState;
    private bool hadSoda = false;
    private bool portalActive = false;
    public bool elftime = false;
    const string portal_in = "portal_in";
    const string portal_idle = "portal_idle";
    const string portal_out = "portal_out";
    const string empty = "empty";
    void Start()
    {
        FindObjectOfType<book_ani>();
        FindObjectOfType<elf_ani>();
        FindObjectOfType<yodasoda>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hadSoda)
        {
            sodacheck();
        }
        

    }

    void sodacheck()
    {
        if (yodasoda.Kloak == true)
        {
            if (!hadSoda)
            {
            activatePortal();
            hadSoda = true;
            }
            
        }
    }

    void activatePortal()
    {
        ChangeAnimationState(portal_in);
    }

    void portalToelfIn()
    {
        elf.elfIn();
        elftime = true;
    }
private bool closetheportal = false;
    public void closePortal()
    {
        if (!closetheportal)
        {
            ChangeAnimationState(portal_out);
            closetheportal = true;
        }
        
    }

    
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }


        //play animation
        animator.Play(newState);

        currentState = newState;
        //Debug.Log(newState);
    }
    void LetsDestroy()
    {
        ChangeAnimationState(empty);
    }
}

