using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitToDesktopScript : MonoBehaviour
{
    [SerializeField] private GameObject exitDesktopContainer;
    [SerializeField] private Button noButton;
    [SerializeField] private Button confirmButton;

    public void OnNoClick()
    {
        exitDesktopContainer.SetActive(false);
    }

    public void OnConfirmClick()
    {
        Application.Quit();
    }
}
