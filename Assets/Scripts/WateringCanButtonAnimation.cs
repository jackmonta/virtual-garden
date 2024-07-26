using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.XR.ARFoundation;

public class WaterinCanButtonAnimation : MonoBehaviour
{
    public GameObject wateringCanPrefab; // Il prefab da istanziare
    private bool isEnabled = false; // Variabile per abilitare/disabilitare l'istanziazionee

    void Update()
    {
        if (isEnabled && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && !IsPointerOverUIObject(touch))
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    
                    // Verifica se l'oggetto ha un componente ARPlane
                    if (hitObject.GetComponent<ARPlane>() != null)
                    {
                        Debug.Log("Tocco su ARPlane ignorato.");
                        return; // Ignora questo tocco
                    }
                    
                    Vector3 position = hitObject.transform.position + Vector3.up * 0.2f;
                    
                    GameObject wateringCan = Instantiate(wateringCanPrefab, position, Quaternion.identity);
                    wateringCan.transform.localScale = Vector3.one * 0.3f;
                    AlignWithCamera(wateringCan);
                    
                    ParticleSystem ps = wateringCan.GetComponentInChildren<ParticleSystem>();
                    if (ps != null)
                    {
                        StartCoroutine(DestroyAfterParticleSystem(ps, wateringCan));
                    }
                    else
                    {
                        Destroy(wateringCan); // Distruggi immediatamente se non c'è un Particle System
                    }
                }
            }
        }
    }

    // Metodo per abilitare l'istanziazione
    public void EnableInstantiation()
    {
        isEnabled = true;
    }
    
    private bool IsPointerOverUIObject(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
    
    private IEnumerator DestroyAfterParticleSystem(ParticleSystem ps, GameObject obj)
    {
        // Attendi la durata del Particle System
        yield return new WaitForSeconds(ps.main.duration);
        
        // Distruggi l'oggetto
        Destroy(obj);
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
}