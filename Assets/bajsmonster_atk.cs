using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bajsmonster_atk : MonoBehaviour
{
    [SerializeField] private P_Stats pstats;
    [SerializeField] private CircleCollider2D hitbox;
    private bool HasAtk = false;
    [SerializeField] private float Damage;
    // Start is called before the first frame update
    void Start()
    {
        pstats = FindObjectOfType<P_Stats>();
        if (pstats == null)
        {
            Debug.LogError("PStats not found");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("in trigger");


            if (!HasAtk)
            {
         pstats.Damage(Damage);
                    Debug.Log("attack with hitbox");
                HasAtk = true;
            }
           



        }
    }
   
}
