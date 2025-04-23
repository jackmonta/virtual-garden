using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI plantName;
    [SerializeField] private Image plantIcon;
    [SerializeField] private TextMeshProUGUI plantNumber;
    [SerializeField] private GameObject plantViewPrefab;

    public void SetData(Plant plant, int number)
    {
        plantName.text = plant.name;
        plantIcon.sprite = plant.Icon;
        plantNumber.text = number.ToString();

        GetComponent<Button>().onClick.AddListener(() => {
            Transform popupContainer = GameObject.Find("SettingUI")?.transform;
            GameObject plantView = Instantiate(plantViewPrefab, popupContainer);
            plantView.GetComponent<PlantView>().SetData(plant);
        });
    }
}
