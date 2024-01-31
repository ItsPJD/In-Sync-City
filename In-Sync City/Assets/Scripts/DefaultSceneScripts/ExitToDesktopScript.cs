using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitToDesktopScript : MonoBehaviour
{
    public GameObject exitDesktopContainer;
    public Button noButton;
    public Button confirmButton;

    public void OnNoClick()
    {
        exitDesktopContainer.SetActive(false);
    }

    public void OnConfirmClick()
    {
        Application.Quit();
    }
}
