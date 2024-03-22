using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddBuilding : MonoBehaviour, IDataPersistence
{

// This script handles the placement of buildings. In particular, it uses IDataPersistence to determine whether a certain building
// has been bought or not.
   [SerializeField] private string id;

    [SerializeField] private GameObject building;

    [SerializeField] private GameObject buildingButton;

    [SerializeField] private GameObject upgradeButton;

    [SerializeField] private long buildingCost = 0;

    [SerializeField] private TextMeshProUGUI buildingCostText;

    public CurrencyScript currencyScript;

    private bool isOwned = false;

    public void PlaceBuilding()
     {
        long totalAmount = currencyScript.GetCurrency();

        if(buildingCost > totalAmount)
        {
            Debug.Log("You cannot afford to place this building");
        }

        else
        {
            currencyScript.SpendCurrency(buildingCost);
            ActivateBuilding();
        }

     }

     public void Start()
     {
         buildingCostText.text = buildingCost.ToString();
     }

     public void ActivateBuilding()
     {     
        isOwned = true;

        building.SetActive(true);

        buildingButton.SetActive(false);
        upgradeButton.SetActive(true);
        Debug.Log(isOwned);

     }

   public void LoadData(GameData data)
   {
      data.isOwned.TryGetValue(id, out isOwned);
      if(isOwned)
      {
         ActivateBuilding();
      }

      else
      {
         building.SetActive(false);
      }

      Debug.Log("Loading Building with id: " + id);
   }

   public void SaveData(ref GameData data)
   {
      if (data.isOwned.ContainsKey(id))
      {
         data.isOwned.Remove(id);
      }
      data.isOwned.Add(id, isOwned);
      Debug.Log("Saving Building with id: " + id + " to GameData");
   }

}
