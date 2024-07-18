using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private static Canvas inventoryCanvas;
    public static InventoryUI Instance { get; private set; }
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private GameObject plantButtonPrefab;

    private List<GameObject> plantButtons;


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
        inventoryCanvas = GetComponent<Canvas>();
        inventoryCanvas.enabled = false;
    }

    public static void ShowUI()
    {
        inventoryCanvas.enabled = true;
    }

    public void CreatePlantButtons(List<Plant> plants)
    {
        if (plantButtons == null)
            plantButtons = new List<GameObject>();

        if (plants.Count == 0)
        {
            Debug.Log("No plants to create buttons for.");
            return;
        }

        foreach (Plant plant in plants)
        {
            GameObject plantButton = Instantiate(plantButtonPrefab, buttonContainer.transform);
            plantButton.GetComponent<InventoryPlantButton>().SetPlant(plant);
            plantButtons.Add(plantButton);
        }
    }
}
