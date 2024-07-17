using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private static Canvas inventoryCanvas;
    private static GameObject inventoryButton;
    private static GameObject inventorySlider;
    private static bool isInventoryOpen;

    void Start()
    {
        isInventoryOpen = false;

        inventoryCanvas = GetComponent<Canvas>();
        inventoryCanvas.enabled = false;

        inventoryButton = transform.Find("InventoryButton").gameObject;
        inventorySlider = transform.Find("InventorySlider").gameObject;

        inventoryButton.GetComponent<Button>().onClick.AddListener(() => {
            if (!isInventoryOpen)
                OpenInventory();
        });
    }

    public static void ShowUI()
    {
        inventoryCanvas.enabled = true;
    }

    private static void OpenInventory()
    {
        inventorySlider.SetActive(true);
        isInventoryOpen = true;
    }
}
