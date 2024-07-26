using UnityEngine;

public class PlantUI : MonoBehaviour
{
    public static PlantUI Instance { get; private set; }

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
    }

    void Start()
    {
        ShowUI(false);
    }

    public static void ShowUI(bool show)
    {
        if (Instance.gameObject.activeSelf != show)
            Instance.gameObject.SetActive(show);
    }

    public static bool isActive()
    {
        return Instance.gameObject.activeSelf;
    }
}
