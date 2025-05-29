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
        plantsToSerialize.earnedCoins = new List<float>();
        foreach (var plant in combinedPlants)
        {
            plantsToSerialize.plantHealths.Add(plant.CurrentHealth.HasValue ? plant.CurrentHealth.Value : plant.Health);
            plantsToSerialize.plantLevels.Add(plant.CurrentLevel.HasValue ? plant.CurrentLevel.Value : 0);
            plantsToSerialize.earnedCoins.Add(plant.EarnedCoins);
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

	private static void SaveAchievements()
    {
        if (AchievementsView.Instance == null)
        {
            Debug.Log("No achievements instance");
            return;
        }
		List<Achievement> achievements = AchievementsView.Instance.GetAchievements();
        AchievementList achievementList = new AchievementList();
        achievementList.achievements = achievements;
        DataManager.SaveToDisk(AchievementsView.achievementsDataPath, achievementList);
    }
    

    public static void SaveAll()
    {
        if (TutorialUI.onlyTutorial != 0) return;
        
        Debug.Log("Saving Game...");

        SaveInventory();
        SaveShop();
        SaveWallet();
        SaveAchievements();

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
