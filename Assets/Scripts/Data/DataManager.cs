using System;
using System.IO;
using UnityEngine;

public static class DataManager
{
    public static void SaveToDisk<T>(string filePath, T data)
    {
        try
        {
            string json;
            if (typeof(T) == typeof(int))
                json = JsonUtility.ToJson(new IntWrapper((int)(object)data));
            else
                json = JsonUtility.ToJson(data);
            
            Debug.Log("Json Data: " + json);
            File.WriteAllText(filePath, json);
            Debug.Log("Data Saved in: " + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save data to disk: " + ex.Message);
        }
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

[Serializable]
public class IntWrapper
{
    public int value;

    public IntWrapper(int value)
    {
        this.value = value;
    }
}