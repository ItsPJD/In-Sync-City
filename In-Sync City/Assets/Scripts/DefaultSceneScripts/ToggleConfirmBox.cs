using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the exit to main menu box for the game over screen.
public class ToggleConfirmBox : MonoBehaviour
{
    [SerializeField] private GameObject exitMenuContainer;

    public void Start()
    {
        exitMenuContainer.SetActive(false);
    }
    public void OnToggle()
    {
        exitMenuContainer.SetActive(true);
    }
}
