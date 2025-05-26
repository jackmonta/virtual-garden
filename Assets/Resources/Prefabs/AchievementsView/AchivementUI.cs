using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchivementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image icon;
    [SerializeField] private Button collectButton;
    private Achievement achivement;

    void Start()
    {
        collectButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Complete the quest...";
    }

    private void UnlockedAchivement(Achievement achievement)
    {
        achivement.OnAchievementUnlocked -= UnlockedAchivement;
        collectButton.gameObject.GetComponent<TextMeshProUGUI>().text = "Collect!";
    }

    public void SetAchivement(Achievement achievement)
    {
        this.achivement = achievement;
        // achivement.OnAchievementUnlocked += UnlockedAchivement;
        // title.text = achivement.title;
        // icon.sprite = achivement.icon;
    }
}
