using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GardenPlant : MonoBehaviour
{
    public static GardenPlant selectedPlant;
    public static List<Collider> colliderList;
    private static Material highlightMaterial;
    public Plant Plant { get; set; }
    
    public GameObject insectPrefab { get; set; }
    public GameObject dropPrefab { get; set; }
    private GameObject dropObj;
    
    private List<GameObject> spawnedInsects = new List<GameObject>();
    public GameObject PlantObj { get; private set; }
    public GameObject VaseObj { get; private set; }

    public bool plantIsDead = false;
    private Color originalColor;
    
    
    IEnumerator Start()
    {
        PlantObj = this.gameObject;
        VaseObj = this.gameObject.transform.Find("Vase").gameObject;
        
        originalColor = PlantObj.GetComponent<Renderer>().material.color;
    
        if (colliderList == null)
            colliderList = new List<Collider>();

        colliderList.Add(PlantObj.GetComponent<Collider>());
        colliderList.Add(VaseObj.GetComponent<Collider>());
    
        if (highlightMaterial == null)
            highlightMaterial = Resources.Load<Material>("Shaders/Outline Material");
        
        Debug.Log("Created new GardenPlant");

        while(true) 
        {
            yield return new WaitForSeconds(3f);
            if(spawnedInsects.Count == 0){
               TrySpawnInsect();
            } else {
               DecreaseHealth(2f);
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
    }
    
    

    void Update()
    {
        DecreaseHealth(1f * Time.deltaTime);
        
        if (DetectTouch())
            SetSelectedPlant(this);
        
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
        } else if (Plant.CurrentHealth.Value > Plant.getHealth()*0.2 && dropObj != null)
        {
            Destroy(dropObj);
            dropObj = null;
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

        if (highlight)
        {
            foreach (GameObject obj in objects)
            {
                Material[] materials = obj.GetComponent<MeshRenderer>().materials;
                Debug.Log("Original materials for " + obj.name + ": " + string.Join(", ", materials.Select(m => m.name)));
                List<Material> materialList = new List<Material>(materials)
                {
                    highlightMaterial
                };
                obj.GetComponent<MeshRenderer>().materials = materialList.ToArray();
                Debug.Log("Updated materials for " + obj.name + ": " + string.Join(", ", obj.GetComponent<MeshRenderer>().materials.Select(m => m.name)));
                Debug.Log("HIGHLIGHTED " + obj.name);
            }
        }
        else
        {
            foreach (GameObject obj in objects)
            {
                Material[] materials = obj.GetComponent<MeshRenderer>().materials;
                obj.GetComponent<MeshRenderer>().materials = materials.Take(materials.Length - 1).ToArray();
                Debug.Log("UN-HIGHLIGHTED " + obj.name);
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
                    GameObject hitObject = hit.collider.gameObject;
                    
                    if (hitObject == PlantObj || hitObject == VaseObj)
                        return true;       
                }
            }
        }
        return false;
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
        if (spawnedInsects.Count >= 10) return; 
        float spawnChance = UnityEngine.Random.Range(0f, 100f);
        if (spawnChance <= 5f)  // 5% di probabilità ogni 3 secondi
        {
            SpawnInsects(10 - spawnedInsects.Count);
        }
    }
    
    // Metodo per spawnare 20 insetti attorno alla pianta
    private void SpawnInsects(int numberOfInsects)
    {
        float insectSpawnRadius = 0.1f;
        float insectSpawnHeight = VaseObj.GetComponent<Collider>().bounds.size.y + PlantObj.GetComponent<Collider>().bounds.size.y;
        for (int i = 0; i < numberOfInsects; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(
                UnityEngine.Random.Range(-insectSpawnRadius, insectSpawnRadius), 
                insectSpawnHeight,
                UnityEngine.Random.Range(-insectSpawnRadius, insectSpawnRadius)
            );

            GameObject insect = Instantiate(insectPrefab, randomPosition, Quaternion.identity);
            insect.transform.localScale /= 50f;
            spawnedInsects.Add(insect);

            // Aggiungi il comportamento di volo attorno alla pianta
            FlyAround flyScript = insect.GetComponent<FlyAround>();
            if (flyScript != null)
            {
                flyScript.target = this.transform;  // La pianta diventa il target
                flyScript.radius = insectSpawnRadius;  // Imposta il raggio di volo
                flyScript.speed = UnityEngine.Random.Range(0.5f, 2f);  // Velocità casuale per ogni insetto
            }
            
        }
    }
    
    public void RemoveInsects()
    {
        foreach (GameObject insect in spawnedInsects)
            Destroy(insect);
        spawnedInsects.Clear();
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
}
