using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlantUpgradeButton : MonoBehaviour
{
    public void StartPlantUpgrade()
    {
        Plant plant = GardenPlant.selectedPlant.Plant;
        if(plant == null) return;
        if (!plant.Upgrade()) return;
        GardenPlant.SetSelectedPlant(null);
        Garden.Instance.UpgradePlant(plant);
    }
}
