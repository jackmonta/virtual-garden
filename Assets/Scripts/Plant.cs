using UnityEngine;

[System.Serializable]
public class Plant
{
    [SerializeField]
    private string name;
    public string Name { get { return name; } }
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }
    public Plant(string name, GameObject prefab)
    {
        this.name = name;
        this.prefab = prefab;
    }
}
