using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class TutorialUI : MonoBehaviour
{
    private static Canvas tutorialCanvas;
    public static Button invisibleButton;
    private static TextMeshProUGUI tutorialText;
    private static AudioSource audioSource;
    public static TutorialUI Instance { get; private set; }
    public static int firstLaunch;
    
    public static GardenPlant selectedPlant;



    private static List<TutorialStep> tutorialSteps = new List<TutorialStep>()
    {
        new TutorialStep("Hi, welcome to Virtual Garden!", "Audio/Welcome"),
        new TutorialStep("Now, select a plant from the inventory below.", "Audio/SelectPlant", TutorialAction.SelectPlant),
        new TutorialStep("Then, tap on the garden to place the plant.", "Audio/PlacePlant", TutorialAction.PlacePlant),
        new TutorialStep("Tap on the plant to select it.", "Audio/HighlightPlant", TutorialAction.HighlightPlant),
        new TutorialStep("Here, you can perform various actions on the plant.", "Audio/ActionsPlant"),
        new TutorialStep("Click on the watering can button to water the plant.", "Audio/WateringCan", TutorialAction.WaterPlant),
        new TutorialStep("Oh no! Insects are attacking the plant. Click on the insecticide to get rid of them.", "Audio/Insects", TutorialAction.KillInsects),
        new TutorialStep("Insects double the rate of health loss. If a plant runs out of water, it will die and turn dark.", "Audio/PlantOutOfWater"),
        new TutorialStep("To revive it, click on the revitalizing button.", "Audio/Revitalizing", TutorialAction.RevivePlant),
        new TutorialStep("By performing these actions, you can keep your garden healthy and earn coins.", "Audio/KeepHealthy"),
        new TutorialStep("Tap on the plant to collect the coins.", "Audio/CollectCoins", TutorialAction.CollectCoins),
        new TutorialStep("You can then use these coins to buy more plants and tools from the shop.", "Audio/Shop"),
        new TutorialStep("Enjoy your garden!", "Audio/Enjoy")
    };

    private static IEnumerator<TutorialStep> iterator;

    public static UnityEvent onPlantSelected = new UnityEvent();
    public static UnityEvent onPlantPlaced = new UnityEvent();
    public static UnityEvent onPlantHightlighted = new UnityEvent();
    public static UnityEvent onPlantWatered = new UnityEvent();
    public static UnityEvent onInsectsKilled = new UnityEvent();
    public static UnityEvent onPlantRevived = new UnityEvent();
    public static UnityEvent onCoinsCollected = new UnityEvent();

    public static UnityEvent<TutorialAction> onNextAction = new UnityEvent<TutorialAction>();

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
        
        invisibleButton = GameObject.Find("UI/Idle/InvisibleButton").GetComponent<Button>();
        invisibleButton.onClick.AddListener(() => Debug.Log("Button clicked"));
        invisibleButton.gameObject.SetActive(false);

        tutorialCanvas = GetComponent<Canvas>();
        tutorialText = GetComponentInChildren<TextMeshProUGUI>();
        tutorialCanvas.enabled = false;

        audioSource = gameObject.AddComponent<AudioSource>();
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
        iterator = tutorialSteps.GetEnumerator();
        SetNextStep();

        invisibleButton.gameObject.SetActive(true);
        invisibleButton.onClick.AddListener(() => SetNextStep());
    }

    private void WelcomeBackGreetings()
    {
        tutorialText.text = "Welcome back to Virtual Garden!";
        PlayAudio("Audio/WelcomeBack");
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
            onNextAction.Invoke(step.ActionRequired);

            tutorialText.text = step.Sentence;
            PlayAudio(step.AudioClipPath);

            if (step.Sentence.Contains("If a plant runs out of water"))
            {
                selectedPlant.Plant.CurrentHealth = 0f;
            } else if (step.Sentence.Contains("By doing these actions") || step.Sentence.Contains("You can then use them"))
            {
                GardenPlant.SetSelectedPlant(null);
            }

            if (step.ActionRequired != TutorialAction.None)
                ListenForActionCompletion(step.ActionRequired);
        }
        else
        {
            selectedPlant = null;
            firstLaunch = 1;
            PlayerPrefs.SetInt("FirstLaunch", 1);
            PlayerPrefs.Save();
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
                selectedPlant.SpawnInsects(10);
                onInsectsKilled.AddListener(EnableNextButton);
                break;
            case TutorialAction.RevivePlant:
                onPlantRevived.AddListener(EnableNextButton);
                break;
            case TutorialAction.CollectCoins:
                GardenPlant.SetSelectedPlant(null);
                selectedPlant.spawnedCoins = true;
                selectedPlant.SpawnCoins();
                onCoinsCollected.AddListener(EnableNextButton);
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
        onCoinsCollected.RemoveListener(EnableNextButton);

        SetNextStep();
    }

    private static void PlayAudio(string clipPath)
    {
        if (!string.IsNullOrEmpty(clipPath))
        {
            AudioClip clip = Resources.Load<AudioClip>(clipPath);
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Audio clip not found at path: " + clipPath);
            }
        }
    }
}
