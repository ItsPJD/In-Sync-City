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

// This method is 
    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

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
