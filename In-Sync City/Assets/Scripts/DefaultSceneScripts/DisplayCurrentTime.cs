using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This script handles the display of showing the current time, according to the time displayed on the user's system.
public class DisplayCurrentTime : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textbox;

    void Update()
    {
        DateTime currentTime = DateTime.Now;

        string formattedTime = currentTime.ToString("HH:mm:ss");

        textbox.text = formattedTime;
    }
}
