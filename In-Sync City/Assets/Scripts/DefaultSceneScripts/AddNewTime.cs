using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This script handles the toggle of the menu panel for adding a new time to the schedule.
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
