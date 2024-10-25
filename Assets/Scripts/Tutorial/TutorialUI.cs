using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private static Canvas tutorialCanvas;
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
        tutorialCanvas.enabled = false;
    }

    public static void ShowUI()
    {
        tutorialCanvas.enabled = true;
    }
}
