using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image icon;
    [SerializeField] private Button collectButton;
    private Achievement achievement;

    void Start()
    {
        collectButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Complete the quest...";
    }

    private void UnlockedAchievement(Achievement achievement)
    {
        achievement.OnAchievementUnlocked -= UnlockedAchievement;
        collectButton.gameObject.GetComponent<TextMeshProUGUI>().text = "Collect!";
    }

    public void SetAchievement(Achievement achievement)
    {
        this.achievement = achievement;
        achievement.OnAchievementUnlocked += UnlockedAchievement;
        title.text = achievement.title;
        icon.sprite = achievement.icon;
    }
}
