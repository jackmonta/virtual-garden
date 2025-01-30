using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public static SettingsPanel Instance { get; private set; }
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        resumeButton.onClick.AddListener(() => {
           Debug.Log("resuming game..."); 
        });
        quitButton.onClick.AddListener(() => {
           Debug.Log("quitting game..."); 
           CloseGame();
        });
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}
