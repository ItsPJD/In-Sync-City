using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
//These settings are here for debugging purposes, in order to test data persistence and functionality.
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool allowDataTestingInScene = false;

// The SerializeField here allows for the fileName to be set and changed in the editor, but still allows for the variable to be set to private so that it cannot be changed by any other script.
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

//Referencing GameData so we can access and use data stored in this data class.
    private GameData gameData;

//This variable creates a list of objects that hold data that we want to persist, and we use the data type of "IDataPersistence" as this is our blueprint from earlier as to what exactly we want each object in this list to have: a saveData and loadData method.
    private List<IDataPersistence> dataPersistenceObjects;

// References the FileDataHandler class, we will use this to reference the FileDataHandler method to create a directory and fileName for the data that we will be saving.
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    public static DataPersistenceManager instance {get; private set; }

// This awake method makes sure that only one DataPersistenceManager is in a scene at one time.
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("There is more than one data persistence manager running in the scene! Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if(disableDataPersistence)
        {
            Debug.LogWarning("Data persistence has been disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        this.selectedProfileId = dataHandler.GetLastUpdatedProfileId();
    }

// This section ensures that whenever the game is opened, it correctly loads the scenes, performs the load and save methods so that all save files can be correctly shown and displayed, 
// and then unloads them again.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded is called");

        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded is called");
        SaveGame();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

//This method changes which profile (save file) has been selected by the user.
    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

//Overwrites the save data on the currently selected save file by using the default data in GameData.
    public void NewGame()
    {
        this.gameData = new GameData();
    }

//The save game method finds all the scripts that have IDataPersistence attached to them, and calls the SaveData method on them. Then, it calls the Save method in
// the data handler so that all that data can be correctly saved to a Json string file.
    public void SaveGame()
    {
        if(disableDataPersistence)
        {
            return;
        }

        if(this.gameData == null)
        {
            Debug.Log("No data to save! A new game must be started in order to save data.");
            return;
        }

        foreach (IDataPersistence dataPersistObj in dataPersistenceObjects)
        {
            dataPersistObj.SaveData(ref gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
    }

// Similar to the SaveGame method, except it reads the save file and performs the loadData method in each script that contains IDataPersistence, to correctly load the saved data to the game.
    public void LoadGame()
    {
        if(disableDataPersistence)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null && allowDataTestingInScene)
        {
            NewGame();
        }

        if(this.gameData == null)
        {
            Debug.Log("No data was found to load. A new game should be started in order to continue.");
            return;
        }

        foreach (IDataPersistence dataPersistObj in dataPersistenceObjects)
        {
            dataPersistObj.LoadData(gameData);
        }
    }

    // When the player exits the game, this method is called so that the most up-to-date data can be saved properly. This is helpful in case the user exits the game unexpectedly, such as a crash of the game or their computer.
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // Finds every monobehaviour script that includes "IDatePersistence" in it, and stores it in a list for saving and loading data.
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    //used in the main menu in order to find whether the player has any save data.
    public bool hasGameData()
    {
        return gameData != null;
    }

// Calls the data handler method that returns a dictionary containing every profile and its saved data.
    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
