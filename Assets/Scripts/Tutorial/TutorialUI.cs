using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialUI : MonoBehaviour
{
    private static Canvas tutorialCanvas;
    private static Button invisibleButton;
    private static TextMeshProUGUI tutorialText;
    public static TutorialUI Instance { get; private set; }
    public static int firstLaunch;

    private static List<String> tutorialSentences = new List<String>()
    {
        "Hi, welcome to Virtual Garden!",
        "Now select a plant from the inventory below.",
        "Now, tap on the garden to place the plant.",
        "You can select a flower from the inventory below.",
        "Tap on the flower to select it.",
        "Here you can operate various actions on the flower.",
        "Click on the Water button to water the flower.",
        "If a swarm of insects attacks the flower, click on the Insect button to kill them.",
        "If a plant run out of water, then it will die.",
        "To revive it, click on the Revive button.",
        "By doing these actions, you can keep your garden healthy, and you will farm coins.",
        "You can then use them to buy more plants and tools.",
        "Enjoy your garden!"
    };

    private static IEnumerator<String> iterator;
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
        firstLaunch = PlayerPrefs.GetInt("FirstLaunch");

        invisibleButton = GetComponentInChildren<Button>();
        invisibleButton.gameObject.SetActive(false);

        tutorialCanvas = GetComponent<Canvas>();
        tutorialText = GetComponentInChildren<TextMeshProUGUI>();
        tutorialCanvas.enabled = false;
    }

    public static void ShowUI()
    {
        tutorialCanvas.enabled = true;

        if (firstLaunch == 0)
            Instance.StartTutorial();
        else
            Instance.WelcomeBackGreetings();
    }

    private void StartTutorial()
    {
        iterator = tutorialSentences.GetEnumerator();
        SetNextSentence();

        invisibleButton.gameObject.SetActive(true);
        invisibleButton.onClick.AddListener(() => SetNextSentence());
    }

    private void WelcomeBackGreetings()
    {
        tutorialText.text = "Welcome back to Virtual Garden!";
        invisibleButton.gameObject.SetActive(true);
        invisibleButton.onClick.AddListener(() => HideUI());
    }

    private static void HideUI()
    {
        tutorialCanvas.enabled = false;
        invisibleButton.gameObject.SetActive(false);
    }

    private static void SetNextSentence()
    {
        if (iterator.MoveNext())
        {
            tutorialText.text = iterator.Current;
        }
        else
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);
            PlayerPrefs.Save();
            HideUI();
        }
    }
}
