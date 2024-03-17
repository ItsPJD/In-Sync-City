using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
