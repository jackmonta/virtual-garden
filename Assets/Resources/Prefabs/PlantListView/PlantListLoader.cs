using UnityEngine;
using System.Collections.Generic;

public class PlantListLoader : MonoBehaviour
{
    [SerializeField] private GameObject PlantItem;
    private float refreshInterval = 1f;
    private HashSet<Plant> displayedPlants = new HashSet<Plant>();

    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 0f, refreshInterval);
    }

    private void Refresh()
    {
        List<Plant> combinedPlants = new List<Plant>(Inventory.Plants);
        combinedPlants.AddRange(Garden.Instance.GetCopyPlantList());

        int displayIndex = displayedPlants.Count + 1;

        foreach (Plant plant in combinedPlants)
        {
            if (!displayedPlants.Contains(plant))
            {
                GameObject obj = Instantiate(PlantItem, this.transform);
                obj.GetComponent<PlantItem>().SetData(plant, displayIndex++);
                displayedPlants.Add(plant);
            }
        }
    }
}