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

    private float earnedCoins = 0;
    public float EarnedCoins
	{
    	get { return earnedCoins; }
    	set { earnedCoins = value; }
	}
    
    [SerializeField]
    private float health;
    public float Health { get { return health; } }
    
    private float? currentHealth;

    public float? CurrentHealth
    {
        get { return currentHealth.HasValue ? currentHealth.Value : null; }
        set { currentHealth = value; }
    }

    private int? currentLevel;

    public int? CurrentLevel
    {
        get { return currentLevel.HasValue ? currentLevel.Value : 0; }
        set { currentLevel = value; }
    }

    [SerializeField]
    private PlantUpgrade[] upgrades;
    public PlantUpgrade[] Upgrades { get { return upgrades; } }
    
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    [SerializeField]
    private string desc;
    public string Desc { get { return desc; } }
    
    public GameObject Prefab { get { return upgrades[currentLevel.HasValue ? currentLevel.Value : 0].Prefab; } }

	public bool Upgrade()
    {
        if (CanUpgrade())
        {
            currentLevel++;
            earnedCoins = 0;
			return true;
        }
		
		return false;
    }

    public bool CanUpgrade()
    {
        return currentLevel.Value >= 0 && currentLevel.Value < upgrades.Length - 1 && Wallet.Instance.CanAfford(upgrades[currentLevel.Value + 1].Price);
    }

    public void EarnCoins(float coins)
    {
        earnedCoins += coins*coinPerSecond;
    }
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
