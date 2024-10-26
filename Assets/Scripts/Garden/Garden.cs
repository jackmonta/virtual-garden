using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class Garden : MonoBehaviour
{
    [SerializeField] private Material gardenMaterial;
    [SerializeField] private GameObject vasePrefab;
    [SerializeField] private GameObject insectPrefab;
    [SerializeField] private GameObject dropPrefab;
    public AudioSource audioSource;
    
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
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !IsPointerOverUI(Input.GetTouch(0).position))
            GardenPlant.SetSelectedPlant(null);
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

        GameObject plantObj = Instantiate(plantToSpawn.Prefab, vaseTopPosition, Quaternion.identity);
        plantObj.name = "Plant";

        plantObj.transform.localScale = Vector3.one * 0.07f;
        vaseObj.transform.SetParent(plantObj.transform);
        plantObj.AddComponent<GardenPlant>();
        plantObj.GetComponent<GardenPlant>().Plant = plantToSpawn;
        plantObj.GetComponent<GardenPlant>().insectPrefab = insectPrefab;
        plantObj.GetComponent<GardenPlant>().dropPrefab = dropPrefab;
        
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

}