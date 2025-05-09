using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantView : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button xButton;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Desc;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Health;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private TextMeshProUGUI Upgrades;
    [SerializeField] private TextMeshProUGUI CoinPerSec;

    void Start()
    {
        closeButton.onClick.AddListener(() => {
            Destroy(gameObject);
        });
        xButton.onClick.AddListener(() => {
            Destroy(gameObject);
        });
    }

    public void SetData(Plant plant)
    {
        Name.text = plant.Name;
        Desc.text = plant.Desc.Replace("\\n", "\n");
        Icon.sprite = plant.Icon;
        Health.text = plant.Health.ToString();
        Price.text = Price.text = plant.Upgrades[1].Price.ToString();
        Upgrades.text = (plant.Upgrades.Length - 1).ToString();
        CoinPerSec.text = plant.CoinPerSecond.ToString();
    }
}
