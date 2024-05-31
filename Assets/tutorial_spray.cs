using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_spray : MonoBehaviour

{ 
 [SerializeField] public GameObject spray_tut;

private BoxCollider2D spraytriggerbox;
// Start is called before the first frame update
void Start()
{
    spraytriggerbox = GetComponent<BoxCollider2D>();
}

private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spray_tut.SetActive(true);
            Debug.Log("spray_tut");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
{
        if (collision.gameObject.CompareTag("Player"))
        {
            spray_tut.SetActive(false);
            Destroy();
        }
}
void Destroy()
{
    Destroy(gameObject);
}
}