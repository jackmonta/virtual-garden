using UnityEngine;

public class PotionButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Vector3 offset;

    public void StartAnimation() 
    {
        //if (!PotionController.isAvailable) return;
        //Debug.Log("Starting animation...");

        Vector3 plantPosition = GardenPlant.selectedPlant.gameObject.transform.position;
        Vector3 position = plantPosition + offset;
        
        GameObject potion = Instantiate(potionPrefab, position, Quaternion.identity);
        potion.transform.localScale = Vector3.one * 0.3f;
        potion.AddComponent<ForcePosition>().Initialize(position);
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

    public void Initialize(Vector3 position)
    {
        targetPosition = position;
    }

    private void LateUpdate()
    {
        transform.position = targetPosition; // Forza la posizione corretta
    }
}