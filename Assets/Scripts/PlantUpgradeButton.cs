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
        Plant plant = GardenPlant.selectedPlant.Plant;
        if(plant == null) return;
        if (!plant.Upgrade()) return;

        int plantLevel = plant.CurrentLevel.Value;
        Wallet.Instance.SubtractMoney(plant.Upgrades[plantLevel].Price);

        Garden.Instance.UpgradePlant(plant);
        PlantLevel.Instance.SetLevel(plant.CurrentLevel, GardenPlant.CalculatePlantProgress());
        GameObject popup = Instantiate(levelupPopup, transform.parent);
        Debug.Log("Popup instantiated");
    }
}
