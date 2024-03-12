using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsScript : MonoBehaviour
{
    public Transform target;
    [SerializeField] public float speed = 1;
    [SerializeField] public float cap = 1;
    public bool dead = false;
    bool waitingToDestroy = false;
    private Vector2 velocity;
    void Update()
    {
        if (!dead)
        {
        Vector2 delta = transform.position - target.position;
        delta = delta.normalized * speed * Time.deltaTime;
        velocity += delta;
        if(velocity.magnitude > cap)
        {
            velocity = velocity.normalized * cap;
        }
        transform.position += (Vector3)(velocity*Time.deltaTime);
        }
        else 
        {
            if (!waitingToDestroy)
            {
                StartCoroutine(WaitAndDestroy());
                waitingToDestroy = true;
            }
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }
}
