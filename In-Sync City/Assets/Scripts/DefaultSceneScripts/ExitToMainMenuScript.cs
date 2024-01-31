using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExitToMainMenuScript : MonoBehaviour
{
    public GameObject exitMenuContainer;
    public Button noButton;
    public Button confirmButton;

    public void OnNoClick()
    {
        exitMenuContainer.SetActive(false);
    }

    public void OnConfirmClick()
    {
        Debug.Log("Going to the main menu!");
    }
}