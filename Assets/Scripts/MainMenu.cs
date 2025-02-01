using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button resumeGameButton;
    [SerializeField] TMP_ColorGradient disabledColor;
    private readonly List<string> fileNames = new()
    {
        "/inventoryData.json",
        "/shopData.json",
        "/walletData.json"
    };

    void Start()
    {
        if (!IsGameStarted())
        {
            Debug.Log("Disabled resume button");
            resumeGameButton.enabled = false;
            resumeGameButton.GetComponentInChildren<TextMeshProUGUI>().colorGradientPreset = disabledColor;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartNewGame()
    {
        ClearData();
        PlayGame();
    }

    private bool IsGameStarted()
    {
        TutorialUI.firstLaunch = PlayerPrefs.GetInt("FirstLaunch");
        return TutorialUI.firstLaunch != 0;
    }

    private void ClearData()
    {
        foreach (string path in fileNames)
        {
            // deleting data files
            string filePath = Application.persistentDataPath + path;
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
                
            // resetting tutorial
            PlayerPrefs.SetInt("FirstLaunch", 0);
            TutorialUI.firstLaunch = PlayerPrefs.GetInt("FirstLaunch");

            // refreshing data
            if (Inventory.Instance != null)
                Inventory.Instance.LoadDataFromDisk();
            if (Shop.Instance != null)
                Shop.Instance.LoadDataFromDisk();
            if (Wallet.Instance != null)
                Wallet.Instance.LoadDataFromDisk();
        }
    }
}
