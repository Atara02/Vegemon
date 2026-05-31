using System.Collections.Generic;
using UnityEngine;

public static class ResourceLoader
{
    public static T[] LoadDataAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }
    public static T LoadData<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
