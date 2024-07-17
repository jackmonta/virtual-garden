using UnityEngine;
using UnityEngine.UI;

public class InventoryPlantButton : MonoBehaviour
{
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject icon;
    private Button button;
    private bool isSelected;
    void Start()
    {
        isSelected = false;

        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            isSelected = !isSelected;
            border.SetActive(isSelected);
            Debug.Log(gameObject.name + " is selected: " + isSelected);
        });
    }
}
