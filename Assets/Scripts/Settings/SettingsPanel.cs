using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public static SettingsPanel Instance { get; private set; }
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button plantButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private GameObject plantView;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        Debug.Log("Game paused");
        Pause();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Resume();
        SettingsUI.Instance.isSettingOpen = false;
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        plantView.gameObject.SetActive(false);
        
        resumeButton.onClick.AddListener(() => {
           Debug.Log("resuming game..."); 
           Hide();
        });
        menuButton.onClick.AddListener(() => {
           Debug.Log("Going to menu..."); 
           SwitchToMenu();
        });
        quitButton.onClick.AddListener(() => {
           Debug.Log("quitting game..."); 
           CloseGame();
        });
        plantButton.onClick.AddListener(() => {
            Debug.Log("opening plant list...");
            OpenPlantList();
        });
        achievementsButton.onClick.AddListener(() => {
            Debug.Log("opening achievements...");
            AchievementsView.Instance.Show();
        });
    }

    private void SwitchToMenu()
    {
        GameSaver.SaveAll();
        Garden.Instance.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Resume();
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Resume()
    {
        Time.timeScale = 1;
    }

    private void CloseGame()
    {
        Application.Quit();
    }

    private void OpenPlantList()
    {
        plantView.gameObject.SetActive(true);
    }
}
