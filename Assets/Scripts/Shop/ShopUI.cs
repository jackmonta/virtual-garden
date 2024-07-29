using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private static Canvas shopCanvas;
    public static ShopUI Instance { get; private set; }
    private static Dictionary<GameObject, Plant> shopButtons;


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
        shopCanvas = GetComponent<Canvas>();
        shopCanvas.enabled = false;
    }

    public static void ShowUI()
    {
        shopCanvas.enabled = true;
    }
}
