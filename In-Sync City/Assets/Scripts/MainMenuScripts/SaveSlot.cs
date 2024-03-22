using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId =  "";

    [Header("Content")]
    [SerializeField] private GameObject noData;
    [SerializeField] private GameObject hasData;
    [SerializeField] private TextMeshProUGUI coinsGainedText;   
    [SerializeField] private TextMeshProUGUI heartgemsGainedText;
    private List<DateTime> dateTimeListData;
    private List<GameObject> dateTimeList = new List<GameObject>();
    private bool isShowingTime;
    [SerializeField] private GameObject dateTimePrefab;
    [SerializeField] private Transform dateTimeContainer;
    [SerializeField] private GameObject dateTimePrefabTemplate;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
        isShowingTime = false;
    }

// This method sets the data for each save file shown in both the new game and load game menus. If there is no data in a file, then it will show the noData version of the display.
// Otherwise, it will display the coins and heartgems gained, as well as each saved time.
    public void SetData(GameData data)
    {
        if(data == null)
        {
            noData.SetActive(true);
            hasData.SetActive(false);
        }
        else
        {
            noData.SetActive(false);
            hasData.SetActive(true);

            coinsGainedText.text = data.totalCurrency.ToString();
            heartgemsGainedText.text = data.totalHeartgems.ToString();

            this.dateTimeListData = data.dateTimeListData.Select(str => DateTime.ParseExact(str, "HH:mm", System.Globalization.CultureInfo.InvariantCulture)).ToList();

            foreach (var dateTime in dateTimeListData)
            {
                SetTimeLineComponent(dateTime);
            }

            isShowingTime = true;
        }
    }

//This method returns the profile id of this save slot.
    public string GetProfileId()
    {
        return this.profileId;
    }

//This method toggles whether or not the save slot can be interacted with or not.
    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }

// This method is similar to the UIMenu version, used in the main scene. It creates the gameObject elements for
// each time that has been saved, so they can be displayed to the user.
    private void SetTimeLineComponent(DateTime parsedTime)
    {
        if(isShowingTime)
        {
            return;
        }

        dateTimePrefabTemplate.SetActive(true);
        GameObject dateTimeElement = Instantiate(dateTimePrefab, dateTimeContainer);
        dateTimePrefabTemplate.SetActive(false);


        TextMeshProUGUI textComponent = dateTimeElement.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = parsedTime.ToString("HH:mm");

        dateTimeList.Add(dateTimeElement);

        // Sort dateTimeUIElements based on the time of day
        dateTimeList = dateTimeList.OrderBy(e => DateTime.Parse(e.GetComponentInChildren<TextMeshProUGUI>().text)).ToList();

        // Rearrange UI elements
        for (int i = 0; i < dateTimeList.Count; i++)
        {
            dateTimeList[i].transform.SetSiblingIndex(i);
        }
    }

}
