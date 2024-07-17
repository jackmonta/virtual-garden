using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private static Canvas inventoryCanvas;

    void Start()
    {
        inventoryCanvas = GetComponent<Canvas>();
        inventoryCanvas.enabled = false;
    }

    public static void ShowUI()
    {
        inventoryCanvas.enabled = true;
    }
}
