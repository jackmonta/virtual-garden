using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; set; }
    private static string inventoryDataPath;
    [SerializeField] private List<Plant> starterPlants;
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

        StartCoroutine(CreatePlantButtons());

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
            plants = starterPlants;
        }
        else
            Debug.Log("No inventory data found at " + inventoryDataPath);
    }

    private void SavePlantsToDisk()
    {
        PlantList plantList = new PlantList();

        if (plants == null || plants.Count == 0)
            plantList.plants = starterPlants;
        else
            plantList.plants = plants;

        string json = JsonUtility.ToJson(plantList);
        File.WriteAllText(inventoryDataPath, json);
        Debug.Log("Inventory Saved: " + inventoryDataPath);
    }

    private IEnumerator CreatePlantButtons()
    {
        while (InventoryUI.Instance == null)
            yield return null;
        
        InventoryUI.Instance.CreatePlantButtons(plants);
    }
    
    public static void SetSelectedPlant(Plant selectedPlant)
    {
        Inventory.selectedPlant = selectedPlant;
    }

    public static Plant GetSelectedPlant()
    {
        return selectedPlant;
    }
    
    public static void RemoveSelectedPlant()
    {
        if (selectedPlant == null)
        {
            Debug.Log("No plant selected to remove.");
            return;
        }

        plants.Remove(selectedPlant);
        InventoryUI.RemoveButton(selectedPlant);
        selectedPlant = null;
    }

    public static void AddPlant(Plant plant)
    {
        plants.Add(plant);
        InventoryUI.Instance.CreatePlantButtons(new List<Plant>(){ plant });
    }
}

[Serializable]
public class PlantList
{
    public List<Plant> plants;
}
