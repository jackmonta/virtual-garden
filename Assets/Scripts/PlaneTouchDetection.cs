using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneTouchDetection : MonoBehaviour
{
    [SerializeField] private GameObject XROrigin;
    [SerializeField] private Canvas scanSurfaceCanvas;
    private ARPlaneManager planeManager;

    private void Start()
    {
        if (XROrigin == null)
        {
            Debug.LogError("XR Origin is not assigned");
            return;
        }

        planeManager = XROrigin.GetComponent<ARPlaneManager>();

        if (scanSurfaceCanvas != null)
            scanSurfaceCanvas.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (planeManager.enabled == true && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                ARPlane clickedPlane = DetectPlaneTouch(touch);
                if (clickedPlane != null)
                {
                    planeManager.enabled = false; // stopping plane search

                    if (scanSurfaceCanvas != null)
                        scanSurfaceCanvas.gameObject.SetActive(false);

                    // handling planes
                    foreach (ARPlane plane in planeManager.trackables)
                    {
                        if (plane == clickedPlane)
                            Garden.Instance.SetGardenPlane(clickedPlane);
                        else
                        {
                            plane.gameObject.SetActive(false);
                            Destroy(plane.gameObject);
                        }
                    }

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
            if (hitPlane != null)
                return hitPlane;

            return null;
        }

        return null;
    }
}