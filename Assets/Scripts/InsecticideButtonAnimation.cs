using UnityEngine;

public class InsecticideButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject insecticidePrefab;
    [SerializeField] private Vector3 offset;

    public void StartAnimation()
    {
        if (!InsecticideController.isAvailable) return;
        Debug.Log("Starting animation...");

        Vector3 plantPosition = GardenPlant.selectedPlant.gameObject.transform.position;
        Vector3 position = plantPosition + offset;
        
        GameObject insecticide = Instantiate(insecticidePrefab, position, Quaternion.identity);
        insecticide.transform.localScale = Vector3.one * 0.01f;
        RotateTowardsPlant(insecticide, plantPosition);
        
        //GardenPlant.selectedPlant?.RemoveInsects();
    }
    private void RotateTowardsPlant(GameObject obj, Vector3 plantPosition)
    {
        Vector3 direction = plantPosition - obj.transform.position;
        direction.y = 0; // 0 rotation on the y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        obj.transform.rotation = targetRotation;
    }
}