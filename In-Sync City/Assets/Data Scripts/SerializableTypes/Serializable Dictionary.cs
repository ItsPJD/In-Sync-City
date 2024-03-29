using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{

// This script allows for Dictionary key/value types to be saved to GameData, and have it then be serialized to a Json string file. This is useful
// for certain data such as building ownership, to identify if they are owned or not.
    [SerializeField] private List<TKey> keys = new List<TKey>();

    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count != values.Count)
        {
            Debug.LogError("Tried deserialise dictionary, but the amount of keys (" + keys.Count + ") does not match number of values (" + values.Count + ") which indicates an error.");
        }

        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
