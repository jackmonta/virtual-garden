using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

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
        LoadDataFromDisk();

        StartCoroutine(CreatePlantButtons());
    }

    public void LoadDataFromDisk()
    {
        try {
            if (TutorialUI.onlyTutorial == 1) throw new Exception();
            
            plants = DataManager.LoadFromDisk<PlantList>(inventoryDataPath).plants;
            List<float> currentHealths = DataManager.LoadFromDisk<PlantList>(inventoryDataPath).plantHealths;
			List<int> currentLevels = DataManager.LoadFromDisk<PlantList>(inventoryDataPath).plantLevels;
            List<float> currentEarnedCoins = DataManager.LoadFromDisk<PlantList>(inventoryDataPath).earnedCoins;
            Debug.Log(plants.Count + " inventory plants loaded from disk.");
            for (int i = 0; i < plants.Count; i++){
                plants[i].CurrentHealth = currentHealths[i];
				plants[i].CurrentLevel = currentLevels[i];
                plants[i].EarnedCoins = currentEarnedCoins[i];
                Debug.Log(plants[i].Name + " current health: " + plants[i].CurrentHealth);
            }
        } catch (Exception)
        {
            Debug.Log("No plants loaded from disk, loading starter set.");
            plants = starterPlants;
            //plants.ForEach(plant => plant.SetMaxHealth());
			
			plants.ForEach(plant => {
       			plant.SetMaxHealth();
       			plant.CurrentLevel = 0;
                plant.EarnedCoins = 0;
    		});
            
        }
        
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