using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script provides similar functionality to the pause menu script, except for the main menu.
public class MainMenuExitScript : MonoBehaviour
{
    public GameObject quitPanel;
    private bool isQuit;

    private void Awake()
    {
        quitPanel.SetActive(false);
        isQuit = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HandleIsQuit();
        }

    }

    public void OnEnterQuit()
    {
        quitPanel.SetActive(true);
        isQuit = true;
    }

    public void OnExitQuit()
    {
        quitPanel.SetActive(false);
        isQuit = false;
    }

    private void HandleIsQuit()
    {
        if(isQuit)
        {
            OnExitQuit();
        }

        else
        {
            OnEnterQuit();
        }
    }
}
