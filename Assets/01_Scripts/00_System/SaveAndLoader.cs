using UnityEngine;

public static class SaveAndLoader
{
    public static void SaveData<T>(T saveData, string key)
    {
        string fileName = "SaveFile_" + key + ".es3";
        ES3.Save<T>(key, saveData, fileName);
    }
    public static T LoadData<T>(T loadData, string key)
    {
        string fileName = "SaveFile_" + key + ".es3";
        if(!HaveData(key))
            SaveData<T>(loadData, key);
        return ES3.Load<T>(key, fileName);
    }

    public static bool HaveData(string key)
    {
        string fileName = "SaveFile_" + key + ".es3";
        if (!ES3.KeyExists(key, fileName)) 
            return false;
        return true;
    }
}
