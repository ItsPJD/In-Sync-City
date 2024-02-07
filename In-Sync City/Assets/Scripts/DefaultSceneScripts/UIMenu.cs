using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System.Xml;
using Unity.VisualScripting;
using System.Linq;
using System.IO;

public class UIMenu : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TMP_InputField hoursInput;
    [SerializeField] private TMP_InputField minutesInput;
    [SerializeField] private GameObject dateTimePrefab;
    [SerializeField] private Transform dateTimeContainer;

    [SerializeField] private GameObject dateTimePrefabTemplate;

    [SerializeField] private int minTimes = 4;
    [SerializeField] private int maxTimes = 10;

    [SerializeField] private TextMeshProUGUI errorMessage;

    [SerializeField] private HealthScript healthScript;
    [SerializeField] private Slider workOrRelaxSlider;

    private List<GameObject> dateTimeList = new List<GameObject>();

    private List<DateTime> dateTimeListData = new List<DateTime>();

    private List<float> dateTimeListWorkOrRelax = new List<float>();

    private bool sameTime = false;


    private void Awake()
    {
        dateTimePrefabTemplate.SetActive(false);
    }

    public void SetTime()
    {

        // if statement for checking max amount of times to be added. max amount of times = 10 (for now). Also make min amount of times, no less than two times on a schedule.
        int listCount = dateTimeListData.Count;

        if (listCount >= maxTimes)
        {
            Debug.Log("Maximum amount of times added.");
        }

        else{
            int hours = int.Parse(hoursInput.text);
            int minutes = int.Parse(minutesInput.text);

            float sliderValue = workOrRelaxSlider.value;

            if (hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59)
            {
                
                // Calculate and display the time
                string time = $"{hours:D2}:{minutes:D2}";
                DateTime parsedTime = DateTime.Parse(time);

                foreach(DateTime times in dateTimeListData)
                {
                    if (parsedTime == times)
                    {
                        sameTime = true;
                    }

                }

                if (sameTime)
                {
                    Debug.Log("That time has already been added");
                    errorMessage.text = ("That time has already been added"); 
                    sameTime = false;
                }

                else
                {
                    dateTimeListData.Add(parsedTime);
                    dateTimeListData.Sort();
                    int indexOfTime = dateTimeListData.IndexOf(parsedTime);
                    dateTimeListWorkOrRelax.Insert(indexOfTime, sliderValue);

                    CreateDateTimeUIElement(parsedTime);

                    healthScript.SetHealthTimeData(dateTimeListData);
                    healthScript.SetHealthTimeWorkOrRelax(dateTimeListWorkOrRelax);
                    healthScript.ActivateFindClosestDateTime();
                    Debug.Log("The Time was accepted");
                }
            }

            else
            {
                Debug.Log("Incorrect Input");
                errorMessage.text = ("Incorrect Input");
            }
        }
    }

    public void CloseMenu()
    {
        // Hide the menu panel
        gameObject.SetActive(false);
    }

    public void RemoveTime(GameObject dateTimeElement)
    {
        int listCount = dateTimeListData.Count;

        if(listCount <= minTimes)
        {
            Debug.Log("You have to have a minimum of 4 times!");
        }

        else
        {
            TextMeshProUGUI textComponent = dateTimeElement.GetComponentInChildren<TextMeshProUGUI>();
            string timeText = textComponent.text;

            // Find and remove the corresponding DateTime from the list
            DateTime timeToRemove = DateTime.ParseExact(timeText, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            int indexOfRemoveTime = dateTimeListData.IndexOf(timeToRemove);
            dateTimeListWorkOrRelax.RemoveAt(indexOfRemoveTime);
            dateTimeListData.Remove(timeToRemove);

            // Remove the UI element from the list and destroy the GameObject
            dateTimeList.Remove(dateTimeElement);
            Destroy(dateTimeElement);

            healthScript.SetHealthTimeData(dateTimeListData);
            healthScript.SetHealthTimeWorkOrRelax(dateTimeListWorkOrRelax);
            healthScript.ActivateFindClosestDateTime();
        }

    }

    private void CreateDateTimeUIElement(DateTime parsedTime)
    {

        dateTimePrefabTemplate.SetActive(true);
        GameObject dateTimeElement = Instantiate(dateTimePrefab, dateTimeContainer);
        dateTimePrefabTemplate.SetActive(false);


        TextMeshProUGUI textComponent = dateTimeElement.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = parsedTime.ToString("HH:mm");

        dateTimeList.Add(dateTimeElement);

        // Sort dateTimeUIElements based on the time of day
        dateTimeList = dateTimeList.OrderBy(e => DateTime.Parse(e.GetComponentInChildren<TextMeshProUGUI>().text)).ToList();

        // Rearrange UI elements
        for (int i = 0; i < dateTimeList.Count; i++)
        {
            dateTimeList[i].transform.SetSiblingIndex(i);
        }
    }

    public List<DateTime> GetDateTimeListData()
    {
        return this.dateTimeListData;
    }

    public List<float> GetWorkOrRelax()
    {
        return this.dateTimeListWorkOrRelax;
    }

    public void InitialiseHealthScriptData()
    {
        healthScript.StartInitialize();
    }

    public void LoadData(GameData data)
    {
        this.dateTimeListData = data.dateTimeListData.Select(str => DateTime.ParseExact(str, "HH:mm", System.Globalization.CultureInfo.InvariantCulture)).ToList();

        foreach (var dateTime in dateTimeListData)
        {
            CreateDateTimeUIElement(dateTime);
        }

        this.dateTimeListWorkOrRelax = data.dateTimeWorkOrRelax;
        InitialiseHealthScriptData();
    }

    public void SaveData(ref GameData data)
    {
        data.dateTimeListData = this.dateTimeListData.Select(dt => dt.ToString("HH:mm")).ToList();
        data.dateTimeWorkOrRelax = this.dateTimeListWorkOrRelax;
    }

}
