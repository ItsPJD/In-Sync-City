using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;

    [SerializeField] private Button backButton;
    [SerializeField] private GameObject overwritePanel;
    

    private bool isLoadingGame = false;
    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
        overwritePanel.SetActive(false);
    }

// This method handles the loading and creating a new game when clicking on a save slot. If its loading, the method will load the next scene, using
// that save slot's profile id to load in the appropriate data. If its a new game, a confirmation of overwriting data will appear first, to confirm
// that the user will lose progress on that save if they continue.
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if(!isLoadingGame)
        {
            overwritePanel.SetActive(true);
        }

        else
        {
            SceneManager.LoadSceneAsync("DefaultScreen");
        }

    }

// This method handles the back button, deactivating this menu and activating the main menu again.
    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

// This method handles the activation of the load and new game menu. It checks whether its loading or if its making a new game,
// and from there displays the information of each save file for the user to see.
    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
        
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

// This method deactivates this menu.
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

// This method deactivates all other menu buttons, so the user doesnt accidentally click on one while transitioning to another scene or other similar issues.
    public void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        backButton.interactable = false;
    }

// This method re-enables the menu buttons, allowing the user to interact with them again.
    public void EnableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(true);
        }

        backButton.interactable = true;
    }

// These methods handle whether the user chose to overwrite the new game data or not.
    public void NoOverwrite()
    {
        overwritePanel.SetActive(false);
        EnableMenuButtons();
    }

    public void ConfirmOverwrite()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("DefaultScreen");
    }
}
