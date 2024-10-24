using TMPro;
using UnityEngine;

public class notificationCounterRefresh : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private NonPlantItem nonPlantItem;

    private void Awake()
    {
        nonPlantItem.OnClickCountChanged += UpdateNotificationText;
        UpdateNotificationText();
    }

    private void UpdateNotificationText()
    {
        notificationText.text = nonPlantItem.ClickCount.ToString();
        Debug.Log("Updating notification counter for " + nonPlantItem.Name + " ... " + nonPlantItem.ClickCount.ToString());
    }

    private void OnDestroy()
    {
        nonPlantItem.OnClickCountChanged -= UpdateNotificationText;
    }
}