using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    [SerializeField]
    private GameObject settingsObject;
    public static SettingsManager instance {get; private set; }


    // Ensure that the instance is not destroyed when loading a new scene
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one settings manager in this scene!");
            Destroy(settingsObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(settingsObject);
    }
}
