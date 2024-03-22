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
    [SerializeField] private int playerHealth = 10;
    private bool timerStarted = false;
    private bool calledTimer = false;
    private float startTimer;
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private TextMeshProUGUI healthText;
    public UIMenu uiMenu;
    private List<DateTime> healthTimeData;
    private List<float> healthTimeWorkOrRelax;
    [SerializeField] private GameObject timeButton;
    public CurrencyScript currencyScript;

    [SerializeField] private GameObject workObject;

    [SerializeField] private GameObject relaxObject;

    [SerializeField] private GameObject deathPanel;

    private int heartgemIncrease = 3;

// The awake method here makes sure that all buttons and objects related to health, as well as the work and relax objects are set to their default.
    private void Awake()
    {
        timeButton.SetActive(false);
        healthText.text = playerHealth.ToString();
        workObject.SetActive(false);
        relaxObject.SetActive(false);
    }

// The health script gets its own, updated versions of the times saved by the user, so they can be compared to the current time. This also helps to set the values
// of the work and relax objects, as to which one should be active.
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

        deathPanel.SetActive(false);
    }

// The update method checks if any of the times saved by the user matches the current time. If it does, then the timer for the health button is activated.
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

// This method is called when the user clicks the health button before the timer expires. This grants them health as well as heartgems. It also
// ensures that the player does not exceed 10 health.
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

// This method is called if the timer expires before the user clicks the health button. One health is removed from the player, and the button is deactivated.
    public void timeButtonPressFailure()
    {
        playerHealth -= 1;
        timeButton.SetActive(false);
        timerStarted = false;
    }

// This method starts the timer for the health button.
    public void ActivateTimer()
    {
        timeButton.SetActive(true);
        startTimer = Time.realtimeSinceStartup;
        timerStarted = true;       
    }

// These methods set the data for the health versions of the saved times and whether they are working or relaxing. These
// get called whenever the time data is updated, such as when a user adds or removes a time.
    public void SetHealthTimeData(List<DateTime> newDateTimeData)
    {
        healthTimeData = newDateTimeData;
    }

    public void SetHealthTimeWorkOrRelax(List<float> newDateTimeWorkOrRelax)
    {
        healthTimeWorkOrRelax = newDateTimeWorkOrRelax;
    }

// This method is called in order to start the searcch for finding the closest time.
    public void ActivateFindClosestDateTime()
    {
        FindClosestDateTime(healthTimeData);
    }

//This method determines whether the work or relax object should be active and which one should not be.
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

//This method is used to determine which saved time is "closest" to the current time, in terms of which saved time was the last one that matched. This helps
// to decide which object, work or relax, should be active at the current time.
    private void FindClosestDateTime(List<DateTime> dateTimeList)
    {
        DateTime currentTime = DateTime.Now;

        DateTime closestDateTime = dateTimeList.Where(time => time <= currentTime).OrderBy(dt => Math.Abs((dt - currentTime).TotalMinutes)).FirstOrDefault();
        Debug.Log("Closest DateTime - " + closestDateTime);

        int indexValueOfClosestTime = healthTimeData.IndexOf(closestDateTime);

        if(indexValueOfClosestTime <= -1)
        {
            float lastDateTimeWorkOrRelax = healthTimeWorkOrRelax.Last();
            WorkOrRelax(lastDateTimeWorkOrRelax);
        }

        else
        {
            float closestTimeWorkOrRelax = healthTimeWorkOrRelax[indexValueOfClosestTime];
            WorkOrRelax(closestTimeWorkOrRelax);
        }

    }

    //This method is called when the player's health reaches 0.

    public void OnDeath()
    {
        Debug.Log("Player has died!");
        deathPanel.SetActive(true);

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
