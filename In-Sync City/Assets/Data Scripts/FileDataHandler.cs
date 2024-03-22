using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    //set variables of the name of the directory path we want to save our data in, and the name of the file itself.
    private string dataDirectoryPath = "";
    private string dataFileName = "";

// This constructor takes two parameters from the dataPersistenceManager script - a path to where the file will be saved, and a name for the file itself.
    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

// The load method takes a parameter of the profileId - this corresponds to which save file is being loaded. From here, it checks if there is an id. If there is,
// it will then find the save path using the directory, the profile id to load from the correct save file, and the name of the actual save itself. It then uses Json to convert the string data
// into a GameData format, so that each piece of saved data will be loaded with its correct data types.
    public GameData Load(string profileId)
    {
        if(profileId == null)
        {
            return null;
        }

        string completePath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
        GameData loadedData = null;
        if(File.Exists(completePath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(completePath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch (Exception e)
            {
                    Debug.LogError("Error occured while trying to load data from: " +completePath + "\n" + e);
            }
        }
        return loadedData;
        }


// The save method does a similar thing to the load method, except here it takes an additional parameter of "data", which allows for the corresponding saved data to be converted to a Json
// string format.
    public void Save(GameData data, string profileId)
    {
        if(profileId == null)
        {
            return;
        }

        string completePath = Path.Combine(dataDirectoryPath, profileId, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(completePath));

            string storedData = JsonUtility.ToJson(data, true);

            //using() makes sure that once the file has been read and written, it will close properly.
            using(FileStream stream = new FileStream(completePath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(storedData);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured while trying to save data to: " +completePath + "\n" + e);
        }
    }

// This method returns a dictionary of all the available save files, so they can be displayed and chosen in the main menu. It does this by finding the directory path,
// and for each file in that path it checks if its a data file. If it is, then it uses the Load method to load the data of that file, and then adds it to the dictionary.
    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirectoryPath).EnumerateDirectories();

        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string completePath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
            if(!File.Exists(completePath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            GameData profileData = Load(profileId);
            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }

            else
            {
                Debug.LogError("Tried to load profile, but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

// This method is used to find the last save file that was loaded by the user. It does this by checking the time that each save file was last updated.
// If a save file has less time since it was last updated than the others, it is assumed to be the last one that was played. This is used for the 
//continue option in the main menu.
    public string GetLastUpdatedProfileId()
    {
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();

        foreach(KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if(gameData == null)
            {
                continue;
            }

            if(mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }

            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }

            }
        }

        return mostRecentProfileId;
    }
}
