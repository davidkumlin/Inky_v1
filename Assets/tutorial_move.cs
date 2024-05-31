using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_move : MonoBehaviour
{
    [SerializeField] public GameObject Move_tut;

    private BoxCollider2D triggerbox;
    // Start is called before the first frame update
    void Start()
    {
        triggerbox = GetComponent<BoxCollider2D>();
    }

   
    private void OnTriggerExit2D(Collider2D collision)
    {
        Move_tut.SetActive(false);
        Destroy();
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
