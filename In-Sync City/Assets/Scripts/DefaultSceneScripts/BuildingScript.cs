using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BuildingScript : MonoBehaviour
{
    private float timeSinceLastCurrency;
    public float currencyInterval = 1f;

    public UpgradeScript upgrade;

    public CurrencyScript overallCurrency;

    public float getCurrencyInterval()
    {
        return currencyInterval;
    }

    void Start()
    {
        timeSinceLastCurrency = Time.realtimeSinceStartup;
    }
    void Update()
    {

        long addedCurrency = upgrade.currencyIncrease;

        Application.runInBackground = true;
        
        float timer = Time.realtimeSinceStartup - timeSinceLastCurrency;

        if (timer >= currencyInterval)
        {
            overallCurrency.AddCurrency(addedCurrency);

            timeSinceLastCurrency = Time.realtimeSinceStartup;
        }
    }

}
