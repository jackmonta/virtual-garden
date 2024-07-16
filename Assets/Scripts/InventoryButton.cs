using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private bool isInventoryOpen;
    private TextMeshProUGUI buttonText;
    
    void Start()
    {
        Debug.Log("Button starting...");
        isInventoryOpen = false;
        buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        gameObject.GetComponent<Button>().onClick.AddListener(OnOpenCloseButtonClick);
        Debug.Log("Added event listener!");
    }

    private void OnOpenCloseButtonClick()
    {
        Debug.Log("Inventory button clicked");
        if (isInventoryOpen)
            CloseInventory();
        else
            OpenInventory();
    }

    private void OpenInventory()
    {
        if (buttonText == null)
            Debug.Log("buttonText is null");
        else
            buttonText.text = "CLOSE";
        isInventoryOpen = true;
    }

    private void CloseInventory()
    {
        buttonText.text = "OPEN";
        isInventoryOpen = false;
    }
}
