using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elf_ani : MonoBehaviour
{
    public Animator animator;
    private string currentState;
    public portal_ani portal;
    public book_ani book;
    public bool elfInDaHouse = false;
    private bool elfActive = false;

    const string elf_in = "elf_in";
    const string elf_idle = "elf_idle";
    const string elf_out = "elf_out";
    const string empty = "empty";
    void Start()
    {
        FindObjectOfType<book_ani>();
        FindObjectOfType<portal_ani>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void elfIn()
    {
        if (!elfInDaHouse)
        {
            ChangeAnimationState(elf_in);
            elfInDaHouse = true;
        }
        
    }

    void elfToportalout()
    {
        Debug.Log("elf close portal and get book");
        portal.closePortal();
        book.BookIn();
    }

    public void elfExit()
    {
        ChangeAnimationState(elf_out);
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
        Destroy(gameObject);
    }
}
