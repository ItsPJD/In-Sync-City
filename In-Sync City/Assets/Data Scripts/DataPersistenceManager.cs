using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool allowDataTestingInScene = false;
//The header is here to give overall explanation in the editor to what the file name variable is for.
// The SerializeField here allows for the fileName to be set and changed in the editor, but still allows for the variable to be set to private so that it cannot be changed by any other script.
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

//Referencing GameData so we can access and use data stored in this data class.
    private GameData gameData;

//This variable creates a list of objects that hold data that we want to persist, and we use the data type of "IDataPersistence" as this is our blueprint from earlier as to what exactly we want each object in this list to have: a saveData and loadData method.
    private List<IDataPersistence> dataPersistenceObjects;

// References the FileDataHandler class, we will use this to reference the FileDataHandler method to create a directory and fileName for the data that we will be saving.
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance {get; private set; }

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

        //When the game starts, the path for the file in which the data is stored becomes equal to the variable name "dataHandler".
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded is called");
        //dataPersistenceObjects becomes equal to the return value of the method "FindAllDataPersistenceObjects".
        //the LoadGame method is called, which will either load previously saved data that was stored, or call the NewGame method if no data was found.
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
    public void NewGame()
    {
        //Called if data in GameData is equal to "null" (there is no data). The gameData variable becomes equal to the values stored in the GameData method in the GameData script, which holds the default values for all data stored there.
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        if(this.gameData == null)
        {
            Debug.Log("No data to save! A new game must be started in order to save data.");
            return;
        }

        foreach (IDataPersistence dataPersistObj in dataPersistenceObjects)
        {
            dataPersistObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("isOwned = " + gameData.isOwned);
        Debug.Log("currencyIncrease = " +gameData.currencyIncrease);
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        //This is for debugging. If we want to test the default screen scene on its own without going through the menu to have game data to load, we can use this to bypass this issue and allow testing
        // of data persistence without the main menu scene.
        if (this.gameData == null && allowDataTestingInScene)
        {
            NewGame();
        }

        //if there is no data, will send a debug log stating a new game needs to be started, and will return so that the rest of the method cannot run and produce any errors regarding abscence of data.
        if(this.gameData == null)
        {
            Debug.Log("No data was found to load. A new game should be started in order to continue.");
            return;
        }

        //For every script that utilises IDataPersistence for saving and loading data, this for loop will attempt to call the LoadData method in each of those scripts in order to correctly fill the loading scene with the data stored in the save file.
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
}
