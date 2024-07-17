using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private static Canvas inventoryCanvas;
    private static List<GameObject> plantObjs;
    private static GameObject selectedPlantObj;
    void Start()
    {
        plantObjs = new List<GameObject>();

        inventoryCanvas = GetComponent<Canvas>();
        inventoryCanvas.enabled = false;
    }

    public static void ShowUI()
    {
        inventoryCanvas.enabled = true;
    }

    public static void SetSelectedPlant(GameObject selectedPlantObj)
    {
        if (!plantObjs.Contains(selectedPlantObj))
            plantObjs.Add(selectedPlantObj);

        InventoryUI.selectedPlantObj = selectedPlantObj;

        foreach (GameObject plantObj in plantObjs)
            if (!plantObj.Equals(selectedPlantObj))
                plantObj.GetComponent<InventoryPlantButton>().Deselect();
    }
}
