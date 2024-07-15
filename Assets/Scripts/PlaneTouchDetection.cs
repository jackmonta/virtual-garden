using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneTouchDetection : MonoBehaviour
{
    [SerializeField] private GameObject XROrigin;
    [SerializeField] private Material touchedMaterial;
    [SerializeField] private GameObject vasePrefab; // Add a public field for the vase prefab
    private ARPlaneManager planeManager;
    private Dictionary<int, ARPlane> planes;

    void Start()
    {
        if (XROrigin == null)
        {
            Debug.LogError("XR Origin is not assigned");
            return;
        }

        planes = new Dictionary<int, ARPlane>();

        // adding listener to the event
        planeManager = XROrigin.GetComponent<ARPlaneManager>();
        planeManager.planesChanged += PlaneChanged;
    }

    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        // managing all existing planes
        args.added.ForEach(plane =>
        {
            if (!planes.ContainsKey(plane.GetInstanceID()))
                planes.Add(plane.GetInstanceID(), plane);
        });
        args.updated.ForEach(plane =>
        {
            if (planes.ContainsKey(plane.GetInstanceID()))
                planes[plane.GetInstanceID()] = plane;
        });
        args.removed.ForEach(plane =>
        {
            if (planes.ContainsKey(plane.GetInstanceID()))
                planes.Remove(plane.GetInstanceID());
        });
    }

    void Update()
    {
        if (planeManager.enabled == true && Input.touchCount > 0)
        {
            Debug.Log("Touch detected");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                ARPlane clickedPlane = DetectPlaneTouch(touch);
                if (clickedPlane != null)
                {
                    // stopping plane search
                    planeManager.planesChanged -= PlaneChanged;
                    planeManager.enabled = false;

                    HideOtherPlanes(clickedPlane);

                    ChangeColor(clickedPlane);

                    PlaceVasesOnPlane(clickedPlane); // Place vases on the detected plane

                    this.enabled = false; // stopping the script
                }
            }
        }
    }

    private ARPlane DetectPlaneTouch(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ARPlane hitPlane = hit.transform.GetComponent<ARPlane>();
            if (hitPlane != null && planes.ContainsKey(hitPlane.GetInstanceID()))
                return hitPlane;

            return null;
        }

        return null;
    }

    private void ChangeColor(ARPlane plane)
    {
        GameObject planeObj = plane.gameObject;
        MeshRenderer renderer = planeObj.GetComponent<MeshRenderer>();
        renderer.material = touchedMaterial;
    }

    private void HideOtherPlanes(ARPlane visiblePlane)
    {
        foreach (ARPlane plane in planes.Values)
            if (plane.GetInstanceID() != visiblePlane.GetInstanceID())
            {
                planes.Remove(plane.GetInstanceID());
                plane.gameObject.SetActive(false);
                Destroy(plane.gameObject);
            }
    }

    private void PlaceVasesOnPlane(ARPlane plane)
    {
        Vector3 planeCenter = plane.center;
        Vector3 planeNormal = plane.normal;
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