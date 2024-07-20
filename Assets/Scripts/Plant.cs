using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant", order = 1)]
[System.Serializable]
public class Plant : ScriptableObject
{
    [SerializeField]
    private new string name;
    public string Name { get { return name; } }
    
    [SerializeField]
    private float health;
    public float Health { get { return health; } }
    
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }
}
