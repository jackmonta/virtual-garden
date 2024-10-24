using UnityEngine;
using System;

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
    
    private int clickCount;
    public int ClickCount 
    { 
        get { return clickCount; } 
        set { clickCount = value; }
    }
    

    // Evento che segnala il cambiamento del contatore di click
    public event Action OnClickCountChanged;

    public void IncrementCounter()
    {
        clickCount++;
        OnClickCountChanged?.Invoke(); // Solleva l'evento se ci sono listener
    }

    public void DecrementCounter()
    {
        if (clickCount > 0)
        {
            clickCount--;
            OnClickCountChanged?.Invoke(); // Solleva l'evento se ci sono listener
        }
    }

    // Metodo per forzare la notifica di cambiamento, utile quando vengono caricati i dati
    public void NotifyCountChanged()
    {
        OnClickCountChanged?.Invoke(); // Solleva l'evento
    }
}