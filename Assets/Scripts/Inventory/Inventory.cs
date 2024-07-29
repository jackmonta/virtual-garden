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

        // loading data from disk
        inventoryDataPath = Application.persistentDataPath + "/inventoryData.json";
        List<Plant> plants = DataManager.LoadFromDisk<List<Plant>>(inventoryDataPath);
        if (plants != null)
            Inventory.plants = plants;
        else
            Inventory.plants = starterPlants;

        StartCoroutine(CreatePlantButtons());

        selectedPlant = null;
    }

    void OnApplicationQuit()
    {
        DataManager.SaveToDisk(inventoryDataPath, plants);
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