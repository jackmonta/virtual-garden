using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; set; }
    public static string inventoryDataPath;
    [SerializeField] private List<Plant> starterPlants;
    private static List<Plant> plants;
    public static List<Plant> Plants { get { return plants; } }
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
        
        // loading data from disk
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

        selectedPlant = null;
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