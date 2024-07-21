using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class Garden : MonoBehaviour
{
    [SerializeField] private Material gardenMaterial;
    [SerializeField] private GameObject vasePrefab;
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private GameObject wateringCanPrefab;
    
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

        // plant touch
        Plant plant = DetectPlantTouch();
        if (plant != null)
        {
            GardenSelectedPlant.SetSelectedPlant(plant, plants[plant]);
            return;
        }

        // garden touch
        var touchPoint = DetectGardenTouch();
        if (touchPoint.HasValue && Inventory.GetSelectedPlant() != null)
        {
            Vector3 touchPosition = DetectGardenTouch().Value;
            Plant plantToSpawn = Inventory.GetSelectedPlant();
            SpawnPlant(plantToSpawn, touchPosition);
            return;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            GardenSelectedPlant.SetSelectedPlant(null, null);
    }

    private Plant DetectPlantTouch()
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
                    
                    if (!plants.ContainsValue(hitObject))
                        return null;
                    
                    foreach (Plant plant in plants.Keys)
                        if (plants[plant] == hitObject)
                            return plant;
                }
            }
            return null;
        }
        return null;
    }

    private void SpawnPlant(Plant plantToSpawn, Vector3 touchPosition)
    {
        GameObject vaseObj = Instantiate(vasePrefab, touchPosition, Quaternion.identity);

        if (CollideWithOtherPlants(vaseObj))
        {
            Destroy(vaseObj);
            Debug.Log("Collision detected");
            return;
        }
        
        Inventory.RemoveSelectedPlant();
        GameObject plantObj = PlacePlantInVase(plantToSpawn.Prefab, vaseObj, touchPosition);
        plants.Add(plantToSpawn, plantObj);
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

        // Applying material
        GameObject planeObj = this.plane.gameObject;
        MeshRenderer renderer = planeObj.GetComponent<MeshRenderer>();
        renderer.material = gardenMaterial;

        InventoryUI.ShowUI();
        PlaceWateringCan(); 
        PlaceVases();
    }
    
    private void PlaceWateringCan()
    {
        Vector3 planeCenter = plane.center;
        float planeWidth = plane.size.x;
        float planeHeight = plane.size.y;

        // Position the watering can near the center of the plane
        Vector3 wateringCanPosition = planeCenter + new Vector3(planeWidth / 4, 0, planeHeight / 4);
        GameObject wateringCan = Instantiate(wateringCanPrefab, wateringCanPosition, Quaternion.identity);

        // Adjust the scale of the watering can if necessary
        wateringCan.transform.localScale = Vector3.one * 0.5f; // Adjust scale as needed
    }

    private void PlaceVases()
    {
        Vector3 planeCenter = plane.center;
        //Vector3 planeNormal = plane.normal;
        float planeWidth = plane.size.x;
        float planeHeight = plane.size.y;

        int numVases = 10; // Number of vases to place
        int vasesPerRow = Mathf.CeilToInt(Mathf.Sqrt(numVases));
        float spacingX = planeWidth / vasesPerRow;
        float spacingZ = planeHeight / vasesPerRow;

        // Calculate the scale factor based on the plane size
        float planeScaleFactor = Mathf.Max(planeWidth, planeHeight);

        for (int i = 0; i < vasesPerRow; i++)
        {
            for (int j = 0; j < vasesPerRow; j++)
            {
                int vaseIndex = i * vasesPerRow + j;
                if (vaseIndex >= numVases) break;

                Vector3 vasePosition = planeCenter + new Vector3((i - vasesPerRow / 2) * spacingX, 0, (j - vasesPerRow / 2) * spacingZ);
                GameObject vaseObj = Instantiate(vasePrefab, vasePosition, Quaternion.identity);

                // Scale the vase based on the plane size
                vaseObj.transform.localScale = Vector3.one * 0.2f * planeScaleFactor;

                // Ensure the vase fits within the plane boundaries
                vaseObj.transform.position = new Vector3(
                    Mathf.Clamp(vaseObj.transform.position.x, planeCenter.x - planeWidth / 2 + spacingX / 2, planeCenter.x + planeWidth / 2 - spacingX / 2),
                    vaseObj.transform.position.y,
                    Mathf.Clamp(vaseObj.transform.position.z, planeCenter.z - planeHeight / 2 + spacingZ / 2, planeCenter.z + planeHeight / 2 - spacingZ / 2)
                );

                // Place plant prefab inside the vase
                PlacePlantInVase(plantPrefab, vaseObj, vaseObj.transform.position);
            }
        }
    }

    private GameObject PlacePlantInVase(GameObject plantPrefab, GameObject vasePrefab, Vector3 vasePosition)
    {
        // Get the renderer of the vase to calculate its actual height
        Renderer vaseRenderer = vasePrefab.GetComponentInChildren<Renderer>();
        if(vaseRenderer != null)
        {
            float vaseHeight = vaseRenderer.bounds.size.y;

            // Calculate the top position of the vase using its actual height
            Vector3 vaseTopPosition = vasePosition + new Vector3(0, vaseHeight / 2, 0);

            // Instantiate the plant and set its parent to the vase
            GameObject plantObj = Instantiate(plantPrefab, vaseTopPosition, Quaternion.identity);
            plantObj.transform.SetParent(vasePrefab.transform);

            // Adjust the plant's scale (if needed)
            plantObj.transform.localScale = Vector3.one * 0.2f; // Adjust scale as needed

            // Ensure the bottom of the plant is aligned with the top of the vase
            Renderer plantRenderer = plantObj.GetComponentInChildren<Renderer>();
            if (plantRenderer != null)
            {
                float plantHeight = plantRenderer.bounds.size.y;
                plantObj.transform.localPosition = new Vector3(0, vaseHeight / 2 + plantHeight / 2, 0);
            }

            return plantObj;
        }

        return null;
    }
}