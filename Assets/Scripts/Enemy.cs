using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string test;
    protected float speed;
    PlayerMovement playref = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log("enemy start" + speed);

    }

    // Update is called once per frame
    void Update()
    {
        if (playref && Vector2.Distance(playref.transform.position, transform.position)<10)
        {
            Chase();
        }
    }

    protected void Chase()
    {
        Debug.Log("chasing");
        Attack();
    }

    protected abstract void Attack();
}


