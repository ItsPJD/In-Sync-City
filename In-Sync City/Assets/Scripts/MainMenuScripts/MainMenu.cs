using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadGameButton;

// The start method here checks to see if there is no saved data, and if not it will not allow the user to interact with the continue or load game functionality.
    private void Start()
    {
        if(!DataPersistenceManager.instance.hasGameData())
        {
            continueButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }

// These methods help to toggle the various buttons in the main menu and their own menus, excluding the exit to desktop functionality.
    public void OnNewGameClick()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClick()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClick()
    {
        SceneManager.LoadSceneAsync("DefaultScreen");
    }

    public void ActivateMenu()
    {   
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
