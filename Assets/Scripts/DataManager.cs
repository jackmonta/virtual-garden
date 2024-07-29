using System.IO;
using UnityEngine;

public static class DataManager
{
    public static void SaveToDisk<T>(string filePath, T data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
        Debug.Log("Data Saved in: " + filePath);
    }

    public static T LoadFromDisk<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("No file found at " + filePath);
            return default;
        }

        string json = File.ReadAllText(filePath);
        T data = JsonUtility.FromJson<T>(json);

        if (data != null)
        {
            Debug.Log("Data Loaded: " + filePath);
            return data;
        }

        Debug.Log("No data found in file at " + filePath);
        return default;
    }
}
