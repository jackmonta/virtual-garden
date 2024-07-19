using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class Garden : MonoBehaviour
{
    [SerializeField] private Material gardenMaterial;
    [SerializeField] private GameObject vasePrefab;
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

        if (Inventory.GetSelectedPlant() == null) return;

        var touchPoint = DetectGardenTouch();
        if (touchPoint.HasValue)
        {
            Vector3 touchPosition = DetectGardenTouch().Value;
            Plant plantToSpawn = Inventory.GetSelectedPlant();
            GameObject plantObj = Instantiate(vasePrefab, touchPosition, Quaternion.Euler(-90, 0, 0));
            
            if (CollideWithOtherPlants(plantObj))
            {
                Destroy(plantObj);
                Debug.Log("Collision detected");
                return;
            }

            plants.Add(plantToSpawn, plantObj);
            Inventory.RemoveSelectedPlant();
        }
    }

    private bool CollideWithOtherPlants(GameObject plantObj)
    {
        if (plantObj.GetComponent<Collider>() == null)
            throw new Exception("No collider for this gameObject");
        
        Collider collider = plantObj.GetComponent<Collider>();

        foreach (GameObject otherPlantObj in plants.Values)
        {
            Collider otherCollider = otherPlantObj.GetComponent<Collider>();

            if (collider.bounds.Intersects(otherCollider.bounds))
                return true;
        }

        return false;
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

        // applying material
        GameObject planeObj = this.plane.gameObject;
        MeshRenderer renderer = planeObj.GetComponent<MeshRenderer>();
        renderer.material = gardenMaterial;

        //PlaceVases();

        InventoryUI.ShowUI();
    }

    private void PlaceVases()
    {
        Vector3 planeCenter = plane.center;
        //Vector3 planeNormal = plane.normal;
        float planeWidth = plane.size.x;
        float planeHeight = plane.size.y;

        int numVases = 10; // Number of vases to place
        float spacing = Mathf.Min(planeWidth, planeHeight) / Mathf.Sqrt(numVases);

        for (int i = 0; i < Mathf.Sqrt(numVases); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(numVases); j++)
            {
                Vector3 vasePosition = planeCenter + new Vector3((i - Mathf.Sqrt(numVases)/2) * spacing, 0, (j - Mathf.Sqrt(numVases)/2) * spacing);
                GameObject vase = Instantiate(vasePrefab, vasePosition, Quaternion.Euler(-90, 0, 0));
                
                // Check if the vase fits on the plane
                if (vase.transform.position.x < planeCenter.x - planeWidth / 2 || vase.transform.position.x > planeCenter.x + planeWidth / 2 ||
                    vase.transform.position.z < planeCenter.z - planeHeight / 2 || vase.transform.position.z > planeCenter.z + planeHeight / 2)
                {
                    // Adjust the scale to fit the vases on the plane
                    vase.transform.localScale = vase.transform.localScale * 0.8f;
                }
            }
        }
    }
}
