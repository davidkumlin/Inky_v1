using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Version 08/02 V1
    // Singleton instance
    public static GameManager Instance { get; private set; }

    
    //For the ON WALL mechanics
    [SerializeField] public bool OnWall 
    { 
        get=>onWall; 
        set 
        { 
            onWall = value;
            OnWallChanged?.Invoke(onWall); 
        } 
    }
    [SerializeField] private bool onWall;
    public delegate void OnBoolChanged(bool newValue); 
    public static event OnBoolChanged OnWallChanged;
    [SerializeField] public bool InHiding { get; private set; } = false;
    [SerializeField] public bool Alerted { get; private set; } = false;

   
    


    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
   
}
