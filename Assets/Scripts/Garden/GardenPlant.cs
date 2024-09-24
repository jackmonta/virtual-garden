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
    public GameObject PlantObj { get; private set; }
    public GameObject VaseObj { get; private set; }
    
    IEnumerator Start()
    {
        PlantObj = this.gameObject;
        VaseObj = this.gameObject.transform.Find("Vase").gameObject;
    
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
            DecreaseHealth(2f);
        }
    }

    private void DecreaseHealth(float amount)
    {
        Plant.DecreaseHealth(amount);
        if (selectedPlant == this)
            HealthBar.Instance.UpdateHealthBar(Plant.CurrentHealth.Value);
    }

    void Update()
    {
        if (DetectTouch())
            SetSelectedPlant(this);
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
        if (Plant == null) return;

        Plant.IncreaseHealth(1f);
        if (selectedPlant == this)
            HealthBar.Instance.UpdateHealthBar(Plant.CurrentHealth.Value);
    }
}
