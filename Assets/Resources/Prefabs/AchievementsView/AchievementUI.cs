using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image icon;
    [SerializeField] private Button collectButton;
    [SerializeField] private TextMeshProUGUI collectButtonText;
    [SerializeField] private AudioSource collectSound;
    [SerializeField] private TextMeshProUGUI goldCoin;
    [SerializeField] private Image tick;
    private Achievement achievement;
    
    private void UnlockedAchievement(Achievement achievement)
    {
        if (collectButton == null || collectButtonText == null)
        {
            Debug.LogWarning("UnlockedAchievement called before UI was fully initialized.");
            return;
        }
        Debug.Log("Buonaseeeeeeeera");
        
        achievement.OnAchievementUnlocked -= UnlockedAchievement;
        collectButtonText.text = "Collect";
        collectButton.interactable = true;
    }
    
    private void ButtonClicked()
    {
        if (achievement.Done && !achievement.Collected)
        {
            Wallet.Instance.AddMoney(achievement.coinsReward);
            collectSound.Play();
            achievement.Collected = true;
            AchievementsView.Instance.UpdateNotification();
            collectButtonText.text = "Collected";
            collectButton.interactable = false;
            tick.gameObject.SetActive(true);
        }
    }

    public void SetAchievement(Achievement achievement)
    {
        this.achievement = achievement;
        collectButton.onClick.AddListener(ButtonClicked);
        
        if(achievement.Done == true && achievement.Collected == true)
        {
            collectButtonText.text = "Collected";
            collectButton.interactable = false;
            tick.gameObject.SetActive(true);
        }
        else if (achievement.Done == true)
        {
            collectButtonText.text = "Collect";
            collectButton.interactable = true;
            tick.gameObject.SetActive(false);
        }
        else
        {
            collectButtonText.text = "Locked";
            achievement.OnAchievementUnlocked += UnlockedAchievement;
            collectButton.interactable = false;
            tick.gameObject.SetActive(false);
        }
        
        title.text = achievement.title;
        icon.sprite = achievement.icon;
        goldCoin.text = achievement.coinsReward.ToString();
    }
}
