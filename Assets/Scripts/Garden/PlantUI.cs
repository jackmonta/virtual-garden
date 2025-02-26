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
                PlantLevel.Instance.SetLevel(GardenPlant.selectedPlant.Plant.CurrentLevel, GardenPlant.CalculatePlantProgress());
            }
            Instance.gameObject.SetActive(show);
            Instance.plantLevel.SetActive(show);
        }
    }

    public void UpdateLevel()
    {
        PlantLevel.Instance.SetLevel(GardenPlant.selectedPlant.Plant.CurrentLevel, GardenPlant.CalculatePlantProgress());
    }

    public static bool isActive()
    {
        return Instance.gameObject.activeSelf;
    }
}
