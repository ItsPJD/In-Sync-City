using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script handles the upgrading of buildings, based upon the building selected. 
public class UpgradeButtonScript : MonoBehaviour
{
    private UpgradeScript targetBuilding;

    [SerializeField] private Button upgradeButton;

    public void UpgradeActivated()
    {
        if(targetBuilding != null)
        {
            targetBuilding.UpgradeBuilding();
        }
        else
        {
            Debug.Log("No target building was found!");
        }
    }

    public void setTargetBuilding(UpgradeScript building)
    {
        targetBuilding = building;
    }
}
