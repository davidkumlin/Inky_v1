using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim_fx : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    public bool OnWall { get; private set; } = false;
    private P_Inky pinky;
    const string BigSplat = "BigSplat";
    const string Painting = "Painting";
    const string BigDrips = "BigDrips";
    const string FullyPainted = "FullyPainted";
    private inky_animation inkyani;
    private string currentState;

    [SerializeField] private Sprite cross_idle;
    [SerializeField] private Sprite cross_OW; //when over sprayed line and can go Onwall
    [SerializeField] private Sprite cross_PO; //when over paintable object and can spray

    void Start()
    {
        inkyani = FindObjectOfType<inky_animation>();
        if (inkyani == null)
        {
            Debug.LogError("inkyani component not found");
        }
        pinky = FindObjectOfType<P_Inky>();
        if (pinky == null)
        {
            Debug.LogError("pinky component not found");
        }
        GameManager.OnWallChanged += OnWallStatus;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        PaintableObject[] paintableObjects = FindObjectsOfType<PaintableObject>();
        foreach (PaintableObject paintableObject in paintableObjects)
        {
            paintableObject.OnFullyBombed.AddListener(Bigshine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("OnWall", OnWall);

        if (pinky.IsDrawing && pinky.aimInsideMask)
        {
            ani.SetBool("Spray", true);
            // Debug.Log("spraytime fx");
        }
        else
        {
            ani.SetBool("Spray", false);
        }

        UpdateCrosshairSprite();
    }

    void Bigshine()
    {
        Debug.Log("plingeling");
        ani.SetTrigger("FullyPainted");
    }

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
        // Debug.Log("PM" + OnWall);
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        // play animation
        ani.Play(newState);
        StartCoroutine(ResetAnimatorState());
        currentState = newState;
        Debug.Log(newState);
    }

    IEnumerator ResetAnimatorState()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.1f);

        // Reset animator state
        ani.speed = 0;
        yield return null; // Yield at least one frame to apply the change
        ani.speed = 1;
    }

    bool isAnimationPlaying(Animator animatorm, string stateName)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateCrosshairSprite()
    {
        if (pinky == null || pinky.ActiveWall == null) return;

        Vector2 aimPos = pinky.CurrentAim;
        bool isOverPaintableObject = false;
        bool isOverSprayedLine = false;

        // Check if the aim is over any paintable object
        PaintableObject[] paintableObjects = FindObjectsOfType<PaintableObject>();
        foreach (PaintableObject paintableObject in paintableObjects)
        {
            if (paintableObject.IsAimInsideSpriteMask(aimPos))
            {
                isOverPaintableObject = true;
                Debug.Log("Aim is over a paintable object.");
                break;
            }
        }

        // Check if the aim is over any sprayed line
        if (pinky.ActiveWall != null)
        {
            foreach (Line line in pinky.ActiveWall.SprayedLines)
            {
                foreach (Vector2 point in line.SprayedPoints)
                {
                    if (Vector2.Distance(point, aimPos) <= DrawManager_2.RESOLUTION)
                    {
                        isOverSprayedLine = true;
                        Debug.Log("Aim is over a sprayed line.");
                        break;
                    }
                }
                if (isOverSprayedLine) break;
            }
        }

        // Update the crosshair sprite using animator parameters
        if (isOverPaintableObject)
        {
            ani.SetBool("IsOverPaintableObject", true);
            ani.SetBool("IsOverSprayedLine", false);
            ani.SetBool("IsIdle", false);
            Debug.Log("Crosshair set to cross_PO.");
        }
        else if (isOverSprayedLine)
        {
            ani.SetBool("IsOverPaintableObject", true);
            ani.SetBool("IsOverSprayedLine", true);
            ani.SetBool("IsIdle", false);
            Debug.Log("Crosshair set to cross_OW.");
        }
        else
        {
            ani.SetBool("IsOverPaintableObject", false);
            ani.SetBool("IsOverSprayedLine", false);
            ani.SetBool("IsIdle", true);
            Debug.Log("Crosshair set to cross_idle.");
        }
    }
}
