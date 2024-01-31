using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button contineButton;
    private void Start()
    {
        if(!DataPersistenceManager.instance.hasGameData())
        {
            contineButton.interactable = false;
        }
    }
    public void OnNewGameClick()
    {
        DataPersistenceManager.instance.NewGame();

        SceneManager.LoadSceneAsync("DefaultScreen");
    }

    public void OnContinueGameClick()
    {
        SceneManager.LoadSceneAsync("DefaultScreen");
    }
}
