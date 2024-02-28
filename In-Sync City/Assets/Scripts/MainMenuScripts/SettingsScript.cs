using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour, IDataPersistence
{
    Resolution[] resolutions;

    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    private int resIndex;

    [SerializeField]
    private Toggle fullscreenToggle;
    [SerializeField]
    private GameObject settingsPanel;

    void Awake()
    {
        SetResolution(resIndex);
    }

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void activatePanel()
    {
        settingsPanel.SetActive(true);
    }


    public void goBack()
    {
        this.gameObject.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        this.resIndex = data.resolutionIndex;
    }

    public void SaveData(ref GameData data)
    {
        data.resolutionIndex = this.resIndex;
    }
}
