using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDisplayPanel : MonoBehaviour
{
    public GameObject buildingDisplayPanel;
   public void OnExitDisplay()
   {
      buildingDisplayPanel.SetActive(false);
   }
}
