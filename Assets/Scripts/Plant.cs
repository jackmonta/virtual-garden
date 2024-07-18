using UnityEngine;

[System.Serializable]
public class Plant
{
    [SerializeField]
    private int id;
    public int ID { get { return id; } }

    [SerializeField]
    private string name;
    public string Name { get { return name; } }

    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    public Plant(int id, string name, GameObject prefab)
    {
        this.id = id;
        this.name = name;
        this.prefab = prefab;
    }
}
