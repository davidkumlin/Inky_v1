using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausMeny : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public Slider aimSpeedSlider;
    public Slider smoothingSlider;

    private AimMovement aimMovement;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;

        aimMovement = FindObjectOfType<AimMovement>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                Debug.Log(GameIsPaused);
            }
            else
            {
                Pause();
                Debug.Log(GameIsPaused);
            }
        }
    }
    void TogglePauseMenu()
    {
        if (GameIsPaused)
        {
            Resume();
            Debug.Log(GameIsPaused);
        }
        else
        {
            Pause();
            Debug.Log(GameIsPaused);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Set the initial values of the sliders based on current values from AimMovement
        if (aimMovement != null)
        {
            // Set the value of the aimSpeedSlider to the current value of aimspeed
            aimSpeedSlider.value = aimMovement.aimspeed;

            // Set the value of the smoothingSlider to the current value of smoothing
            smoothingSlider.value = aimMovement.smoothing;
        }
    }

    // This method is called when the aimspeed slider value changes
    public void OnAimSpeedSliderChanged(float newValue)
    {
        if (aimMovement != null)
        {
            aimMovement.UpdateAimSpeed(newValue);
        }
    }

    // This method is called when the smoothing slider value changes
    public void OnSmoothingSliderChanged(float newValue)
    {
        if (aimMovement != null)
        {
            aimMovement.UpdateSmoothing(newValue);
        }
    }
}
