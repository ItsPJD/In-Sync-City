using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the deactivation of the display panel for a building.
public class DisableDisplayPanel : MonoBehaviour
{
    [SerializeField] private GameObject buildingDisplayPanel;
   public void OnExitDisplay()
   {
      buildingDisplayPanel.SetActive(false);
   }
}
