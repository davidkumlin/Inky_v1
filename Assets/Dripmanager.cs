using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dripmanager : MonoBehaviour
{
    [SerializeField] GameObject dripPrefab;
    [SerializeField] GameObject OnwalldripPrefab;
    [SerializeField] GameObject SpraydripPrefab;
    private P_Inky pinky;
   
    
    //Drip
    private bool dripCoroutineRunning = false;


    public float minSpawnInterval = 0.5f; // Minimum time interval between drips
    public float maxSpawnInterval = 3f; // Maximum time interval between drips

    private void Start()
    {
        pinky = FindObjectOfType<P_Inky>();
        
        if (pinky == null)
        {
            Debug.Log("pinky is null");
        }
       
    }

    void FixedUpdate()
    {
        if (pinky.IsDrawing)
        {
            maxSpawnInterval = 3f;
            dripPrefab = SpraydripPrefab;
        }
        if (pinky.OnWall)
        {
            maxSpawnInterval = 1f;
            dripPrefab = OnwalldripPrefab;
        }
        // Check if pinky is not null and is drawing
        if (pinky != null && pinky.IsDrawing || pinky.OnWall)
        {
            // Check if the drip coroutine is not already running
            if (!dripCoroutineRunning)
            {
                // Start the coroutine
                StartCoroutine(SpawnDrips());
            }
        }
    }

    IEnumerator SpawnDrips()
    {
        // Set the flag to true to indicate that the coroutine is running
        dripCoroutineRunning = true;

        while (pinky != null && pinky.IsDrawing || pinky.OnWall)
        {
            // Calculate random spawn interval
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            // Spawn a drip at the current position
            Instantiate(dripPrefab, pinky.CurrentAim, Quaternion.identity);

            // Wait for the specified duration before spawning the next drip
            yield return new WaitForSeconds(spawnInterval);
        }

        // Reset the flag to indicate that the coroutine has finished
        dripCoroutineRunning = false;
    }
}