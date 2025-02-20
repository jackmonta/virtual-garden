using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance { get; set; }
    public static string shopDataPath;

    [SerializeField] private List<Plant> starterShopPlants;
    [SerializeField] private List<NonPlantItem> starterShopNonPlantItems;

    public static List<Plant> ShopPlants { get; private set; }
    public static List<NonPlantItem> ShopNonPlantItems { get; private set; }

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
        LoadDataFromDisk();
    }

    public void LoadDataFromDisk()
    {
        try {
            if (TutorialUI.onlyTutorial == 1) throw new Exception();

            ShopData data = DataManager.LoadFromDisk<ShopData>(shopDataPath);
            ShopPlants = data.plants;
            ShopNonPlantItems = starterShopNonPlantItems;
            foreach (var itemData in data.nonPlantItems)
            {
                foreach (NonPlantItem nonPlantItem in ShopNonPlantItems)
                {
                    if (nonPlantItem.Name == itemData.name)
                    {
                        nonPlantItem.ClickCount = itemData.clickCount;
                        nonPlantItem.NotifyCountChanged(); // Questo solleva l'evento OnClickCountChanged
                        Debug.Log("Notification counter for " + nonPlantItem.Name + " ... " + nonPlantItem.ClickCount.ToString());
                    }
                }
                
            }
            Debug.Log(ShopPlants.Count + " shop plants loaded from disk.");
            Debug.Log(ShopNonPlantItems.Count + " non-plant items loaded from disk.");
        } catch (Exception)
        {
            Debug.Log("No file found, created new shop");
            ShopPlants = starterShopPlants;
            ShopNonPlantItems = starterShopNonPlantItems;
            ShopPlants.ForEach(plant => plant.SetMaxHealth());
            ShopNonPlantItems.ForEach(item => item.ClickCount = 1);
        }
    }

    public static void SelectNonPlantItem(NonPlantItem item)
    {
        item.IncrementCounter();
        Debug.Log(item.Name + " selected " + item.ClickCount + " times.");
    }
    
    public static void RemovePlant(Plant plant)
    {
        if (ShopPlants.Contains(plant))
        {
            ShopPlants.Remove(plant);
            ShopPanel.Instance.Refresh(); // Aggiorna il pannello per riflettere la rimozione
        }
    }
}
