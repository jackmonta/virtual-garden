using System.Collections.Generic;
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

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else 
            Instance = this;
        
        achievementsDataPath = Application.persistentDataPath + "/achievementData.json";
        LoadAchievements();
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

    void LoadAchievements()
    {
        Debug.Log("Loading Achievements...");
        try {
            achievements = DataManager.LoadFromDisk<AchievementList>(achievementsDataPath).achievements;
        } catch
        {
            Debug.Log("No achievements loaded from disk, loading starter set.");
            achievements = starterAchievements;
        }

        foreach (Achievement achievement in achievements)
        {
            Debug.Log($"Loading achievement: {achievement.title}");
            GameObject achievementUI = Instantiate(Resources.Load<GameObject>("Prefabs/AchievementsView/Achievement"), GameObject.Find("AchievementContent").transform);
            AchievementUI achievementComponent = achievementUI.GetComponent<AchievementUI>();
            achievementComponent.SetAchievement(achievement);
        }
    }
}
