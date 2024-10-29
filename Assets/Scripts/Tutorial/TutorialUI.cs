using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class TutorialUI : MonoBehaviour
{
    private static Canvas tutorialCanvas;
    private static Button invisibleButton;
    private static TextMeshProUGUI tutorialText;
    public static TutorialUI Instance { get; private set; }
    public static int firstLaunch;

    private static List<TutorialStep> tutorialSteps = new List<TutorialStep>()
    {
        new TutorialStep("Hi, welcome to Virtual Garden!"),
        new TutorialStep("Now select a plant from the inventory below.", TutorialAction.SelectPlant),
        new TutorialStep("Then, tap on the garden to place the plant.", TutorialAction.PlacePlant),
        new TutorialStep("Tap on the flower to select it.", TutorialAction.HighlightPlant),
        new TutorialStep("Here you can operate various actions on the flower."),
        new TutorialStep("Click on the Water button to water the plant.", TutorialAction.WaterPlant),
        new TutorialStep("If a swarm of insects attacks the flower, click on the Insect button to kill them.", TutorialAction.KillInsects),
        new TutorialStep("If a plant runs out of water, then it will die."),
        new TutorialStep("To revive it, click on the revitalizing button.", TutorialAction.RevivePlant),
        new TutorialStep("By doing these actions, you can keep your garden healthy, and you will farm coins."),
        new TutorialStep("You can then use them to buy more plants and tools."),
        new TutorialStep("Enjoy your garden!")
    };

    private static IEnumerator<TutorialStep> iterator;

    public static UnityEvent onPlantSelected = new UnityEvent();
    public static UnityEvent onPlantPlaced = new UnityEvent();
    public static UnityEvent onPlantHightlighted = new UnityEvent();
    public static UnityEvent onPlantWatered = new UnityEvent();
    public static UnityEvent onInsectsKilled = new UnityEvent();
    public static UnityEvent onPlantRevived = new UnityEvent();

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

        //if (firstLaunch == 0)
            Instance.StartTutorial();
        //else
            //Instance.WelcomeBackGreetings();
    }

    private void StartTutorial()
    {
        iterator = tutorialSteps.GetEnumerator();
        SetNextStep();

        invisibleButton.gameObject.SetActive(true);
        invisibleButton.onClick.AddListener(() => SetNextStep());
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

    private static void SetNextStep()
    {
        if (iterator.MoveNext())
        {
            TutorialStep step = iterator.Current;

            tutorialText.text = step.Sentence;

            if (step.ActionRequired != TutorialAction.None)
                ListenForActionCompletion(step.ActionRequired);
        }
        else
        {
            //PlayerPrefs.SetInt("FirstLaunch", 1);
            //PlayerPrefs.Save();
            HideUI();
        }
    }

    private static void ListenForActionCompletion(TutorialAction action)
    {
        invisibleButton.gameObject.SetActive(false);

        switch (action)
        {
            case TutorialAction.SelectPlant:
                onPlantSelected.AddListener(EnableNextButton);
                break;
            case TutorialAction.PlacePlant:
                onPlantPlaced.AddListener(EnableNextButton);
                break;
            case TutorialAction.HighlightPlant:
                onPlantHightlighted.AddListener(EnableNextButton);
                break;
            case TutorialAction.WaterPlant:
                onPlantWatered.AddListener(EnableNextButton);
                break;
            case TutorialAction.KillInsects:
                onInsectsKilled.AddListener(EnableNextButton);
                break;
            case TutorialAction.RevivePlant:
                onPlantRevived.AddListener(EnableNextButton);
                break;
        }

        Debug.Log("Waiting for action: " + action);
    }

    private static void EnableNextButton()
    {
        Debug.Log("Action completed");
        invisibleButton.gameObject.SetActive(true);

        // Unsubscribe from the event so it doesn’t trigger again
        onPlantSelected.RemoveListener(EnableNextButton);
        onPlantPlaced.RemoveListener(EnableNextButton);
        onPlantHightlighted.RemoveListener(EnableNextButton);
        onPlantWatered.RemoveListener(EnableNextButton);
        onInsectsKilled.RemoveListener(EnableNextButton);
        onPlantRevived.RemoveListener(EnableNextButton);

        SetNextStep();
    }
}
