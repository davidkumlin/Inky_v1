using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI pointMeterText;
    [SerializeField] private Image healthBar;

    private int totalPoints = 0;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
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
        if (playerMovement != null)
        {
            float healthPercentage = playerMovement.hp / playerMovement.maxHp;
            healthBar.fillAmount = healthPercentage;
        }
    }
    // Method to add points to the total and update the point meter UI
    private void AddPoints()
    {
        PaintableObject[] paintableObjects = FindObjectsOfType<PaintableObject>();
        foreach (PaintableObject paintableObject in paintableObjects)
        {
            if (paintableObject.fullyBombed)
            {
                totalPoints += paintableObject.originalPaintHP; // Add the original paintHP value
            }
        }
        pointMeterText.text = "Points: " + totalPoints.ToString();
    }
}