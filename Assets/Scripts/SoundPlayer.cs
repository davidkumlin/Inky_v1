using UnityEngine;
using FMODUnity;

public class SoundPlayer : MonoBehaviour
{
    //private SoundPlayer soundPlayer;
    //in start  soundPlayer = GetComponentInChildren<SoundPlayer>();
    [SerializeField]
    private StudioEventEmitter[] eventEmitters;

    void Start()
    {
        if (eventEmitters == null || eventEmitters.Length == 0)
        {
            Debug.LogError("No StudioEventEmitters assigned. Please assign them in the inspector.");
        }
    }

    // Play sound by index
    public void PlaySound(int index)
    {
        if (index >= 0 && index < eventEmitters.Length)
        {
            eventEmitters[index].Play();
        }
        else
        {
            Debug.LogError("Index out of range. Make sure the index corresponds to a sound in the list.");
        }
    }
    // Stop sound by index
    public void StopSound(int index)
    {
        if (index >= 0 && index < eventEmitters.Length)
        {
            eventEmitters[index].Stop();
        }
        else
        {
            Debug.LogError("Index out of range. Make sure the index corresponds to a sound in the list.");
        }
    }
}
