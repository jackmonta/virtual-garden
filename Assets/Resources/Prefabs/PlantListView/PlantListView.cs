using UnityEngine;
using UnityEngine.UI;

public class PlantListView : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button xButton;
    void Start()
    {
        this.gameObject.SetActive(false);

        closeButton.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
        xButton.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
    }
}
