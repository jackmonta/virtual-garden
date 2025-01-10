using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class beeButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject beePrefab;
    
    private static beeButtonAnimation _instance;
    private GameObject currentBee;
    private BeeController beeController;

    public static beeButtonAnimation Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<beeButtonAnimation>();
                if (_instance == null)
                {
                    Debug.LogError("No instance of beeButtonAnimation found in the scene.");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // Assicura che l'istanza non venga distrutta al cambio di scena
        }
        else
        {
            Destroy(gameObject);  // Distrugge gli oggetti duplicati
        }
    }
    
    private Dictionary<string, string> plantTexts = new Dictionary<string, string>
{
    { "Rose", "Rose (Rosa spp.)\nElegant and fragrant flower, ideal for gardens and bouquets.\n\nCare Tips\n    Sun: At least 6 hours daily.\n    Water: Keep soil moist, avoid wetting leaves.\n    Soil: Fertile, well-draining.\n    Fertilizer: Every 4-6 weeks in spring/summer.\n    Prune in winter and remove faded blooms.\n    Protect from frost and watch for pests." },
    { "Geranium", "Geranium (Pelargonium spp.)\nColorful and hardy plant, perfect for borders and containers.\n\nCare Tips\n    Sun: Full sun or partial shade.\n    Water: Water regularly but allow soil to dry out between waterings.\n    Soil: Well-draining, fertile soil.\n    Fertilizer: Monthly during the growing season.\n    Prune regularly to encourage new growth.\n    Protect from frost and ensure good air circulation." },
    { "Monstera", "Monstera (Monstera deliciosa)\nTropical plant with large, split leaves, popular for indoor decoration.\n\nCare Tips\n    Sun: Indirect bright light.\n    Water: Keep soil slightly moist, water when topsoil is dry.\n    Soil: Well-draining, peat-based soil.\n    Fertilizer: Monthly in spring and summer.\n    Prune to control growth and remove damaged leaves.\n    Protect from direct sunlight and drafts." },
    { "Banana", "Banana (Musa spp.)\nLarge, tropical plant with broad, striking leaves. Ideal for warm climates.\n\nCare Tips\n    Sun: Full sun.\n    Water: Keep soil consistently moist.\n    Soil: Rich, well-draining soil.\n    Fertilizer: Regular feeding during the growing season.\n    Prune: Remove dead leaves to encourage new growth.\n    Protect from cold temperatures." },
    { "Lotus", "Lotus (Nymphaea spp.)\nWater plant known for its beautiful flowers that bloom on the surface of ponds.\n\nCare Tips\n    Sun: Full sun.\n    Water: Keep the roots submerged in water, but avoid covering the leaves.\n    Soil: Fertile, silty soil in the water.\n    Fertilizer: Use aquatic plant fertilizer during the growing season.\n    Prune: Remove faded flowers and dead leaves.\n    Protect from cold temperatures." },
    { "Bamboo", "Bamboo (Bambusoideae)\nFast-growing grass known for its tall, slender stems.\n\nCare Tips\n    Sun: Prefers partial shade to full sun.\n    Water: Regular watering, especially in dry periods.\n    Soil: Well-draining, slightly acidic soil.\n    Fertilizer: Fertilize in spring for best growth.\n    Prune: Trim to maintain shape and size.\n    Protect from freezing temperatures." },
    { "Coconut", "Coconut (Cocos nucifera)\nTropical tree known for its large fruits and tall, slender trunk.\n\nCare Tips\n    Sun: Full sun.\n    Water: Keep soil consistently moist but well-draining.\n    Soil: Sandy, well-draining soil.\n    Fertilizer: Use a balanced fertilizer regularly during the growing season.\n    Prune: Remove dead leaves and coconuts.\n    Protect from frost and ensure good airflow." }
};

    public void StartAnimation()
    {
        if (currentBee != null)
        {
            if (beeController != null)
            {
                beeController.StartReturnSequence();
            }
            else
            {
                Debug.LogWarning("BeeController not found on current bee.");
            }
            return;
        }
        if (beePrefab == null)
        {
            Debug.LogError("beePrefab non assegnato!");
            return;
        }

        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.6f, Camera.main.nearClipPlane + 5f));
        Vector3 nearCameraPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;

        GameObject beeInstance = Instantiate(beePrefab, spawnPosition, Quaternion.identity);
        
        beeInstance.transform.localScale = beeInstance.transform.localScale * 0.01f;
        currentBee = beeInstance;

        string plantName = GardenPlant.selectedPlant.Plant.Name;
        string beeText = plantTexts.ContainsKey(plantName) ? plantTexts[plantName] : "Bzzz! Sono un'ape!";

        TMPro.TMP_Text textComponent = beeInstance.GetComponentInChildren<TMPro.TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = beeText;
        }
        else
        {
            Debug.LogWarning("Non è stato trovato un componente TMP_Text nel prefab dell'ape.");
        }
        
        // Aggiungi il comportamento di movimento
        beeController = beeInstance.AddComponent<BeeController>();
        beeController.Setup(spawnPosition, nearCameraPosition);
    }
}