using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadBox : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private P_Stats p_Stats;

    void Start()
    {
        p_Stats = FindObjectOfType<P_Stats>();
        if (p_Stats == null)
        {
            Debug.Log("No P_Stats found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            p_Stats.LoadScene(sceneName);
        }
    }
}
