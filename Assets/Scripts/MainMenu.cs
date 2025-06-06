using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button resumeGameButton;
    [SerializeField] TMP_ColorGradient disabledColor;
    [SerializeField] GameObject creditsPanel;
    private readonly List<string> fileNames = new()
    {
        "/inventoryData.json",
        "/shopData.json",
        "/walletData.json",
        "/achievementData.json"
    };

    void Start()
    {
        if (!IsGameStarted())
        {
            Debug.Log("Disabled resume button");
            resumeGameButton.enabled = false;
            resumeGameButton.GetComponentInChildren<TextMeshProUGUI>().colorGradientPreset = disabledColor;
        }

        if (TutorialUI.onlyTutorial == 1)
            TutorialUI.onlyTutorial = 0;
        
        creditsPanel.SetActive(false);
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

    public void StartTutorial()
    {
        TutorialUI.onlyTutorial = 1;
        PlayGame();
    }

    public void OpenCredits()
    {
        this.gameObject.SetActive(false);
        creditsPanel.SetActive(true);
    }

    private bool IsGameStarted()
    {
        TutorialUI.firstLaunch = PlayerPrefs.GetInt("FirstLaunch");
        Debug.Log("FirstLaunch is " + TutorialUI.firstLaunch.ToString());
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
            if (AchievementsView.Instance != null)
                AchievementsView.Instance.LoadDataFromDisk();
        }
    }
}
