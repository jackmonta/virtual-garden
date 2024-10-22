using UnityEngine;

public class GameSaver : MonoBehaviour
{

    private void SaveInventory()
    {
        if (Inventory.Instance == null)
        {
            Debug.Log("No inventory instance");
            return;
        }
        PlantList plantsToSerialize = new PlantList();
        plantsToSerialize.plants = Inventory.Plants;
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
        shopData.nonPlantItems = Shop.ShopNonPlantItems;
        
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
