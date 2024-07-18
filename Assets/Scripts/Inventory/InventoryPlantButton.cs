using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlantButton : MonoBehaviour
{
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject plantName;
    [SerializeField] private GameObject plantIcon;
    private Button button;
    private bool isSelected;
    
    void Start()
    {
        isSelected = false;

        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            isSelected = !isSelected;

            if (isSelected)
                InventoryUI.SetSelectedButton(gameObject);
            else
                InventoryUI.SetSelectedButton(null);

            border.SetActive(isSelected);
        });
    }

    public void Deselect()
    {
        isSelected = false;
        border.SetActive(isSelected);
    }

    public void SetPlant(Plant plant)
    {
        SetName(plant.Name);
        //plantIcon.GetComponent<Image>().sprite = plant.Icon;
    }

    private void SetName(string name)
    {
        plantName.GetComponent<TextMeshProUGUI>().text = name;
    }
}
