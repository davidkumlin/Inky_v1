using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStart : MonoBehaviour
{
    private drivebycar DriveByCar;
    // Start is called before the first frame update
    void Start()
    {
        DriveByCar = FindObjectOfType<drivebycar>();

        if (DriveByCar == null)
        {
            Debug.LogError("Car not found");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DriveByCar.carstarted = true;
        }
    }
}
