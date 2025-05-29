using UnityEngine;

public class PlantUpgradeButton : MonoBehaviour
{
    GameObject levelupPopup;
    void Start()
    {
        levelupPopup = Resources.Load<GameObject>("Prefabs/PlantLevelUp/PlantLevelUp");
    }

    public void StartPlantUpgrade()
    {
		GardenPlant gardenPlant = GardenPlant.selectedPlant;
        Plant plant = gardenPlant.Plant;
        if (plant == null) return;
        if (!plant.Upgrade()) return;

        gardenPlant.upgradeRevitalizing();
		if(gardenPlant.IsInfected()) gardenPlant.RemoveInsects();
		//if(gardenPlant.CanCollectCoins()) StartCoroutine(gardenPlant.AnimateCoinsToWallet());
		
        int plantLevel = plant.CurrentLevel.Value;
        Wallet.Instance.SubtractMoney(plant.Upgrades[plantLevel].Price);
        PlantLevel.Instance.SetLevel(plant.CurrentLevel, GardenPlant.CalculatePlantProgress());
        Instantiate(levelupPopup, GameObject.Find("SettingUI").transform);

        GardenPlant.SetSelectedPlant(null);
		Garden.Instance.UpgradePlant(plant);
    }
}
