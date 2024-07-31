using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; }
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button openButton;

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
        Button closeButton = shopPanel.GetComponent<ShopPanel>().CloseShopButton;

        openButton.onClick.AddListener(() => {
            shopPanel.SetActive(true);

            openButton.enabled = false;
            closeButton.enabled = true;
        });

        closeButton.onClick.AddListener(() => {
            shopPanel.SetActive(false);
            
            closeButton.enabled = false;
            openButton.enabled = true;
        });

        openButton.gameObject.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void ShowUI()
    {
        openButton.gameObject.SetActive(true);
    }
}
