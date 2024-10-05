using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance { get; set; }
    public static string shopDataPath;

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

        // loading data from disk
        try {
            ShopPlants = DataManager.LoadFromDisk<PlantList>(shopDataPath).plants;
            Debug.Log(ShopPlants.Count + " shop plants loaded from disk.");
        } catch (Exception)
        {
            Debug.Log("No file found, created new shop");
            ShopPlants = starterShopPlants;
        }
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
