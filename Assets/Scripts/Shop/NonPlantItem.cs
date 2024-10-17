using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New nonPlant", menuName = "nonPlant", order = 1)]
[System.Serializable]
public class NonPlantItem : ScriptableObject
{
    [SerializeField]
    private new string name;
    public string Name { get { return name; } }
    
    [SerializeField]
    private int price;
    public int Price { get { return price; } }
    
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }
    
    [SerializeField]
    private int clickCount;
    public int  ClickCount { get { return clickCount; } }
    
    [SerializeField]
    public TextMeshProUGUI notificationText;
    
    public void IncrementCounter()
    {
        clickCount++;
        UpdateCounterText(); 
    }
    
    public void DecrementCounter()
    {
        if(clickCount > 0){
            clickCount--;
            UpdateCounterText();
        }
    }
    
    private void UpdateCounterText()
    {
        if (notificationText != null)
        {
            notificationText.text = clickCount.ToString();
        }
    }
}
