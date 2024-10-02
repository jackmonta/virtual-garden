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
        List<Plant> loadedPlants = DataManager.LoadFromDisk<PlantList>(inventoryDataPath).plants;
        if (loadedPlants != null && loadedPlants.Count > 0)
        {
            Debug.Log(loadedPlants.Count + " plants loaded from disk.");
            Inventory.plants = loadedPlants;
        }
        else
        {
            Debug.Log("No plants loaded from disk, loading starter set.");
            Inventory.plants = starterPlants;
        }

        StartCoroutine(CreatePlantButtons());
        StartCoroutine(AutoSaveCoroutine(15f));

        selectedPlant = null;
    }

    private void SaveInventory()
    {
        PlantList plantsToSerialize = new PlantList();
        plantsToSerialize.plants = plants;
        //plantsToSerialize.plants = starterPlants;
        DataManager.SaveToDisk(inventoryDataPath, plantsToSerialize);
    }

    void OnApplicationQuit()
    {
        SaveInventory();
    }
    private IEnumerator AutoSaveCoroutine(float saveInterval)
    {
        while (true)
        {
            yield return new WaitForSeconds(saveInterval);
            SaveInventory();
        }
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