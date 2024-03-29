using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script handles the confirmation of exiting back to the main menu.
public class ExitToMainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject exitMenuContainer;

    public void OnNoClick()
    {
        exitMenuContainer.SetActive(false);
    }

    public void OnConfirmClick()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("MainMenu");

        Debug.Log("Going to the main menu!");
    }

}