using UnityEngine;
using System.Collections.Generic;

public class GameSaver : MonoBehaviour
{
    private void SaveInventory()
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
        foreach (var plant in combinedPlants)
        {
            //plantsToSerialize.plantHealths.Add(plant.CurrentHealth.Value);
            plantsToSerialize.plantHealths.Add(plant.CurrentHealth.HasValue ? plant.CurrentHealth.Value : plant.Health);
        }
        DataManager.SaveToDisk(Inventory.inventoryDataPath, plantsToSerialize);
    }

    private void SaveShop()
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
    
    private void SaveWallet()
    {
        if (Wallet.Instance == null)
        {
            Debug.Log("No wallet instance");
            return;
        }
        int money = Wallet.Instance.Money;
        DataManager.SaveToDisk(Wallet.walletDataPath, money);
    }
    

    private void SaveAll()
    {
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
