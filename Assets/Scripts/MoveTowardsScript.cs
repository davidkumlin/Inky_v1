using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsScript : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float speed = 1;
    [SerializeField] private float cap = 1;

    private Vector2 velocity;
    void Update()
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

}
