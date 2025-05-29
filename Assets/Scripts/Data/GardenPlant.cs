using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GardenPlant : MonoBehaviour
{
    public static GardenPlant selectedPlant;
    public static List<Collider> colliderList;
    private static Material highlightMaterial;
    private static Dictionary<GameObject, Material[]> originalMaterials = new();
    public Plant Plant { get; set; }
    
    public GameObject insectPrefab1 { get; set; }
    public GameObject insectPrefab2 { get; set; }
    public GameObject insectPrefab3 { get; set; }
    public GameObject insectPrefab4 { get; set; }

    public GameObject dropPrefab { get; set; }
    private GameObject dropObj;
    private List<GameObject> spawnedInsects = new List<GameObject>();
    public GameObject PlantObj { get; private set; }
    public GameObject VaseObj { get; private set; }
    private ParticleSystem vaseParticleSystem;
   

    public bool plantIsDead = false;
    private Color originalColor;


    private float _accumulatedCoins = 0;
    private int _coins = 0;
    public bool spawnedCoins = false;
    public int Coins
    {
        get => _coins;
        private set {
            _coins = value;
            if (_coins >= Plant.CoinPerSecond*30 && !spawnedCoins && TutorialUI.firstLaunch != 0) 
            {
                Debug.Log("Spawning coins...");
                SpawnCoins();
                spawnedCoins = true;
            }
        } 
    }
    private List<GameObject> coinGameobjects = new List<GameObject>();
    private static GameObject coinPrefab;
    private static readonly (Vector3 position, Quaternion rotation)[] coinSpawnOffsets = new (Vector3, Quaternion)[]
    {
        (new Vector3(0.00419999985024333f,0.12040000408887863f,-0.04270000755786896f), Quaternion.Euler(0, 0, 1)),
        (new Vector3(0,0.300000012f,-1.07000005f), Quaternion.Euler(20, 0, 0)),
        (new Vector3(0.689999998f,0.300000012f,-0.74000001f), Quaternion.Euler(20, -40, 0)),
        (new Vector3(0.949999988f,0.150000006f,0.730000019f), Quaternion.Euler(90, 0, 0)),
        (new Vector3(-0.920000017f,0.150000006f,0.519999981f), Quaternion.Euler(90, 0, 0)),
        (new Vector3(-0.456889004f,1.65999997f,0.25999999f), Quaternion.Euler(0, 45, 0)),
        (new Vector3(0.49000001f,1.74000001f,0.460000008f), Quaternion.Euler(20, 50, 0))                    
    };
    
    IEnumerator Start()
    {
        PlantObj = this.gameObject;
        VaseObj = this.gameObject.transform.Find("Vase").gameObject;
        vaseParticleSystem = VaseObj.GetComponentInChildren<ParticleSystem>();
        
        if (vaseParticleSystem == null)
        {
            Debug.LogError("ParticleSystem is missing from VaseObj.");
            
            foreach (Transform child in VaseObj.transform)
            {
                Debug.Log("Child: " + child.name);
            }
        }
        
        originalColor = PlantObj.GetComponent<Renderer>().material.color;
    
        if (colliderList == null)
            colliderList = new List<Collider>();

        colliderList.Add(PlantObj.GetComponent<Collider>());
        colliderList.Add(VaseObj.GetComponent<Collider>());
    
        if (highlightMaterial == null)
            highlightMaterial = Resources.Load<Material>("Shaders/Outline Material");
        
        Debug.Log("Created new GardenPlant");
        if (TutorialUI.firstLaunch == 0)
        {
            TutorialUI.selectedPlant = this;
            TutorialUI.onPlantPlaced.Invoke();
        } else if (!Plant.CurrentHealth.HasValue)
            Plant.CurrentHealth = Plant.Health;

		if (!Plant.CurrentLevel.HasValue) Plant.CurrentLevel = 0;

        while(true) 
        {
            yield return new WaitForSeconds(5f);
            if(spawnedInsects.Count == 0){
               TrySpawnInsect();
            } else {
               DecreaseHealth(3f);
            }
        }
    }

    private void DecreaseHealth(float amount)
    {
        Plant.DecreaseHealth(amount);
        if (selectedPlant == this)
            HealthBar.Instance.UpdateHealthBar(Plant.CurrentHealth.Value);
    }
    
    public void potionRevitalizing()
    {
        plantIsDead = false;
        Plant.SetMaxHealth();
        if (selectedPlant == this)
            HealthBar.Instance.UpdateHealthBar(Plant.CurrentHealth.Value);
        
        if(TutorialUI.firstLaunch != 0)
            AchievementsView.Instance.UnlockAchievement("Revitalize one plant");
        
		if(dropObj != null){
            Destroy(dropObj);
            dropObj = null;
		}
    }
    
    public void upgradeRevitalizing()
    {
        plantIsDead = false;
        Plant.SetMaxHealth();
        if (selectedPlant == this)
            HealthBar.Instance.UpdateHealthBar(Plant.CurrentHealth.Value);
        
        if(dropObj != null){
            Destroy(dropObj);
            dropObj = null;
        }
    }
    
    

    void Update()
    {   
     
        if (DetectTouch())
        {
            SetSelectedPlant(this);
            
            if (CanCollectCoins())
            {
                StartCoroutine(AnimateCoinsToWallet());
            }
        }

        if (Plant.CurrentHealth.Value > 0){
            DecreaseHealth(1f * Time.deltaTime);
            _accumulatedCoins += 1 * Time.deltaTime;
			Plant.EarnCoins(1 * Time.deltaTime);
			if (this == selectedPlant)
                    PlantUI.Instance.UpdateLevel();
            if (_accumulatedCoins >= 1)
            {
                Coins += Plant.CoinPerSecond*(Plant.CurrentLevel.Value+1);
                _accumulatedCoins = 0;
            }
        }
        
        if (Plant.CurrentHealth.Value <= 0){
            plantIsDead = true;
            Color darkGrey = new Color(0.2f, 0.2f, 0.2f); 
            SetPlantColor(darkGrey); 
        } else if (Plant.CurrentHealth.Value <= Plant.getHealth()*0.2 && dropObj == null){
            Collider plantCollider = PlantObj.GetComponent<Collider>();
            Vector3 topOfPlant = plantCollider.bounds.center + new Vector3(0, plantCollider.bounds.extents.y, 0) + new Vector3(0, 0.15f, 0); ;
            dropObj = Instantiate(dropPrefab, topOfPlant, Quaternion.identity);
        } else if (Plant.CurrentHealth.Value >= 0 && PlantObj.GetComponent<Renderer>().materials[0].color != originalColor){
            plantIsDead = false;
            SetPlantColor(originalColor);
            TutorialUI.onPlantRevived.Invoke();
        } else if (Plant.CurrentHealth.Value > Plant.getHealth()*0.2 && dropObj != null)
        {
            Destroy(dropObj);
            dropObj = null;
        }
        
        if (Plant.CurrentHealth.Value > Plant.getHealth() * 0.8) {
            if (!vaseParticleSystem.isPlaying) {
                vaseParticleSystem.Play();
            }
        } else {
            if (vaseParticleSystem.isPlaying) {
                vaseParticleSystem.Stop();
            }
        }

    }
    
    public static void SetSelectedPlant(GardenPlant plant)
    {
        if (plant == selectedPlant) return;

        // removing highlight material from previous plant
        if (selectedPlant != null)
            HighlightSelectedPlant(false);

        if (plant == null)
        {
            selectedPlant = null;
            PlantUI.ShowUI(false);
            return;
        }

        selectedPlant = plant;
        HighlightSelectedPlant(true);
        TutorialUI.onPlantHightlighted.Invoke();

        if (PlantUI.isActive() == false)
            PlantUI.ShowUI(true);
        HealthBar.Instance.SetMaxHealth(plant.Plant.Health);
        HealthBar.Instance.UpdateHealthBar(plant.Plant.CurrentHealth.Value);
    } 
    
private static void HighlightSelectedPlant(bool highlight)
{
    List<GameObject> objects = new List<GameObject>
    {
        selectedPlant.PlantObj,
        selectedPlant.VaseObj
    };

    foreach (GameObject obj in objects)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer == null) continue;

        if (highlight)
        {
            // Crea un nuovo GameObject figlio per highlight
            GameObject outlineObj = new GameObject("Outline_" + obj.name);
            outlineObj.transform.SetParent(obj.transform);
            outlineObj.transform.localPosition = Vector3.zero;
            outlineObj.transform.localRotation = Quaternion.identity;
            outlineObj.transform.localScale = Vector3.one;

            MeshFilter originalMesh = obj.GetComponent<MeshFilter>();
            MeshFilter outlineMesh = outlineObj.AddComponent<MeshFilter>();
            outlineMesh.sharedMesh = originalMesh.sharedMesh;

            MeshRenderer outlineRenderer = outlineObj.AddComponent<MeshRenderer>();
            int subMeshCount = renderer.sharedMaterials.Length;

            Material[] outlineMaterials = Enumerable.Repeat(highlightMaterial, subMeshCount).ToArray();
            outlineRenderer.materials = outlineMaterials;

            outlineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            outlineRenderer.receiveShadows = false;
        }
        else
        {
            Transform outlineTransform = obj.transform.Find("Outline_" + obj.name);
            if (outlineTransform != null)
                GameObject.Destroy(outlineTransform.gameObject);
        }
    }
}

    
    
    private bool DetectTouch()
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
                    if (uiHit)
                        return false;

                    GameObject hitObject = hit.collider.gameObject;
                    
                    if (hitObject == PlantObj || hitObject == VaseObj)
                        return true;       
                }
            }
        }
        return false;
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

    void OnParticleCollision(GameObject other)
    {
        if (Plant == null || plantIsDead) return;

        Plant.IncreaseHealth(1f);

        if (selectedPlant == this)
            HealthBar.Instance.UpdateHealthBar(Plant.CurrentHealth.Value);
    }
    
    private void TrySpawnInsect()
    {
        if (spawnedInsects.Count >= 10 || TutorialUI.firstLaunch == 0) return; 
        float spawnChance = UnityEngine.Random.Range(0f, 100f);
        if (spawnChance <= 2f)  // 2% di probabilità ogni 5 secondi
        {
            SpawnInsects(10 - spawnedInsects.Count);
        }
    }
    
    
    public void SpawnInsects(int numberOfInsects)
    {
        float insectSpawnRadius = 0.1f;
        float insectSpawnHeight = VaseObj.GetComponent<Collider>().bounds.size.y + PlantObj.GetComponent<Collider>().bounds.size.y;
        for (int i = 0; i < numberOfInsects; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(
                Random.Range(-insectSpawnRadius, insectSpawnRadius), 
                insectSpawnHeight,
                Random.Range(-insectSpawnRadius, insectSpawnRadius)
            );
            
            GameObject insect = null;
            int insectType = UnityEngine.Random.Range(0, 4);

            switch (insectType)
            {
        		case 0:
            		insect = Instantiate(insectPrefab1, randomPosition, Quaternion.identity);
					insect.transform.localScale /= 50f;
           		    break;
        		case 1:
            		insect = Instantiate(insectPrefab2, randomPosition, Quaternion.identity);
					insect.transform.localScale /= 50f;
            		break;
       			case 2:
          		 	insect = Instantiate(insectPrefab3, randomPosition, Quaternion.identity);
                    insect.transform.localScale /= 50f;
            		break;
        		case 3:
            		insect = Instantiate(insectPrefab4, randomPosition, Quaternion.identity);
                    insect.transform.localScale /= 500f;
            		break;
        		default:
            		Debug.Log("Error: non valid number insect spawn switch");
            		break;
    		}
            spawnedInsects.Add(insect);

            // Aggiungi il comportamento di volo attorno alla pianta
            FlyAround flyScript = insect.GetComponent<FlyAround>();
            if (flyScript != null)
            {
                flyScript.target = this.transform;  // La pianta diventa il target
                flyScript.radius = insectSpawnRadius;  // Imposta il raggio di volo
                flyScript.speed = Random.Range(0.5f, 2f);  // Velocità casuale per ogni insetto
                switch (insectType)
                {
                    case 1:
                    case 2:
                        flyScript.rotation = true;
                        break;
                    default:
                        flyScript.rotation = false;
						break;
                }
            }
            
        }
    }
    
    public void RemoveInsects()
    {
        foreach (GameObject insect in spawnedInsects)
            Destroy(insect);
        spawnedInsects.Clear();
        if(TutorialUI.firstLaunch != 0) 
            AchievementsView.Instance.UnlockAchievement("Kill the Insects");
    }
    
    private void SetPlantColor(Color color)
    {
        List<GameObject> objects = new List<GameObject> { PlantObj, VaseObj };
        foreach (GameObject obj in objects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.color = color;  // Imposta un colore grigio o marrone
                }
            }
        }
    }
    
    public bool IsDead() {
        return plantIsDead;
    }

    public bool IsInfected() {
        return spawnedInsects.Count > 0;
    }
    public void SpawnCoins()
    {
        if (coinPrefab == null)
        {
            coinPrefab = Resources.Load<GameObject>("Prefabs/GoldCoin/Coin");
            coinPrefab.gameObject.transform.localScale = new Vector3(0.7142857909202576f, 0.7142857909202576f, 0.7142857909202576f);
        }

        foreach (var spawnOffset in coinSpawnOffsets)
        {
            Vector3 worldPos = VaseObj.transform.TransformPoint(spawnOffset.position);
            GameObject coin = Instantiate(coinPrefab, worldPos, spawnOffset.rotation, VaseObj.transform);
            coinGameobjects.Add(coin);
        }
    }

    public bool CanCollectCoins()
    {
        return coinGameobjects.Count > 0;
    }
    
    public IEnumerator AnimateCoinsToWallet()
    {
        int totalCoins = Coins;
        int baseAmount = totalCoins / coinGameobjects.Count;
        int remainder = totalCoins % coinGameobjects.Count;
        int index = 0;
        foreach (GameObject coin in coinGameobjects)
        {
            coin.GetComponent<AudioSource>().Play();
            int amount = baseAmount + (index < remainder ? 1 : 0); // Aggiunge 1 solo ai primi 'remainder' coin
            index++;
                
            Vector3 startPosition = coin.transform.position;
                
            float duration = 0.5f; // durata dell'animazione
            float elapsedTime = 0f;
    
            // Animazione dei coins verso il wallet
            while (elapsedTime < duration)
            {
                Vector3 targetScreenPosition = new Vector3(0, Screen.height*0.9f, Camera.main.nearClipPlane);
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);
                coin.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                coin.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.05f, 0.05f, 0.05f), elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
    
            Wallet.Instance.AddMoney(amount);
            coin.GetComponent<AudioSource>().Stop();
            Destroy(coin);
        }
    
        coinGameobjects.Clear();
        spawnedCoins = false; // Permette di spawnare nuovi coin
            
        Debug.Log("Colleccted: " + Coins + " coins");
        Coins = 0;
        TutorialUI.onCoinsCollected.Invoke();
            
    }

    public static float CalculatePlantProgress()
    {
        GardenPlant selectedPlant = GardenPlant.selectedPlant;
		float value = (float) (selectedPlant.Plant.EarnedCoins / (System.Math.Pow(selectedPlant.Plant.Price, 1.1) + selectedPlant.Plant.Health));
        if(selectedPlant.Plant.CurrentLevel.Value > 0)
            value /= 4;
        return value;
    }
}
