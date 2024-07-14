using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneTouchDetection : MonoBehaviour
{
    [SerializeField] private GameObject XROrigin;
    [SerializeField] private Material touchedMaterial;
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


}
