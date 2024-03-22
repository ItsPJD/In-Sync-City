using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour, IDataPersistence
{
   [Header("Room ID")]
   [SerializeField] private string id;
   [Header("Coin Generated")]
   [SerializeField] private long currencyIncrease = 1;
    [SerializeField] private int currencyUpgrade = 1;
    [Header("Level")]
    [SerializeField] private int currentLvl = 1;
    private int maxLvl = 30;
    [Header("Upgrade Costs")]
    [SerializeField] private long upgradeCostCoins = 50;
    [SerializeField] private int upgradeCostHeartgems = 1;
    [SerializeField] private long upgradeCostIncrement = 25;
    [SerializeField] private int upgradeCostHeartgemsIncrement = 1;
    public CurrencyScript currencyScript;
    [Header("Room Display")]
    [SerializeField] private string buildingName;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI buildingCurrentLevelText;
    [SerializeField] private TextMeshProUGUI buildingNextLevelText;
    [SerializeField] private TextMeshProUGUI buildingCostText;
    [SerializeField] private TextMeshProUGUI buildingCurrentCoinGenerationText;
    [SerializeField] private TextMeshProUGUI buildingNextCoinGenerationText;
    [SerializeField] private TextMeshProUGUI buildingHeartGemCostText;
    [SerializeField] private GameObject buildingDisplayPanel;
    [SerializeField] private UpgradeButtonScript upgradeButton;
    [SerializeField] private BuildingScript buildingScript;

    void Start()
    {
      buildingDisplayPanel.SetActive(false);
    }

// This method handles the upgrading of buildins themselves, checking whether they have reached the max level or if they cannot afford the upgrade.
// If neither of these are true, then it will correctly upgrade that building, as well as remove the appropriate currency for payment of that upgrade.
    public void UpgradeBuilding()
     {
         long totalAmount = currencyScript.GetCurrency();

         int totalHeartgems = currencyScript.GetHeartgems();

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
   
   // This method is called when a user clicks on a building, and sets the target building so that the correct building's info is displayed. It then displays the panel
   // so the user can see this information directly.
   public void OnClickBuilding()
   {
      upgradeButton.setTargetBuilding(this);
      buildingDisplayPanel.SetActive(true);
      if(currentLvl >= maxLvl)
      {
         displayMaxInfo();
      }
      displayPanelInfo();
   }

//This method closes the building display.
   public void OnExitDisplay()
   {
      buildingDisplayPanel.SetActive(false);
   }

// This method handles the display of all the information of a building, within the building display panel.
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

// This method is displayed when the maximum level of a building has been reached.
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

// This method returns the value of how much currency this building produces, which is used in the building script to determine
// how much should be given to the user for each time interval.
   public long getCurrencyIncrease()
   {
      return this.currencyIncrease;
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
