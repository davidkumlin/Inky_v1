using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_box : MonoBehaviour
{

    private P_Stats P_Stats;
    // Start is called before the first frame update
    void Start()
    {
        P_Stats = FindObjectOfType<P_Stats>();
        if (P_Stats == null)
        {
            Debug.Log("ingen pstats");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        P_Stats.ResetLevel();
    }
}
