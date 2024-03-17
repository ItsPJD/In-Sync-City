using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddNewTime : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }
}
