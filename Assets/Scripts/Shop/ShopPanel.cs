using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public static ShopPanel Instance { get; private set; }
    [SerializeField] private Button closeShopButton;
    [SerializeField] private GameObject ButtonsContent;
    [SerializeField] private GameObject shopItemButtonPrefab;
    private bool isLoaded;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isLoaded = false;
    }

    public Button CloseShopButton
    {
        get { return closeShopButton; }
    }

    void OnEnable()
    {
        if (isLoaded == false)
        {
            Refresh();
            isLoaded = true;
        }
    }

    private void LoadShopItems()
    {
        // Carica le piante
        Shop.ShopPlants.ForEach((Plant p) => {
            GameObject shopItemButtonObj = Instantiate(shopItemButtonPrefab, transform);
            shopItemButtonObj.transform.SetParent(ButtonsContent.transform, false);

            ShopItemButton shopItemButton = shopItemButtonObj.GetComponent<ShopItemButton>();
            shopItemButton.SetPlant(p);
        });

        // Carica gli oggetti non vegetali
        Shop.ShopNonPlantItems.ForEach((NonPlantItem item) => {
            GameObject shopItemButtonObj = Instantiate(shopItemButtonPrefab, transform);
            shopItemButtonObj.transform.SetParent(ButtonsContent.transform, false);

            ShopItemButton shopItemButton = shopItemButtonObj.GetComponent<ShopItemButton>();
            shopItemButton.SetNonPlantItem(item);
        });
    }

    public void Refresh()
    {
        foreach (Transform child in ButtonsContent.transform)
            Destroy(child.gameObject);

        LoadShopItems();
    }
}
