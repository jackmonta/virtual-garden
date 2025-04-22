using UnityEngine;

public class PlantListLoader : MonoBehaviour
{
    [SerializeField] private GameObject PlantItem;
    void Start()
    {
        Plant[] plantAssets = Resources.LoadAll<Plant>("Plants");
        for (int i=0; i < plantAssets.Length; i++)
        {
            GameObject obj = Instantiate(PlantItem, this.transform);
            obj.GetComponent<PlantItem>().SetData(plantAssets[i], i + 1);
        }
    }
}
