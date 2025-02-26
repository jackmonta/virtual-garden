using UnityEngine;
using UnityEngine.UI;

public class LevelupPopup : MonoBehaviour
{
    [SerializeField] Button closeButton;
    void Start()
    {
        closeButton.onClick.AddListener(() => Destroy(this.gameObject));
    }
}
