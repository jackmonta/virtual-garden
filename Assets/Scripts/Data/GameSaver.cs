using UnityEngine;
using System.Collections.Generic;

public class GameSaver : MonoBehaviour
{
    private static Dictionary<string, string> savedData = new();
    private static void SaveInventory()
    {
        if (Inventory.Instance == null)
        {
            Debug.Log("No inventory instance");
            return;
        }
        List<Plant> combinedPlants = new List<Plant>(Inventory.Plants);
        combinedPlants.AddRange(Garden.Instance.GetCopyPlantList());
        PlantList plantsToSerialize = new PlantList();
        plantsToSerialize.plants = combinedPlants;
        plantsToSerialize.plantHealths = new List<float>();
        plantsToSerialize.plantLevels = new List<int>();
        foreach (var plant in combinedPlants)
        {
            plantsToSerialize.plantHealths.Add(plant.CurrentHealth.HasValue ? plant.CurrentHealth.Value : plant.Health);
            plantsToSerialize.plantLevels.Add(plant.CurrentLevel.HasValue ? plant.CurrentLevel.Value : 0);
        }
        DataManager.SaveToDisk(Inventory.inventoryDataPath, plantsToSerialize);
    }

    private static void SaveShop()
    {
        if (Shop.Instance == null)
        {
            Debug.Log("No shop instance");
            return;
        }
        
        ShopData shopData = new ShopData();
        shopData.plants = Shop.ShopPlants;
        shopData.nonPlantItems = new List<NonPlantItemData>();
        
        foreach (var item in Shop.ShopNonPlantItems) // Modifica qui
        {
            NonPlantItemData nonPlantItemData = new NonPlantItemData
            {
                name = item.Name,
                clickCount = item.ClickCount,
            };
            shopData.nonPlantItems.Add(nonPlantItemData);
        }
        
        DataManager.SaveToDisk(Shop.shopDataPath, shopData);
    }
    
    private static void SaveWallet()
    {
        if (Wallet.Instance == null)
        {
            Debug.Log("No wallet instance");
            return;
        }
        int money = Wallet.Instance.Money;
        DataManager.SaveToDisk(Wallet.walletDataPath, money);
    }
    

    public static void SaveAll()
    {
        if (TutorialUI.onlyTutorial != 0) return;
        
        Debug.Log("Saving Game...");

        SaveInventory();
        SaveShop();
        SaveWallet();

        Debug.Log("Game Saved!");
    }

    void OnApplicationPause()
    {
        SaveAll();
    }

    void OnApplicationQuit()
    {
        SaveAll();   
    }
}
