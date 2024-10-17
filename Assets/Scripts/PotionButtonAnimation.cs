using UnityEngine;
using TMPro; 

public class PotionButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Vector3 offset;
    [SerializeField] private NonPlantItem nonPlantItem; 
    [SerializeField] private TextMeshProUGUI notificationText; 
    
    private GardenPlant potionSelectedPlant;

    public void StartAnimation() 
    {
        Debug.Log("Starting potion animation...");
        potionSelectedPlant = GardenPlant.selectedPlant;
        if (!potionSelectedPlant.IsDead()) return;
        
        if(nonPlantItem.ClickCount <= 0) return;
        nonPlantItem.DecrementCounter();
        notificationText.text = nonPlantItem.ClickCount.ToString();
        Debug.Log(nonPlantItem.Name + " selected " + nonPlantItem.ClickCount + " times. animation started");

        Vector3 plantPosition = potionSelectedPlant.gameObject.transform.position;
        Vector3 position = plantPosition + offset;
        
        GameObject potion = Instantiate(potionPrefab, position, Quaternion.identity);
        potion.AddComponent<ForcePosition>().Initialize(position, potionSelectedPlant);
        RotateTowardsPlant(potion, plantPosition);
    }
    private void RotateTowardsPlant(GameObject obj, Vector3 plantPosition)
    {
        Vector3 direction = plantPosition - obj.transform.position;
        direction.y = 0; // 0 rotation on the y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        obj.transform.rotation = targetRotation;
    }
}

public class ForcePosition : MonoBehaviour
{
    private Vector3 targetPosition;
    private GardenPlant potionSelectedPlant;
    private float lifeTime = 2.3f; // Durata vita dell'oggetto in secondi

    public void Initialize(Vector3 position, GardenPlant potionSelectedPlant)
    {
        targetPosition = position;
        this.potionSelectedPlant = potionSelectedPlant;
        
    }

    private void LateUpdate()
    {
        transform.position = targetPosition; // Forza la posizione corretta
        
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) {
            Debug.Log("Tempo scaduto. Distruggo l'oggetto.");
            Destroy(gameObject);
            potionSelectedPlant.potionRevitalizing();
        }
    }
}