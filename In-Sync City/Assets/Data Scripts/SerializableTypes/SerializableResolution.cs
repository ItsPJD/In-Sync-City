using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableResolution
{
    public int width;
    public int height;

    public SerializableResolution(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

}
