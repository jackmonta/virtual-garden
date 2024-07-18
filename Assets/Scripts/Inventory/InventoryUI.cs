using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private static Canvas inventoryCanvas;
    public static InventoryUI Instance { get; private set; }
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private GameObject plantButtonPrefab;
    private static Dictionary<GameObject, Plant> plantButtons;
    private static GameObject selectedButton;


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
            plantButtons = new Dictionary<GameObject, Plant>();

        if (plants.Count == 0)
        {
            Debug.Log("No plants to create buttons for.");
            return;
        }

        foreach (Plant plant in plants)
        {
            GameObject plantButton = Instantiate(plantButtonPrefab, buttonContainer.transform);
            plantButton.GetComponent<InventoryPlantButton>().SetPlant(plant);
            plantButtons.Add(plantButton, plant);
        }
    }

    public static void SetSelectedButton(GameObject selectedButton)
    {
        InventoryUI.selectedButton = selectedButton;

        // set selected plant
        if (selectedButton != null)
        {
            Plant selectedPlant = plantButtons[InventoryUI.selectedButton];
            Inventory.SetSelectedPlant(selectedPlant);
        }
        else
            Inventory.SetSelectedPlant(null);

        // deselect all other buttons
        foreach (GameObject button in plantButtons.Keys)
            if (button != selectedButton)
                button.GetComponent<InventoryPlantButton>().Deselect();
    }

    public static void RemoveButton(Plant plant)
    {
        foreach (GameObject button in plantButtons.Keys)
            if (plantButtons[button] == plant)
            {
                if (button == selectedButton)
                    SetSelectedButton(null);

                Destroy(button);
                plantButtons.Remove(button);
            }
    }
}
