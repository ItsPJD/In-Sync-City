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

    public GameData Load()
    {
        string completePath = Path.Combine(dataDirectoryPath, dataFileName);
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

    public void Save(GameData data)
    {
        string completePath = Path.Combine(dataDirectoryPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(completePath));

            string storedData = JsonUtility.ToJson(data, true);

            //using the "using()" block ensures that file is closed once it has been written to read from method.
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
}
