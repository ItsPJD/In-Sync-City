using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTimeObject : MonoBehaviour
{
    public UIMenu uiMenu;

    public void RemoveThisTime()
    {
        uiMenu.RemoveTime(this.gameObject);
    }
}
