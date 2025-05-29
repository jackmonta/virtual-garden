using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image icon;
    [SerializeField] private Button collectButton;
    [SerializeField] private AudioSource collectSound;
    private Achievement achievement;
    
    private void UnlockedAchievement(Achievement achievement)
    {
        achievement.OnAchievementUnlocked -= UnlockedAchievement;
        collectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Collect!";
        collectButton.interactable = true;
    }
    
    private void ButtonClicked()
    {
        if (achievement.Done && !achievement.Collected)
        {
            Wallet.Instance.AddMoney(achievement.coinsReward);
            collectSound.Play();
            achievement.Collected = true;
            collectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Collected!";
            collectButton.interactable = false;
        }
    }

    public void SetAchievement(Achievement achievement)
    {
        this.achievement = achievement;
        collectButton.onClick.AddListener(ButtonClicked);
        
        if(achievement.Done == true && achievement.Collected == true)
        {
            collectButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Collected!";
            collectButton.interactable = false;
        }
        else if (achievement.Done == true)
        {
            collectButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Collect!";
            collectButton.interactable = true;
        }
        else
        {
            collectButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Complete the quest...";
            achievement.OnAchievementUnlocked += UnlockedAchievement;
            collectButton.interactable = false;
        }
        
        title.text = achievement.title;
        icon.sprite = achievement.icon;
    }
}
