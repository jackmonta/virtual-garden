using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    public AudioSource audioSource_buy;
    public AudioSource audioSource_wrong;

    public Plant Plant { get; private set; }
    public NonPlantItem NonPlantItem { get; private set; }
    
    private bool isFirstPlant = true;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (Plant != null)
        {
            if (Wallet.Instance.CanAfford(Plant.Price))
            {
                AudioSource tempAudioSource = Instantiate(audioSource_buy, transform.position, Quaternion.identity);
                tempAudioSource.Play();
                Wallet.Instance.SubtractMoney(Plant.Price);
                Shop.RemovePlant(Plant);
                Inventory.AddPlant(Plant);
                if (isFirstPlant)
                {
                    isFirstPlant = false;
                    AchievementsView.Instance.UnlockAchievement("Buy 1 Plant");
                }
            }
            else audioSource_wrong.Play();
        }
        else if (NonPlantItem != null)
        {
            if (Wallet.Instance.CanAfford(NonPlantItem.Price))
            {
                audioSource_buy.Play();
                Wallet.Instance.SubtractMoney(NonPlantItem.Price);
                Shop.SelectNonPlantItem(NonPlantItem); // Incrementa il contatore
            }
            else audioSource_wrong.Play();  
        }
    }

    public void SetPlant(Plant plant)
    {
        Plant = plant;
        itemIcon.sprite = plant.Icon;
        itemName.text = plant.Name;
        itemPrice.text = plant.Price.ToString();
    }

    public void SetNonPlantItem(NonPlantItem nonPlantItem)
    {
        NonPlantItem = nonPlantItem;
        itemIcon.sprite = nonPlantItem.Icon;
        itemName.text = nonPlantItem.Name;
        itemPrice.text = nonPlantItem.Price.ToString();
    }
}
