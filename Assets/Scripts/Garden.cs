using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Garden : MonoBehaviour
{
    [SerializeField] private Material gardenMaterial;
    [SerializeField] private GameObject vasePrefab;
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

        // applying material
        GameObject planeObj = this.plane.gameObject;
        MeshRenderer renderer = planeObj.GetComponent<MeshRenderer>();
        renderer.material = gardenMaterial;

        PlaceVases();
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
