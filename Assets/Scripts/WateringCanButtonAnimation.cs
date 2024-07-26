using UnityEngine;
using System.Collections;

public class WaterinCanButtonAnimation : MonoBehaviour
{
    [SerializeField] private GameObject wateringCanPrefab; // Il prefab da istanziare
    [SerializeField] private Vector3 offset;
    private static bool isAvailable;

    private void Start()
    {
        isAvailable = true;
    }

    public void StartAnimation()
    {
        if (!isAvailable) return;
        isAvailable = false;
        Debug.Log("Starting animation...");

        Vector3 plantPosition = GardenPlant.selectedPlant.gameObject.transform.position;
        Vector3 position = plantPosition + offset;
        
        GameObject wateringCan = Instantiate(wateringCanPrefab, position, Quaternion.identity);
        wateringCan.transform.localScale = Vector3.one * 0.3f;
        //AlignWithCamera(wateringCan);
        RotateTowardsPlant(wateringCan, plantPosition);
        
        ParticleSystem ps = wateringCan.GetComponentInChildren<ParticleSystem>();
        StartCoroutine(DestroyAfterParticleSystem(ps, wateringCan));
    }
    
    private IEnumerator DestroyAfterParticleSystem(ParticleSystem ps, GameObject obj)
    {
        // Attendi la durata del Particle System
        yield return new WaitForSeconds(ps.main.duration);
        
        Destroy(obj);
        isAvailable = true;
    }
    
    private void AlignWithCamera(GameObject obj)
    {
        // Ottieni la fotocamera principale
        Camera camera = Camera.main;
        
        // Calcola la direzione verso la fotocamera
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // Mantieni solo la componente orizzontale
        
        // Calcola la rotazione necessaria per allineare l'oggetto con la direzione della fotocamera
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        targetRotation *= Quaternion.Euler(0, 90, 0);
        
        // Applica la rotazione al prefab
        obj.transform.rotation = targetRotation;
        
        //Vector3 offset = - obj.transform.forward * 0.2f;
        //obj.transform.position += offset;
    }

    private void RotateTowardsPlant(GameObject obj, Vector3 plantPosition)
    {
        Vector3 direction = plantPosition - obj.transform.position;
        direction.y = 0; // 0 rotation on the y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        obj.transform.rotation = targetRotation;
    }
}