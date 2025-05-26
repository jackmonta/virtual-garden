using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsView : MonoBehaviour
{
    public static AchivementsView Instance { get; private set; }
    public static string achivementsDataPath;
    [SerializeField] private List<Achievement> starterAchivements;
    private List<Achievement> achivements;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button xButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else 
            Instance = this;
        
        achivementsDataPath = Application.persistentDataPath + "/achivementData.json";
        LoadAchivements();
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

    void LoadAchivements()
    {
        Debug.Log("Loading Achievements...");
        try {
            achivements = DataManager.LoadFromDisk<AchivementList>(achivementsDataPath).achivements;
        } catch
        {
            Debug.Log("No achievements loaded from disk, loading starter set.");
            achivements = starterAchivements;
        }

        foreach (Achievement achivement in achivements)
        {
            Debug.Log($"Loading achievement: {achivement.title}");
            GameObject achivementUI = Instantiate(Resources.Load<GameObject>("Prefabs/AchievementsView/Achivement"), GameObject.Find("AchivementContent").transform);
            AchivementUI achivementComponent = achivementUI.GetComponent<AchivementUI>();
            achivementComponent.SetAchivement(achivement);
        }
    }
}
