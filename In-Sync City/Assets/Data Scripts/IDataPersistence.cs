using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface that is added to each script that needs data to be saved to a file.
public interface IDataPersistence
{
// These are the "blueprints" of the methods we want to use in the rest of our scripts for saving and loading data.
    void LoadData(GameData data);

//used ref here as we want to modify data here, but not when loading the game.
    void SaveData(ref GameData data);
    
}
