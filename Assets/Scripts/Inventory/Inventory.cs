using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; set; }
    private static string inventoryDataPath;
    private static List<Plant> plants;
    private static Plant selectedPlant;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        inventoryDataPath = Application.persistentDataPath + "/inventoryData.json";

        LoadPlantsFromDisk();

        selectedPlant = null;
    }

    void OnApplicationQuit()
    {
        SavePlantsToDisk();
    }

    private void LoadPlantsFromDisk()
    {
        if (File.Exists(inventoryDataPath))
        {
            string json = File.ReadAllText(inventoryDataPath);
            PlantList plantList = JsonUtility.FromJson<PlantList>(json);

            if (plantList != null && plantList.plants != null)
            {
                plants = plantList.plants;
                Debug.Log("Inventory Loaded: " + inventoryDataPath);
                return;
            }

            Debug.Log("No plants found in inventory data at " + inventoryDataPath);
            Debug.Log("Creating a new inventory...");
            plants = new List<Plant>();
        }
        else
            Debug.Log("No inventory data found at " + inventoryDataPath);
    }

    private void SavePlantsToDisk()
    {
        PlantList plantList = new PlantList();

        if (plants == null || plants.Count == 0)
        {
            List<Plant> temp = new List<Plant>()
            {
                new Plant("Plant 1", null),
                new Plant("Plant 2", null),
                new Plant("Plant 3", null)
            };
            plantList.plants = temp;
        }
        else
            plantList.plants = plants;

        string json = JsonUtility.ToJson(plantList);
        File.WriteAllText(inventoryDataPath, json);
        Debug.Log("Inventory Saved: " + inventoryDataPath);
    }

    public static void SetSelectedPlant(Plant selectedPlant)
    {
        Inventory.selectedPlant = selectedPlant;
    }
}

[Serializable]
public class PlantList
{
    public List<Plant> plants;
}