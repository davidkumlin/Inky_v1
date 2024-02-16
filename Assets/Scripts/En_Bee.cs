using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class En_Bee : Enemy
{
    public string test2;

    protected override void Attack()
    {
        Debug.Log("Attack");
    }

    protected override void Start()
    {
        speed = 10;
        Debug.Log("bee start");
        base.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Chase();
    }
}

