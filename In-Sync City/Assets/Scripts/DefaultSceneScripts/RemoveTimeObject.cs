using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the removal of a saved time.
public class RemoveTimeObject : MonoBehaviour
{
    public UIMenu uiMenu;

    public void RemoveThisTime()
    {
        uiMenu.RemoveTime(this.gameObject);
    }
}
