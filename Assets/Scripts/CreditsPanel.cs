using UnityEngine;
using UnityEngine.UI;

public class CreditsPanel : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    void Start()
    {
        Button closeButton = this.gameObject.GetComponentInChildren<Button>();
        closeButton.onClick.AddListener(() => Hide());
    }

    void Hide()
    {
        this.gameObject.SetActive(false);
        menuPanel.SetActive(true);
    }
}
