using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; set; }
    private Canvas inventoryCanvas;
    private Button openCloseButton;
    private List<GameObject> plants;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        inventoryCanvas = GetComponent<Canvas>();
        inventoryCanvas.enabled = false;
        openCloseButton = GetComponentInChildren<Button>();

        plants = new List<GameObject>();
        // TODO: load plants from disk
    }

    public void ShowUI()
    {
        inventoryCanvas.enabled = true;
    }
}
