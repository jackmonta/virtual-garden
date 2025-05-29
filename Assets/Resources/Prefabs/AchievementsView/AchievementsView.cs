using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsView : MonoBehaviour
{
    public static AchievementsView Instance { get; private set; }
    public static string achievementsDataPath;
    [SerializeField] private List<Achievement> starterAchievements;
    private List<Achievement> achievements;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button xButton;
    [SerializeField] private GameObject settingsNotification;
    [SerializeField] private GameObject achievementNotification;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else 
            Instance = this;
        
        achievementsDataPath = Application.persistentDataPath + "/achievementData.json";
        LoadDataFromDisk();
        UpdateNotification();
    }

    public void UpdateNotification()
    {
        int counter = GetReadyAchievements();
        if (counter > 0)
        {
            settingsNotification.gameObject.SetActive(true);
            achievementNotification.gameObject.SetActive(true);
            settingsNotification.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = counter.ToString();
            achievementNotification.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = counter.ToString();
        }
        else
        {
            settingsNotification.gameObject.SetActive(false);
            achievementNotification.gameObject.SetActive(false);
        }
    }

    private int GetReadyAchievements()
    {
        int counter = 0;
        foreach (Achievement a in achievements)
        {
            if (a.Done == true && a.Collected == false) counter++;
        }

        return counter;
    }

    void Start()
    {
        closeButton.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
        xButton.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void LoadDataFromDisk()
    {
        Debug.Log("Loading Achievements...");
        
        achievements = new List<Achievement>(starterAchievements);

        try
        {
            if (TutorialUI.onlyTutorial == 1) throw new Exception();
            AchievementList achievementList = DataManager.LoadFromDisk<AchievementList>(achievementsDataPath);
            for (int i = 0; i < achievementList.achievements.Count; i++)
            {
                achievements[i].Done = achievementList.done[i];
                achievements[i].Collected = achievementList.collected[i];
            }
        }
        catch
        {
            Debug.Log("No achievements loaded from disk, loading starter set.");
            foreach (Achievement achievement in achievements)
            {
                achievement.Done = false;
                achievement.Collected = false;
            }
        }

        foreach (Achievement achievement in achievements)
        {
            Debug.Log($"Loading achievement: {achievement.title}");
            GameObject achievementUI = Instantiate(Resources.Load<GameObject>("Prefabs/AchievementsView/Achievement"), GameObject.Find("AchievementContent").transform);
            AchievementUI achievementComponent = achievementUI.GetComponent<AchievementUI>();
            achievementComponent.SetAchievement(achievement);
        }
    }
    
    
    public void UnlockAchievement(string achievementId)
    {
        foreach (var achievement in achievements)
        {
            if (achievement.title == achievementId && !achievement.Done)
            {
                achievement.Done = true;
                UpdateNotification();
            }
        }
    }

    public List<Achievement> GetCopyAchievements()
    {
        List<Achievement> copy = new List<Achievement>();
        foreach (var achievement in achievements)
        {
            Achievement clonedAchievement = ScriptableObject.CreateInstance<Achievement>();
            clonedAchievement.title = achievement.title;
            clonedAchievement.Done = achievement.Done;
            clonedAchievement.Collected = achievement.Collected;
            // Copy other fields if necessary
        
            copy.Add(clonedAchievement);
        }
        return copy;
    }
}
