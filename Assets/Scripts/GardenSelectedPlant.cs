using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GardenSelectedPlant : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    public static GardenSelectedPlant Instance { get; set; }
    private static Plant selectedPlant;
    private static GameObject selectedPlantObj;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        selectedPlant = null;
    }

    public static void SetSelectedPlant(Plant plant, GameObject plantObj)
    {
        if (plant == selectedPlant) return;

        // removing highlight material from previous plant
        if (selectedPlantObj != null)
        {
            Material[] materials = selectedPlantObj.GetComponent<MeshRenderer>().materials;
            selectedPlantObj.GetComponent<MeshRenderer>().materials = materials.Take(materials.Length - 1).ToArray();
        }

        if (plant == null)
        {
            selectedPlant = null;
            selectedPlantObj = null;
            return;
        }

        selectedPlant = plant;
        selectedPlantObj = plantObj;
        selectedPlantObj.GetComponent<MeshRenderer>().AddMaterial(Instance.highlightMaterial);
        Debug.Log("Applied highlight material to " + selectedPlant.Name);
    }
}
