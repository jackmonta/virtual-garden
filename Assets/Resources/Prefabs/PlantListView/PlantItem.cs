using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI plantName;
    [SerializeField] private Image plantIcon;
    [SerializeField] private TextMeshProUGUI plantNumber;

    public void SetData(Plant plant, int number)
    {
        plantName.text = plant.name;
        plantIcon.sprite = plant.Icon;
        plantNumber.text = number.ToString();
    }
}
