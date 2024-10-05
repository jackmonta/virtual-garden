using System.IO;
using UnityEngine;

public static class DataManager
{
    public static void SaveToDisk<T>(string filePath, T data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log("Json Data: " + json);
        File.WriteAllText(filePath, json);
        Debug.Log("Data Saved in: " + filePath);
    }

    public static T LoadFromDisk<T>(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("The file does not exist.", filePath);

        string json = File.ReadAllText(filePath);
        Debug.Log("Json Data: " + json);
        T data = JsonUtility.FromJson<T>(json);

        if (data != null)
        {
            Debug.Log("Data Loaded: " + filePath);
            Debug.Log("Data: " + data);
            return data;
        }

        Debug.Log("No data found in file at " + filePath);
        return default;
    }
}
