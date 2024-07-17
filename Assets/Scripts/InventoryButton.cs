using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{   
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnOpenButtonClick);
    }

    private void OnOpenButtonClick()
    {
        if (!InventoryUI.IsInventoryOpen)
            InventoryUI.OpenInventory();
    }
}
