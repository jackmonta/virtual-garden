using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    public Plant Plant { get; private set; }

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (Wallet.Instance.CanAfford(Plant.Price))
        {
            Wallet.Instance.SubtractMoney(Plant.Price);
            Shop.RemovePlant(Plant);
            Inventory.AddPlant(Plant);
        }
    }

    public void SetPlant(Plant plant)
    {
        Plant = plant;

        itemIcon.sprite = plant.Icon;
        itemName.text = plant.Name;
        itemPrice.text = plant.Price.ToString();
    }
}
