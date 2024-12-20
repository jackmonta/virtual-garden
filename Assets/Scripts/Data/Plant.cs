using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant", order = 1)]
[System.Serializable]
public class Plant : ScriptableObject
{
    [SerializeField]
    private new string name;
    public string Name { get { return name; } }
    
    [SerializeField]
    private int price;
    public int Price { get { return price; } }
    
    [SerializeField]
    private int coinPerSecond;
    public int CoinPerSecond { get { return coinPerSecond; } }
    
    [SerializeField]
    private float health;
    public float Health { get { return health; } }
    
    private float? currentHealth;

    public float? CurrentHealth
    {
        get { return currentHealth.HasValue ? currentHealth.Value : null; }
        set { currentHealth = value; }
    }
    
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    public void IncreaseHealth(float amount)
    {
        if (IsCurrentHealthNull())
            SetMaxHealth();
        
        currentHealth += amount;
        if (currentHealth > health)
            currentHealth = health;
    }

    public void DecreaseHealth(float amount)
    {
        if (IsCurrentHealthNull())
            SetMaxHealth();
        
        currentHealth -= amount;
        if (currentHealth < 0f)
            currentHealth = 0f;
    }

    private bool IsCurrentHealthNull()
    {
        float? currentHealth = CurrentHealth;
        return currentHealth == null;
    }

    public void SetMaxHealth() {
        currentHealth = health;
    }
    
    public float getHealth() {
        return health;
    }
}
