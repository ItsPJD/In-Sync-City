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

//The set time method parses the inputted text for saving a time into string values. It then checks to make sure that the numbers
// inputted are within 0 - 23 for hours and 0 - 59 for minutes. It then makes this into a DateTime data type value, and saves it to
// the dateTime list. From there, it also sets the value of if its a work or relax time, and sends this information to the health script.
// It then also creates a new dateTime element.
    public void SetTime()
    {

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

// This method closes the menu for adding times.
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

// This method removes a time according to the gameObject passed in as a parameter. It checks what time was saved on that gameObject,
// and from there finds it in the dateTimeList. It then removes both that time and the gameObject from their respective lists, and updates
// the health script with this information.
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

            DateTime timeToRemove = DateTime.ParseExact(timeText, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            int indexOfRemoveTime = dateTimeListData.IndexOf(timeToRemove);
            dateTimeListWorkOrRelax.RemoveAt(indexOfRemoveTime);
            dateTimeListData.Remove(timeToRemove);

            dateTimeList.Remove(dateTimeElement);
            Destroy(dateTimeElement);

            healthScript.SetHealthTimeData(dateTimeListData);
            healthScript.SetHealthTimeWorkOrRelax(dateTimeListWorkOrRelax);
            healthScript.ActivateFindClosestDateTime();
        }

    }

// Tihs method is used to create all the GameObject versions of the times saved by the user. It goes through each time saved in the data list,
// and from there creates a gameObject using a prefab that is applied in the Unity Editor.
    private void CreateDateTimeUIElement(DateTime parsedTime)
    {

        dateTimePrefabTemplate.SetActive(true);
        GameObject dateTimeElement = Instantiate(dateTimePrefab, dateTimeContainer);
        dateTimePrefabTemplate.SetActive(false);


        TextMeshProUGUI textComponent = dateTimeElement.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = parsedTime.ToString("HH:mm");

        dateTimeList.Add(dateTimeElement);

        dateTimeList = dateTimeList.OrderBy(e => DateTime.Parse(e.GetComponentInChildren<TextMeshProUGUI>().text)).ToList();

        for (int i = 0; i < dateTimeList.Count; i++)
        {
            dateTimeList[i].transform.SetSiblingIndex(i);
        }
    }

// These methods get the values of the dateTime data, as well as the workOrRelax data. 
    public List<DateTime> GetDateTimeListData()
    {
        return this.dateTimeListData;
    }

    public List<float> GetWorkOrRelax()
    {
        return this.dateTimeListWorkOrRelax;
    }

// This method initialises the health time data, ensuring that it has the correct information at the start of the game.
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
