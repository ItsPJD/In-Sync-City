using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCurrentTime : MonoBehaviour
{

    public TextMeshProUGUI textbox;

    void Update()
    {
        DateTime currentTime = DateTime.Now;

        string formattedTime = currentTime.ToString("HH:mm:ss");

        textbox.text = formattedTime;
    }
}
