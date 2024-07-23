using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GardenPlant : MonoBehaviour
{
    public static GardenPlant selectedPlant;
    private static Material highlightMaterial;
    public Plant Plant { get; set; }
    public GameObject PlantObj { get; private set;}
    public GameObject VaseObj { get; private set;}
    
    void Start()
    {
        PlantObj = this.gameObject;
        VaseObj = this.gameObject.transform.GetChild(0).gameObject;

        // TODO: check collision with other plants

        Debug.Log("Created new GardenPlant");

        if (highlightMaterial == null)
            highlightMaterial = Resources.Load<Material>("Shaders/Outline Material");
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
            return;
        }

        selectedPlant = plant;
        HighlightSelectedPlant(true);
    } 
    
    private static void HighlightSelectedPlant(bool highlight)
    {
        GameObject plantObj = selectedPlant.PlantObj;
        GameObject vaseObj = selectedPlant.VaseObj;
        List<GameObject> objects = new List<GameObject> {plantObj, vaseObj};

        if (highlight)
        {
            foreach (GameObject obj in objects)
            {
                Material[] materials = obj.GetComponent<MeshRenderer>().materials;
                
                List<Material> materialList = new List<Material>(materials)
                {
                    highlightMaterial
                };
                obj.GetComponent<MeshRenderer>().materials = materialList.ToArray();
                Debug.Log("Highlighted " + obj.name);
            }
        }
        else
        {
            foreach (GameObject obj in objects)
            {
                Material[] materials = obj.GetComponent<MeshRenderer>().materials;
                obj.GetComponent<MeshRenderer>().materials = materials.Take(materials.Length - 1).ToArray();
            }
            Debug.Log("Plant unhighlighted");
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

}
