using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class CurrencyScript : MonoBehaviour, IDataPersistence
{

    public long totalCurrency = 0;

    public int totalHeartgems = 0;

    public TextMeshProUGUI currencyText;

    public TextMeshProUGUI heartgemText;

    public void AddCurrency(long amount)
    {
        totalCurrency += amount;
    }

    public void AddHeartgems(int amount)
    {
        totalHeartgems += amount;
    }

    public void SpendCurrency(long spentAmount)
    {
        totalCurrency -= spentAmount;
        Debug.Log("Money Spent");
    }

    public void SpendHeartgems(int spentHeartgems)
    {
        totalHeartgems -= spentHeartgems;
        Debug.Log("Heartgems Spent");
    }


    private void Update()
    {
        currencyText.SetText(totalCurrency.ToString());
        heartgemText.SetText(totalHeartgems.ToString());
    }

    public void LoadData(GameData data)
    {
        this.totalCurrency = data.totalCurrency;
        this.totalHeartgems = data.totalHeartgems;
        Debug.Log("Currency Being Loaded in currency Script");
    }

    public void SaveData(ref GameData data)
    {
        data.totalCurrency = this.totalCurrency;
        data.totalHeartgems = this.totalHeartgems;
        Debug.Log("Currency Being Saved in currency Script");
    }
}
