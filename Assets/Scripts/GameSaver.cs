using System.Collections;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private float interval = 15f;

    void Start()
    {
        StartCoroutine(StartAutoSaveCoroutine());
    }

    private IEnumerator StartAutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            SaveAll();
            Debug.Log("Game Saved!");
        }
    }

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
    
    public void SaveWallet()
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
        SaveInventory();
        SaveShop();
        SaveWallet();
    }

    void OnApplicationQuit()
    {
        SaveAll();   
    }
}
