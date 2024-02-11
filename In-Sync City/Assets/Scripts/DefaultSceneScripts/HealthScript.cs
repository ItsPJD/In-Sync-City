using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour, IDataPersistence
{
    private int playerHealth = 10;
    private bool timerStarted = false;
    private bool calledTimer = false;
    private float startTimer;
    public float timeLimit = 60f;
    public TextMeshProUGUI healthText;
    public UIMenu uiMenu;
    private List<DateTime> healthTimeData;
    private List<float> healthTimeWorkOrRelax;
    public GameObject timeButton;
    public CurrencyScript currencyScript;

    public GameObject workObject;

    public GameObject relaxObject;

    private int heartgemIncrease = 3;

    private DateTime matchedTime;


    private void Awake()
    {
        timeButton.SetActive(false);
        healthText.text = playerHealth.ToString();
        workObject.SetActive(false);
        relaxObject.SetActive(false);
    }

    public void StartInitialize()
    {
        healthTimeData = uiMenu.GetDateTimeListData();
        healthTimeWorkOrRelax = uiMenu.GetWorkOrRelax();

        // Validate the obtained lists
        if (healthTimeData != null && healthTimeWorkOrRelax != null && healthTimeData.Count > 0 && healthTimeData.Count == healthTimeWorkOrRelax.Count)
        {
            FindClosestDateTime(healthTimeData);
        }

        else
        {
            Debug.Log("Invalid or empty data obtained from uiMenu.");
        }
    }

    private void Update()
    {
        healthText.text = playerHealth.ToString();

        DateTime checkingTimeNow = DateTime.Now;

        if(healthTimeData.Any(time => time.Hour == checkingTimeNow.Hour && time.Minute == checkingTimeNow.Minute))
        {

            FindClosestDateTime(healthTimeData);

            if(!calledTimer)
            {
                ActivateTimer();
                calledTimer = true;
            }
        }

        else
        {
            calledTimer = false;
        }

        if(timerStarted)
        {
            float elapsedTime = Time.realtimeSinceStartup - startTimer;

            if(elapsedTime >= timeLimit)
            {
                Debug.Log("Time button was not pressed in time!");
                timeButtonPressFailure();
            }
        }

        if(playerHealth <= 0)
        {
            OnDeath();
        }
    }
    public void OnTimeButtonPress()
    {
        playerHealth += 1;
        currencyScript.AddHeartgems(heartgemIncrease);
        timeButton.SetActive(false);
        timerStarted = false;

        if(playerHealth >= 10)
        {
            playerHealth = 10;
        }

    }

    public void timeButtonPressFailure()
    {
        playerHealth -= 1;
        timeButton.SetActive(false);
        timerStarted = false;
    }

    public void ActivateTimer()
    {
        timeButton.SetActive(true);
        startTimer = Time.realtimeSinceStartup;
        timerStarted = true;       
    }

    public void SetHealthTimeData(List<DateTime> newDateTimeData)
    {
        healthTimeData = newDateTimeData;
    }

    public void SetHealthTimeWorkOrRelax(List<float> newDateTimeWorkOrRelax)
    {
        healthTimeWorkOrRelax = newDateTimeWorkOrRelax;
    }

    public void ActivateFindClosestDateTime()
    {
        FindClosestDateTime(healthTimeData);
    }

    private void WorkOrRelax(float workOrRelaxValue)
    {
        if(workOrRelaxValue == 1)
        {
            workObject.SetActive(true);
            relaxObject.SetActive(false);
        }

        else
        {
            workObject.SetActive(false);
            relaxObject.SetActive(true);
        }
    }

    private void FindClosestDateTime(List<DateTime> dateTimeList)
    {
        DateTime currentTime = DateTime.Now;

        DateTime closestDateTime = dateTimeList.Where(time => time <= currentTime).OrderBy(dt => Math.Abs((dt - currentTime).TotalMinutes)).FirstOrDefault();
        Debug.Log("Closest DateTime - " + closestDateTime);

        int indexValueOfClosestTime = healthTimeData.IndexOf(closestDateTime);

        // If indexValueOfClosestTime returns -1, it means that it did not find a time that existed in the list that matched the closestDateTime, which will typically be the "Default" 00:00.
        if(indexValueOfClosestTime <= -1)
        {
            //If the first time of the day has not been reached once the time reaches 00:00, it will default the work or relax state to the last time in the list, as this will be the one that is continued until the next day.
            float lastDateTimeWorkOrRelax = healthTimeWorkOrRelax.Last();
            WorkOrRelax(lastDateTimeWorkOrRelax);
        }

        else
        {
            float closestTimeWorkOrRelax = healthTimeWorkOrRelax[indexValueOfClosestTime];
            WorkOrRelax(closestTimeWorkOrRelax);
        }

    }

    public void OnDeath()
    {
        Debug.Log("Player has died!");
    }

    public void LoadData(GameData data)
    {
        this.playerHealth = data.playerHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.playerHealth = this.playerHealth;
    }
}
