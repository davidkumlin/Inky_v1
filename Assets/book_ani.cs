using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class book_ani : MonoBehaviour
{
    public Animator animator;
    private string currentState;
    public portal_ani portal;
    public elf_ani elf;
    private bool bookTaken = false;
    public BoxCollider2D bookcollider;

    const string book_in = "book_in";
    const string book_idle = "book_idle";
    const string book_out = "book_out";
    const string fullypainted = "fullypainted";
    void Start()
    {
        FindObjectOfType<elf_ani>();
        FindObjectOfType<portal_ani>();
      
        animator = GetComponent<Animator>();
       
    }

    public void BookIn()
    {
        ChangeAnimationState(book_in);
    }

    private void activateColl()
    {
        bookcollider.enabled = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!bookTaken)
        {
            elf.elfExit();
            bookTaken = true;
            ChangeAnimationState(fullypainted);
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
        Destroy(gameObject);
    }
}
