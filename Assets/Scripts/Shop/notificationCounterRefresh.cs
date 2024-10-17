using UnityEngine;
using TMPro; 

public class notificationCounterRefresh : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private NonPlantItem nonPlantItem;
    
    void Update()
    {
        notificationText.text = nonPlantItem.ClickCount.ToString();
    }
}
