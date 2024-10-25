using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    private static Canvas tutorialCanvas;
    private static TextMeshProUGUI tutorialText;
    public static TutorialUI Instance { get; private set; }

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
        tutorialCanvas = GetComponent<Canvas>();
        tutorialText = GetComponentInChildren<TextMeshProUGUI>();
        tutorialCanvas.enabled = false;
    }

    public static void ShowUI()
    {
        tutorialCanvas.enabled = true;
    }
}
