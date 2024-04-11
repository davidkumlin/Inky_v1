using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI pointMeterText;
    [SerializeField] private Image healthBar;
    //Dialoge
    [SerializeField] public Image Inky;
    [SerializeField] public Image froggy;
    [SerializeField] public Image Yeti;
    [SerializeField] public Image Printa;
    [SerializeField] public TextMeshProUGUI dialogueText;

    private int totalPoints = 0;

    private P_Stats pstats;
    private P_Inky pinky;

    private void Start()
    {
        pstats = FindObjectOfType<P_Stats>();
        pinky = FindObjectOfType<P_Inky>();
        // Find all PaintableObjects in the scene and subscribe to their OnFullyBombed event
        PaintableObject[] paintableObjects = FindObjectsOfType<PaintableObject>();
        foreach (PaintableObject paintableObject in paintableObjects)
        {
            paintableObject.OnFullyBombed.AddListener(AddPoints);
        }
    }

    private void Update()
    {
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        // Update the health bar value based on the player's health
        if (pstats != null)
        {
            float healthPercentage = pstats.hp / pstats.maxHp;
            healthBar.fillAmount = healthPercentage;
        }
    }
    // Method to add points to the total and update the point meter UI
    private void AddPoints()
    {
        totalPoints = 0; // Reset total points before recalculating
        PaintableObject[] paintableObjects = FindObjectsOfType<PaintableObject>();
        foreach (PaintableObject paintableObject in paintableObjects)
        {
            if (paintableObject.fullyBombed)
            {
                totalPoints += paintableObject.originalPaintHP; // Add the original paintHP value
                
            }
        }
        pointMeterText.text = totalPoints.ToString();
    }

    //Froggy
    public void ShowFroggy()
    {
        froggy.gameObject.SetActive(true);
        Inky.gameObject.SetActive(true);
    }

    public void HideFroggy()
    {
        froggy.gameObject.SetActive(false);
        Inky.gameObject.SetActive(false);
    }
    //Yeti
    public void ShowYeti()
    {
        Yeti.gameObject.SetActive(true);
        Inky.gameObject.SetActive(true);
    }

    public void HideYeti()
    {
        Yeti.gameObject.SetActive(false);
        Inky.gameObject.SetActive(false);
    }
    //Printa
    public void ShowPrinta()
    {
        Printa.gameObject.SetActive(true);
        Inky.gameObject.SetActive(true);
    }

    public void HidePrinta()
    {
        Printa.gameObject.SetActive(false);
        Inky.gameObject.SetActive(false);
    }
    //Dialoge
    public void ShowDialogue(string text)
    {
        dialogueText.text = text;
        dialogueText.gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogueText.gameObject.SetActive(false);
        
    }
}