using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("MainMenu");

        Debug.Log("Going to the main menu!");
    }
}