using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class Garden : MonoBehaviour
{
    [SerializeField] private Material gardenMaterial;
    [SerializeField] private GameObject vasePrefab;
    [SerializeField] private GameObject dropPrefab;
	[SerializeField] private GameObject insectPrefab1;
    [SerializeField] private GameObject insectPrefab2;
	[SerializeField] private GameObject insectPrefab3;
	[SerializeField] private GameObject insectPrefab4;

	[SerializeField] private GameObject butterflyPrefab1;
	[SerializeField] private GameObject butterflyPrefab2;
	[SerializeField] private GameObject butterflyPrefab3;
    private List<GameObject> spawnedButterflies = new List<GameObject>();

	[SerializeField] private GameObject coinPrefab;

    public AudioSource audioSource;

	private float butterflySpawnTimer = 0f; // Timer per lo spawn delle farfalle
	private float spawnInterval = 15f; 
    
    public static Garden Instance { get; set; }
    private ARPlane plane;
    private Dictionary<Plant, GameObject> plants;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        plants = new Dictionary<Plant, GameObject>();
    }

    private void Update()
    {
        if (plane == null) return;
        
        // garden touch
        var touchPoint = DetectGardenTouch();
        if (touchPoint.HasValue && Inventory.GetSelectedPlant() != null)
        {
            Vector3 touchPosition = DetectGardenTouch().Value;
            Plant plantToSpawn = Inventory.GetSelectedPlant();
            GameObject plantObj = SpawnPlant(plantToSpawn, touchPosition);

            if (plantObj != null)
            {
                plants.Add(plantToSpawn, plantObj);
                Inventory.RemoveSelectedPlant();
            }
            return;
        }

        // hit outside the garden
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !IsPointerOverUI(Input.GetTouch(0).position)){
            if(TutorialUI.selectedPlant == null)
                GardenPlant.SetSelectedPlant(null);
            
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
       		if (Physics.Raycast(ray, out RaycastHit hit))
        	{
            	if (hit.collider.gameObject.CompareTag("Butterfly")) // Controlla se è una farfalla
            	{
                	TransformButterflyToCoin(hit.collider.gameObject);
            	}
        	}
		}


		butterflySpawnTimer += Time.deltaTime; // Aggiungi il tempo trascorso

    	if (butterflySpawnTimer >= spawnInterval && spawnedButterflies.Count <= 10) // Ogni 10 secondi
    	{
       		SpawnButterfly();
        	butterflySpawnTimer = 0f; // Resetta il timer
    	}
    }
	
	private void TransformButterflyToCoin(GameObject butterfly)
    {
        Vector3 position = butterfly.transform.position;
        Quaternion rotation = butterfly.transform.rotation;

        Destroy(butterfly); // Rimuovi la farfalla
    	
        if (coinPrefab == null)
        {
            Debug.LogError("Coin prefab not found!");
            return;
        }

        GameObject coin = Instantiate(coinPrefab, position, rotation);

        StartCoroutine(AnimateCoinToWallet(coin)); // Avvia l'animazione
    }


    private IEnumerator AnimateCoinToWallet(GameObject coin)
    {
        coin.GetComponent<AudioSource>().Play();
        
        float duration = 0.5f; // Durata dell'animazione
        float elapsedTime = 0f;

        Vector3 startPosition = coin.transform.position;
        Vector3 targetScreenPosition = new Vector3(0, Screen.height * 0.9f, Camera.main.nearClipPlane);
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);

        while (elapsedTime < duration)
        {
            coin.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            coin.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.02f, 0.02f, 0.02f), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Wallet.Instance.AddMoney(1);
        coin.GetComponent<AudioSource>().Stop();
        Destroy(coin);
    }
    /*
    public void UpgradePlant()
    {
        Plant plant = GardenPlant.selectedPlant.Plant;
        
        if (!plants.ContainsKey(plant))
        {
            Debug.LogError("Plant not found in dictionary.");
            return;
        }
        
        GameObject oldPlantObj = plants[plant];
        GameObject vaseObj = oldPlantObj.transform.GetChild(0).gameObject;
        
        Collider oldPlantCollider = oldPlantObj.GetComponent<Collider>();
        Collider oldVaseCollider = vaseObj.GetComponent<Collider>();
        if (oldPlantCollider != null) GardenPlant.colliderList.Remove(oldPlantCollider);
        if (oldVaseCollider != null) GardenPlant.colliderList.Remove(oldVaseCollider);
        
        plant.Upgrade();
        
        float vaseHeight = vaseObj.GetComponentInChildren<Renderer>().bounds.size.y;
        Vector3 vaseTopPosition = vaseObj.transform.position + new Vector3(0, vaseHeight / 2, 0);

        GameObject plantObj = Instantiate(plant.Prefab, vaseTopPosition, Quaternion.identity);
        plantObj.name = "Plant";
        
        plantObj.transform.localScale = Vector3.one * 0.07f;
        vaseObj.transform.SetParent(plantObj.transform);
        
        GardenPlant gardenPlant = plantObj.AddComponent<GardenPlant>();
        gardenPlant = oldPlantObj.GetComponent<GardenPlant>();
        GardenPlant.selectedPlant = null;
        
        Debug.Log("Plant upgraded.");
        
        if (plantObj != null)
        {
            plants[plant] = plantObj;
        }
        Destroy(oldPlantObj);
    }*/
    
    public void UpgradePlant(Plant plant)
    {
        if (!plants.ContainsKey(plant))
        {
            Debug.LogError("Plant not found in dictionary.");
            return;
        }
        
        GameObject oldPlantObj = plants[plant];
        GameObject vaseObj = oldPlantObj.transform.GetChild(0).gameObject;
        Vector3 vasePosition = vaseObj.transform.position;
        
        Collider oldPlantCollider = oldPlantObj.GetComponent<Collider>();
        Collider oldVaseCollider = vaseObj.GetComponent<Collider>();
        if (oldPlantCollider != null) GardenPlant.colliderList.Remove(oldPlantCollider);
        if (oldVaseCollider != null) GardenPlant.colliderList.Remove(oldVaseCollider);
        
        Destroy(oldPlantObj);
        Destroy(vaseObj);
        
        GameObject plantObj = SpawnPlant(plant, vasePosition);
        
        Debug.Log("Plant upgraded.");
        
        if (plantObj != null)
        {
            plants[plant] = plantObj;
        }
        else
        {
            plants.Remove(plant);
            GardenPlant.SetSelectedPlant(null);
            Inventory.AddPlant(plant);
        }
        
    }

    private GameObject SpawnPlant(Plant plantToSpawn, Vector3 touchPosition)
    {
        GameObject vaseObj = SpawnPrefab(vasePrefab, touchPosition);
        if (vaseObj == null)
        {
            Debug.Log("Vase collision detected");
            return null;
        }
        vaseObj.name = "Vase";

        float vaseHeight = vaseObj.GetComponentInChildren<Renderer>().bounds.size.y;
        Vector3 vaseTopPosition = vaseObj.transform.position + new Vector3(0, vaseHeight / 2, 0);

        GameObject plantObj = Instantiate(plantToSpawn.Prefab, vaseTopPosition, plantToSpawn.Prefab.transform.rotation);
        plantObj.name = "Plant";
        
        plantObj.transform.localScale = Vector3.one * 0.07f;
        vaseObj.transform.SetParent(plantObj.transform);
        
        GardenPlant gardenPlant = plantObj.AddComponent<GardenPlant>();
        gardenPlant.Plant = plantToSpawn;
        gardenPlant.dropPrefab = dropPrefab;
        gardenPlant.insectPrefab1 = insectPrefab1;
		gardenPlant.insectPrefab2 = insectPrefab2;
		gardenPlant.insectPrefab3 = insectPrefab3;
		gardenPlant.insectPrefab4 = insectPrefab4;
        return plantObj;
    }

    GameObject SpawnPrefab(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        // checking collisions
        if (GardenPlant.colliderList != null)
        {
            Collider collider = obj.GetComponent<Collider>();

            foreach (Collider otherCollider in GardenPlant.colliderList)    
                if (collider.bounds.Intersects(otherCollider.bounds))
                {
                    Destroy(obj);
                    return null;
                }
        }

        return obj;
    }

    private Vector3? DetectGardenTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    bool uiHit = IsPointerOverUI(touch.position);
                    bool gardenHit = hit.collider.gameObject == plane.GetComponent<MeshCollider>().gameObject;

                    if (!uiHit && gardenHit)
                        return hit.point;
                    
                    return null;
                }
            }
        }
        return null;
    }

    private bool IsPointerOverUI(Vector2 pos)
    {   
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = pos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    public void SetGardenPlane(ARPlane plane)
    {
        this.plane = plane;

        // Applying material
        GameObject planeObj = this.plane.gameObject;
        MeshRenderer renderer = planeObj.GetComponent<MeshRenderer>();
        renderer.material = gardenMaterial;

        audioSource.Play();  //birds

        InventoryUI.ShowUI();
        ShopUI.Instance.ShowUI();
        TutorialUI.ShowUI();
        SettingsUI.Instance.ShowUI();
    }
    
    public List<Plant> GetCopyPlantList()
    {
        List<Plant> copy = new List<Plant>();
        foreach (GameObject plantObj in plants.Values)
        {
            Plant p = plantObj.gameObject.GetComponent<GardenPlant>().Plant;
            copy.Add(p);
        }
        return copy;
    }

    public void Reset()
    {
        plants = new Dictionary<Plant, GameObject>();
        GardenPlant.colliderList = new List<Collider>();
        GardenPlant.selectedPlant = null;
    }

    private void SpawnButterfly()
    {
        if (plane == null) return;

        Vector3 planeCenter = plane.transform.position;
        float randomX = Random.Range(-5f, 5f) * plane.size.x;
        float randomZ = Random.Range(-5f, 5f) * plane.size.y;
        Vector3 spawnPosition = new Vector3(planeCenter.x + randomX, planeCenter.y + 3f, planeCenter.z + randomZ);

		GameObject butterfly = null;
		int insectType = UnityEngine.Random.Range(0, 3);

        switch (insectType)
        {
        	case 0:
            	butterfly = Instantiate(butterflyPrefab1, spawnPosition, Quaternion.identity);
    			butterfly.transform.localScale /= 15f; 
           		break;
        	case 1:
            	butterfly = Instantiate(butterflyPrefab2, spawnPosition, Quaternion.identity);
    			butterfly.transform.localScale /= 15f; 
            	break;
       		case 2:
          		butterfly = Instantiate(butterflyPrefab3, spawnPosition, Quaternion.identity);
    			butterfly.transform.localScale /= 250f; 
            	break;
        	default:
            	Debug.Log("Error: non valid number insect spawn switch");
            	break;
    	} 

		if (butterfly.GetComponent<Collider>() == null)
    	{
        	butterfly.AddComponent<BoxCollider>().isTrigger = true;
    	}
		
	    spawnedButterflies.Add(butterfly);

        StartCoroutine(MoveButterflyRandomly(butterfly));
    }


    private IEnumerator MoveButterflyRandomly(GameObject butterfly)
    {
        float maxRadius = 3f;
        float maxHeight = 3f;

        Vector3 velocity = Vector3.zero;;
        float acceleration = 1f;
        float maxSpeed = 1f;
        float maxTurnSpeed = 0.3f; // Limita quanto velocemente può cambiare direzione

        Vector3 planeCenter = plane.transform.position;
        
        while (butterfly != null)  
        {
            Vector3 targetPosition;
            do
            {
                targetPosition = planeCenter + new Vector3(
                    Random.Range(-maxRadius, maxRadius),
                    Random.Range(0.5f, maxHeight),
                    Random.Range(-maxRadius, maxRadius)
                );
            } while (Vector3.Distance(planeCenter, targetPosition) > maxRadius);

            while (Vector3.Distance(butterfly.transform.position, targetPosition) > 0.1f)
            {
                // Direzione verso il target
                Vector3 desiredVelocity = (targetPosition - butterfly.transform.position).normalized * maxSpeed;

                // Applica una transizione dolce con una piccola accelerazione
                velocity = Vector3.Lerp(velocity, desiredVelocity, acceleration * Time.deltaTime);
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed); 

                // Movimento
                butterfly.transform.position += velocity * Time.deltaTime;

                // Rotazione dolce con limiti sulla velocità di rotazione
                if (velocity.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(velocity);
                    butterfly.transform.rotation = Quaternion.Slerp(
                        butterfly.transform.rotation, 
                        targetRotation, 
                        maxTurnSpeed * Time.deltaTime
                    );
                }

                yield return null;
            }
        }
    }
}