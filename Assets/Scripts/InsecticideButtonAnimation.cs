using UnityEngine;

public class InsecticideButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject insecticidePrefab;
    [SerializeField] private Vector3 offset;
    [SerializeField] private NonPlantItem nonPlantItem;
    
    private GardenPlant insecticideSelectedPlant;

    public void StartAnimation()
    {
        if (!InsecticideController.isAvailable) return;
        
        Debug.Log("Starting insecticide animation...");
        insecticideSelectedPlant = GardenPlant.selectedPlant;
        if (!insecticideSelectedPlant.IsInfected()) return;
        
        if(nonPlantItem.ClickCount <= 0) return;
        nonPlantItem.DecrementCounter();
        Debug.Log(nonPlantItem.Name + " selected " + nonPlantItem.ClickCount + " times. animation started");

        Vector3 plantPosition = insecticideSelectedPlant.gameObject.transform.position;
        Vector3 position = plantPosition + offset;
        
        GameObject insecticide = Instantiate(insecticidePrefab, position, Quaternion.identity);
        insecticide.transform.localScale = Vector3.one * 0.01f;
        RotateTowardsPlant(insecticide, plantPosition);
    }
    private void RotateTowardsPlant(GameObject obj, Vector3 plantPosition)
    {
        Vector3 direction = plantPosition - obj.transform.position;
        direction.y = 0; // 0 rotation on the y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        obj.transform.rotation = targetRotation;
    }
}