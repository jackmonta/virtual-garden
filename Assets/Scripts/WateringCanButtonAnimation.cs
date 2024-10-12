using UnityEngine;

public class WaterinCanButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject wateringCanPrefab;
    [SerializeField] private Vector3 offset;

    public void StartAnimation()
    {
        if (!WateringCanController.isAvailable) return;
        Debug.Log("Starting watering can animation...");

        Vector3 plantPosition = GardenPlant.selectedPlant.gameObject.transform.position;
        Vector3 position = plantPosition + offset;
        
        GameObject wateringCan = Instantiate(wateringCanPrefab, position, Quaternion.identity);
        wateringCan.transform.localScale = Vector3.one * 0.3f;
        RotateTowardsPlant(wateringCan, plantPosition);
    }
    private void RotateTowardsPlant(GameObject obj, Vector3 plantPosition)
    {
        Vector3 direction = plantPosition - obj.transform.position;
        direction.y = 0; // 0 rotation on the y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        obj.transform.rotation = targetRotation;
    }
}