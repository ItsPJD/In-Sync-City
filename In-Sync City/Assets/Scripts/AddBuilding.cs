using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddBuilding : MonoBehaviour, IDataPersistence
{

   [SerializeField] private string id;

    public GameObject building;

    public GameObject buildingButton;

    public GameObject upgradeButton;

    public int buildingCost = 0;

    public CurrencyScript currencyScript;

    private bool isOwned = false;

    public void PlaceBuilding()
     {
        long totalAmount = currencyScript.totalCurrency;

        if(buildingCost > totalAmount)
        {
            Debug.Log("You cannot afford to place this building");
        }

        else
        {
            ActivateBuilding();
        }

     }

     public void ActivateBuilding()
     {     
        isOwned = true;

        building.SetActive(true);

        buildingButton.SetActive(false);
        upgradeButton.SetActive(true);
        Debug.Log(isOwned);

        //Debug.Log("The building is now set to be active");
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
