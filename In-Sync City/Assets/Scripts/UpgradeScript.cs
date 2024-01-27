using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour, IDataPersistence
{

   [SerializeField] private string id;
    public long currencyIncrease = 1;

    [SerializeField] private int currencyUpgrade = 1;

    public int currentLvl = 1;

    private int maxLvl = 30;

    public long upgradeCostCoins = 50;
    public int upgradeCostHeartgems = 1;

    [SerializeField] private long upgradeCostIncrement = 25;

    [SerializeField] private int upgradeCostHeartgemsIncrement = 1;
    public CurrencyScript currencyScript;

    public string buildingName;

    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI buildingCurrentLevelText;
    public TextMeshProUGUI buildingNextLevelText;
    public TextMeshProUGUI buildingCostText;
    public TextMeshProUGUI buildingCurrentCoinGenerationText;
    public TextMeshProUGUI buildingNextCoinGenerationText;

    public TextMeshProUGUI buildingHeartGemCostText;
    public GameObject buildingDisplayPanel;

    public UpgradeButtonScript upgradeButton;

    public BuildingScript buildingScript;

    void Start()
    {
      buildingDisplayPanel.SetActive(false);
    }
    public void UpgradeBuilding()
     {
         long totalAmount = currencyScript.totalCurrency;

         int totalHeartgems = currencyScript.totalHeartgems;

         if(currentLvl >= maxLvl)
         {
            Debug.Log("Max Level reached on this building!");
            displayMaxInfo();
         }

         if (upgradeCostCoins > totalAmount || upgradeCostHeartgems > totalHeartgems)
         {
            Debug.Log("You cannot afford this upgrade!");
         }

         else
         {

            currencyIncrease += currencyUpgrade;
            currentLvl ++;

            currencyScript.SpendCurrency(upgradeCostCoins);
            currencyScript.SpendHeartgems(upgradeCostHeartgems); 

            if(currentLvl == 10 || currentLvl == 20)
            {
              upgradeCostHeartgems += upgradeCostHeartgemsIncrement;
            }

            upgradeCostCoins += upgradeCostIncrement;

            Debug.Log("Upgraded Building");
            displayPanelInfo();
         }

     }
   
   public void OnClickBuilding()
   {
      upgradeButton.setTargetBuilding(this);
      buildingDisplayPanel.SetActive(true);
      displayPanelInfo();
   }

   public void OnExitDisplay()
   {
      buildingDisplayPanel.SetActive(false);
   }

   private void displayPanelInfo()
   {
      buildingNameText.text = buildingName;

      buildingCurrentLevelText.text = ("Level ") + currentLvl.ToString();
      int nextLevel = currentLvl + 1;
      buildingNextLevelText.text = ("Level ") + nextLevel.ToString();

      buildingCostText.text = upgradeCostCoins.ToString();

      float currencyInterval = buildingScript.getCurrencyInterval();
      buildingCurrentCoinGenerationText.text = currencyIncrease.ToString() + (" Coin per ") + currencyInterval.ToString() + ("s");
      long nextCoinGen = currencyIncrease + currencyUpgrade;
      buildingNextCoinGenerationText.text = nextCoinGen.ToString() + (" Coin per ") + currencyInterval.ToString() + ("s");

      buildingHeartGemCostText.text = upgradeCostHeartgems.ToString();

   }

   private void displayMaxInfo()
   {
      buildingNameText.text = buildingName;

      buildingCurrentLevelText.text = currentLvl.ToString();
      buildingNextLevelText.text = ("MAX UPGRADE REACHED");

      buildingCostText.text = ("N/A");

      buildingCurrentCoinGenerationText.text = currencyIncrease.ToString();
      buildingNextCoinGenerationText.text = ("MAX UPGRADE REACHED");

      buildingHeartGemCostText.text = ("N/A");
   }

   public void LoadData(GameData data)
   {
      if (data.currencyIncrease.TryGetValue(id, out long currencyIncrease))
      {
         this.currencyIncrease = currencyIncrease;
      }  


      if (data.currentLvl.TryGetValue(id, out int currentLvl))
      {
         this.currentLvl = currentLvl;
      } 


      if (data.upgradeCost.TryGetValue(id, out long upgradeCost))
      {
         this.upgradeCostCoins = upgradeCost;
      } 

      if (data.upgradeCostHeartgems.TryGetValue(id, out int upgradeCostHeartgems))
      {
         this.upgradeCostHeartgems = upgradeCostHeartgems;
      }

      Debug.Log("Loaded upgrade data to GameData from ID: " + id);
   }

   public void SaveData(ref GameData data)
   {
      if(data.currencyIncrease.ContainsKey(id))
      {
         data.currencyIncrease.Remove(id);
      }
      data.currencyIncrease.Add(id, currencyIncrease);


      if(data.currentLvl.ContainsKey(id))
      {
         data.currentLvl.Remove(id);
      }
      data.currentLvl.Add(id, currentLvl);


      if(data.upgradeCost.ContainsKey(id))
      {
         data.upgradeCost.Remove(id);
      }
      data.upgradeCost.Add(id, upgradeCostCoins);


      if(data.upgradeCostHeartgems.ContainsKey(id))
      {
         data.upgradeCostHeartgems.Remove(id);
      }
      data.upgradeCostHeartgems.Add(id, upgradeCostHeartgems);

      Debug.Log("Saved upgrade data to GameData from ID: " + id);
   }
   
}
