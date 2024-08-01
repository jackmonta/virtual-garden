using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance { get; set; }
    private static string shopDataPath;
    private static string walletDataPath;
    public static Wallet wallet;

    [SerializeField] private List<Plant> starterShopPlants;
    public static List<Plant> ShopPlants { get; private set; }

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

        shopDataPath = Application.persistentDataPath + "/shopData.json";
        List<Plant> plants = DataManager.LoadFromDisk<List<Plant>>(shopDataPath);
        if (plants != null)
            ShopPlants = plants;
        else
        {
            ShopPlants = starterShopPlants;
            Debug.Log("No file found, created new shop");
        }

        walletDataPath = Application.persistentDataPath + "walletData.json";
        wallet = Wallet.Load(walletDataPath);
    }
    void OnApplicationQuit()
    {
        DataManager.SaveToDisk(shopDataPath, ShopPlants);
        wallet.Save(walletDataPath);
    }

    public static void RemovePlant(Plant plant)
    {
        if (ShopPlants.Contains(plant))
        {
            ShopPlants.Remove(plant);
            ShopPanel.Instance.Refresh();
        }
    }
}
