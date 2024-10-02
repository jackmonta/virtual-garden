using System;
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
        PlantList plantsToSerialize = new PlantList();
        plantsToSerialize.plants = Inventory.Plants;
        DataManager.SaveToDisk(Inventory.inventoryDataPath, plantsToSerialize);
    }

    private void SaveAll()
    {
        SaveInventory();
        //SaveShop();
        //SaveWallet();
    }

    void OnApplicationQuit()
    {
        SaveAll();   
    }
}
