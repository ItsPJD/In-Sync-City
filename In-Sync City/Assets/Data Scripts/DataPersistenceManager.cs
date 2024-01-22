using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
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
            Debug.LogError("There is more than one data persistence manager running in the scene!");
        }
        instance = this;
    }

    private void Start()
    {
        //When the game starts, the path for the file in which the data is stored becomes equal to the variable name "dataHandler".
        //dataPersistenceObjects becomes equal to the return value of the method "FindAllDataPersistenceObjects".
        //Finally, the LoadGame method is called, which will either load previously saved data that was stored, or call the NewGame method if no data was found.
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void NewGame()
    {
        //Called if data in GameData is equal to "null" (there is no data). The gameData variable becomes equal to the values stored in the GameData method in the GameData script, which holds the default values for all data stored there.
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
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

        //if method is called and no data is found, calls the NewGame method.
        if(this.gameData == null)
        {
            Debug.Log("Setting data to default");
            NewGame();
        }

        foreach (IDataPersistence dataPersistObj in dataPersistenceObjects)
        {
            dataPersistObj.LoadData(gameData);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
