using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image icon;
    [SerializeField] private Button collectButton;
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
            Vector3 uiWorldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                collectButton.GetComponent<RectTransform>(),
                RectTransformUtility.WorldToScreenPoint(Camera.main, collectButton.transform.position),
                Camera.main,
                out uiWorldPos
            );
            GameObject coin = Instantiate(Resources.Load<GameObject>("Prefabs/GoldCoin/Coin"), uiWorldPos, Quaternion.identity);
            StartCoroutine(AnimateCoinToWallet(coin));
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
    
    
    private IEnumerator AnimateCoinToWallet(GameObject coin)
    {
        coin.GetComponent<AudioSource>().Play();
        
        Debug.Log("Animating coin to wallet");
        
        float duration = 0.5f; // Durata dell'animazione
        float elapsedTime = 0f;

        Vector3 startPosition = coin.transform.position;
        Vector3 targetScreenPosition = new Vector3(0, Screen.height * 0.9f, Camera.main.nearClipPlane);
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);
		
        Debug.Log("Animating coin to wallet 2");
        coin.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("Animating coin to wallet 3");

        while (elapsedTime < duration)
        {
            coin.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            coin.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.02f, 0.02f, 0.02f), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Finished animating coin to wallet");

        Wallet.Instance.AddMoney(achievement.coinsReward);
        coin.GetComponent<AudioSource>().Stop();
        Destroy(coin);
        Debug.Log("Coin collected and destroyed");
    }
}
