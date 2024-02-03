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
    private void Start()
    {
        if(!DataPersistenceManager.instance.hasGameData())
        {
            continueButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
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
