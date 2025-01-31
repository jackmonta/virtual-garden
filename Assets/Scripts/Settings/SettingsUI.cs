using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; }
    [SerializeField] GameObject openSettingButton;
    private bool isSettingOpen = false;

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
        openSettingButton.GetComponent<Button>().onClick.AddListener(() => {
            if (!isSettingOpen)
                SettingsPanel.Instance.Show();
            else
                SettingsPanel.Instance.Hide();
            isSettingOpen = ! isSettingOpen;

            Debug.Log("Open settings button clicked");
        });

        openSettingButton.SetActive(false);
    }

    public void ShowUI()
    {
        openSettingButton.SetActive(true);
    }
}
