using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseContainer;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitToMenuButton;
    [SerializeField] private Button exitToDesktopButton;
    [SerializeField] private GameObject exitToMenuContainer;
    [SerializeField] private GameObject exitToDesktopContainer;

    private bool isPaused;

    private void Awake()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

// If the escape button is pressed on the user's keyboard, the toggle for activating and deactivating the pause menu should happen.
// The update method also makes sure that if the menu or desktop containers are not active, then it should default the pause menu to its default
// position.
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HandleIsPaused();
        }

        if(!exitToMenuContainer.activeSelf && !exitToDesktopContainer.activeSelf)
        {
            DefaultPause();
        }
    }

// These methods handle the entering and exiting of the pause menu.
    public void OnEnterPause()
    {
        pausePanel.SetActive(true);
        DefaultPause();
        isPaused = true;
    }

    public void OnExitPause()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

//This method handles the activation of the container for confirming whether to exit to the main menu.
    public void OnMainMenuClick()
    {
        pauseContainer.SetActive(false);
        exitToMenuContainer.SetActive(true);
    }

//This method handles the activation of the container for confirming whether to exit to the desktop.
    public void OnDesktopClick()
    {
        pauseContainer.SetActive(false);
        exitToDesktopContainer.SetActive(true);
    }

//This method handles the toggle of whether the pause menu should be open or closed.
    private void HandleIsPaused()
    {
        if(isPaused)
        {
            OnExitPause();
        }

        else
        {
            OnEnterPause();
        }
    }

//This method is provided as a default of the pause menu, if the user refuses to exit to either the main menu or the desktop.
    private void DefaultPause()
    {
        pauseContainer.SetActive(true);
        exitToMenuContainer.SetActive(false);
        exitToDesktopContainer.SetActive(false);
    }
}
