using UnityEngine;

public class Plant
{
    public string Name { get; private set; }
    public GameObject Prefab { get; private set; }
    public Plant(string name, GameObject prefab)
    {
        Name = name;
        Prefab = prefab;
    }
}
