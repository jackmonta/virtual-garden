using UnityEngine;

public class PlantUI : MonoBehaviour
{
    public static PlantUI Instance { get; private set; }
    [SerializeField] GameObject plantLevel;

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
    }

    void Start()
    {
        ShowUI(false);
    }

    public static void ShowUI(bool show)
    {
        if (Instance.gameObject.activeSelf != show)
        {
            if (show)
            {
                PlantLevel.Instance.SetLevel(GardenPlant.selectedPlant.Plant.CurrentLevel, CalculatePlantProgress());
            }
            Instance.gameObject.SetActive(show);
            Instance.plantLevel.SetActive(show);
        }
    }

    private static float CalculatePlantProgress()
    {
        GardenPlant selectedPlant = GardenPlant.selectedPlant;
        return selectedPlant.Plant.EarnedCoins / (selectedPlant.Plant.Health / 2); // TODO: check progress calculation
    }

    public void UpdateLevel()
    {
        PlantLevel.Instance.SetLevel(GardenPlant.selectedPlant.Plant.CurrentLevel, CalculatePlantProgress());
    }

    public static bool isActive()
    {
        return Instance.gameObject.activeSelf;
    }
}
