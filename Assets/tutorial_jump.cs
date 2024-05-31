using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_jump : MonoBehaviour
{
    [SerializeField] public GameObject jump_tut;

    private BoxCollider2D triggerbox;
    // Start is called before the first frame update
    void Start()
    {
        triggerbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jump_tut.SetActive(true);
        }
        }
        private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jump_tut.SetActive(false);
            Destroy();
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
