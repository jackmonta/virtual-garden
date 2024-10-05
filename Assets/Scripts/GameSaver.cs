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
        PlantList plantsToSerialize = new PlantList();
        plantsToSerialize.plants = Shop.ShopPlants;
        DataManager.SaveToDisk(Shop.shopDataPath, plantsToSerialize);
    }
    
    private void SaveWallet()
    {
        if (Wallet.Instance == null)
        {
            Debug.Log("No wallet instance");
            return;
        }
        DataManager.SaveToDisk(Wallet.walletDataPath, Wallet.Instance.Money);
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
