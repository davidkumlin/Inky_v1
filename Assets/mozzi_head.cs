using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mozzi_head : MonoBehaviour
{
    [SerializeField] private Animator headAnimator;
    [SerializeField] private BoxCollider2D triggerbox;
    private mozzi_resi mozzi;
    private bool readytomozz = false;
    private bool hasAtk = false;
    
    private float Damage = 34f;
    [SerializeField] private P_Stats pstats;

    private string currentState;
    const string mozzi_in = "mozzi_in";
    const string mozzi_idle = "mozzi_idle";
    const string mozzi_atk = "mozzi_atk";
    
    // Start is called before the first frame update
    void Start()
    {
        triggerbox = GetComponent<BoxCollider2D>();
        headAnimator = GetComponent<Animator>();
        mozzi = FindObjectOfType<mozzi_resi>();
    }
    void resetAtk()
    {
        hasAtk = false;
        ChangeheadAnimationState(mozzi_idle);
    }
    private void Update()
    {
        if (mozzi.mozzyready&& !readytomozz)
        {
            ChangeheadAnimationState(mozzi_in);
            readytomozz = true;
        }
    }
    void ChangeheadAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }


        //play animation
        headAnimator.Play(newState);

        currentState = newState;
        //Debug.Log(newState);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasAtk)
        {

            if (readytomozz)
            {

           
            hasAtk = true;
            Debug.Log("bzzzz");
            ChangeheadAnimationState(mozzi_atk);
             }


        }
    }
    void ATK()
    {
       
        pstats.hp -= Damage;
        pstats.Damage();
    }


}
