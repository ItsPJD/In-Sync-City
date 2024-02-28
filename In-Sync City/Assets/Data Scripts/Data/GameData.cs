using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playerHealth;
    public long lastUpdated;
    public long totalCurrency;
    public int totalHeartgems;
    public SerializableDictionary<string, bool> isOwned;

    public SerializableDictionary<string, long> currencyIncrease;

    public SerializableDictionary<string, int> currentLvl;

    public SerializableDictionary<string, long> upgradeCost;

    public SerializableDictionary<string, int> upgradeCostHeartgems;

    public List<String> dateTimeListData;
    
    public List<float> dateTimeWorkOrRelax;
    public int resolutionIndex;



//This constructor is called whenever no data is found in the data file for the game, and this resets values back to default.
    public GameData()
    {
        this.playerHealth = 10;
        this.totalCurrency = 0;
        this.totalHeartgems = 0;
        this.isOwned = new SerializableDictionary<string, bool>();
        this.currencyIncrease = new SerializableDictionary<string, long>();
        this.currentLvl = new SerializableDictionary<string, int>();
        this.upgradeCost = new SerializableDictionary<string, long>();
        this.upgradeCostHeartgems = new SerializableDictionary<string, int>();
        this.dateTimeListData = new List<String>();
        this.dateTimeWorkOrRelax = new List<float>();

        this.dateTimeListData.Add("09:00");
        this.dateTimeListData.Add("12:00");
        this.dateTimeListData.Add("13:00");
        this.dateTimeListData.Add("17:00");

        this.dateTimeWorkOrRelax.Add(1);
        this.dateTimeWorkOrRelax.Add(0);
        this.dateTimeWorkOrRelax.Add(1);
        this.dateTimeWorkOrRelax.Add(0);

        this.resolutionIndex = 0;
    }
}
