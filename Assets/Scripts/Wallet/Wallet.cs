using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance;
    public static string walletDataPath;
    public int Money { get; private set; }

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

        walletDataPath = Application.persistentDataPath + "/walletData.json";
        LoadDataFromDisk();
    }

    public void LoadDataFromDisk()
    {
        try {
            if (TutorialUI.onlyTutorial == 1) throw new Exception();
            
            Instance.Money = DataManager.LoadFromDisk<IntWrapper>(walletDataPath).value;
            Debug.Log("Wallet loaded from disk: " + Instance.Money + " coins.");
        } catch (Exception)
        {
            Debug.Log("No wallet data found, created new wallet");
            Instance.Money = 10000;
        }
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void SubtractMoney(int amount)
    {
        Money -= amount;
    }

    public bool CanAfford(int amount)
    {
        return Money >= amount;
    }
}