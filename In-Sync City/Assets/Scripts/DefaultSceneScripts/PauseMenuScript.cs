using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseContainer;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitToMenuButton;
    [SerializeField] private Button exitToDesktopButton;
    [SerializeField] private GameObject exitToMenuContainer;
    [SerializeField] private GameObject exitToDesktopContainer;
    [SerializeField] private GameObject settingsContainer;
    private bool isPaused;

    private void Awake()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HandleIsPaused();
        }

        if(!exitToMenuContainer.activeSelf && !exitToDesktopContainer.activeSelf && !settingsContainer.activeSelf)
        {
            DefaultPause();
        }
    }

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

    public void OnSettingsClick()
    {
        pauseContainer.SetActive(false);
        settingsContainer.SetActive(true);
    }

    public void OnMainMenuClick()
    {
        pauseContainer.SetActive(false);
        exitToMenuContainer.SetActive(true);
    }

    public void OnDesktopClick()
    {
        pauseContainer.SetActive(false);
        exitToDesktopContainer.SetActive(true);
    }

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

    private void DefaultPause()
    {
        pauseContainer.SetActive(true);
        exitToMenuContainer.SetActive(false);
        exitToDesktopContainer.SetActive(false);
        settingsContainer.SetActive(false);
    }
}
