using UnityEngine;

[System.Serializable]
public class PlantUpgrade
{
    [SerializeField]
    private int price;
    public int Price { get { return price; } }

    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }
}
