using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private static Canvas inventoryCanvas;
    private static GameObject inventorySlider;
    public static bool IsInventoryOpen { get; set; }

    void Start()
    {
        IsInventoryOpen = false;

        inventoryCanvas = GetComponent<Canvas>();
        inventoryCanvas.enabled = false;

        inventorySlider = transform.Find("InventorySlider").gameObject;
    }

    public static void ShowUI()
    {
        inventoryCanvas.enabled = true;
    }

    public static void OpenInventory()
    {
        inventorySlider.SetActive(true);
        IsInventoryOpen = true;
    }
}
