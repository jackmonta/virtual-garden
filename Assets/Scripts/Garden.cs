using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Garden : MonoBehaviour
{
    [SerializeField] private Material gardenMaterial;
    [SerializeField] private GameObject vasePrefab;
    [SerializeField] private GameObject plantPrefab; // Add plant prefab reference
    public static Garden Instance { get; set; }
    private ARPlane plane;

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
    }

    public void SetGardenPlane(ARPlane plane)
    {
        this.plane = plane;

        // Applying material
        GameObject planeObj = this.plane.gameObject;
        MeshRenderer renderer = planeObj.GetComponent<MeshRenderer>();
        renderer.material = gardenMaterial;

        PlaceVases();
    }

    private void PlaceVases()
    {
        Vector3 planeCenter = plane.center;
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
                GameObject vase = Instantiate(vasePrefab, vasePosition, Quaternion.identity);

                // Scale the vase based on the plane size
                vase.transform.localScale = Vector3.one * 0.2f * planeScaleFactor;

                // Ensure the vase fits within the plane boundaries
                vase.transform.position = new Vector3(
                    Mathf.Clamp(vase.transform.position.x, planeCenter.x - planeWidth / 2 + spacingX / 2, planeCenter.x + planeWidth / 2 - spacingX / 2),
                    vase.transform.position.y,
                    Mathf.Clamp(vase.transform.position.z, planeCenter.z - planeHeight / 2 + spacingZ / 2, planeCenter.z + planeHeight / 2 - spacingZ / 2)
                );

                // Place plant prefab inside the vase
                PlacePlantInVase(vase, vase.transform.position);
            }
        }
    }

    private void PlacePlantInVase(GameObject vase, Vector3 vasePosition)
    {
        // Get the renderer of the vase to calculate its actual height
        Renderer vaseRenderer = vase.GetComponentInChildren<Renderer>();
        if(vaseRenderer != null)
        {
            float vaseHeight = vaseRenderer.bounds.size.y;

            // Calculate the top position of the vase using its actual height
            Vector3 vaseTopPosition = vasePosition + new Vector3(0, vaseHeight / 2, 0);

            // Instantiate the plant and set its parent to the vase
            GameObject plant = Instantiate(plantPrefab, vaseTopPosition, Quaternion.identity);
            plant.transform.SetParent(vase.transform);

            // Adjust the plant's scale (if needed)
            plant.transform.localScale = Vector3.one * 0.2f; // Adjust scale as needed

            // Ensure the bottom of the plant is aligned with the top of the vase
            Renderer plantRenderer = plant.GetComponentInChildren<Renderer>();
            if (plantRenderer != null)
            {
                float plantHeight = plantRenderer.bounds.size.y;
                plant.transform.localPosition = new Vector3(0, vaseHeight / 2 + plantHeight / 2, 0);
            }
        }
    }
}