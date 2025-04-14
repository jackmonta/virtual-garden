using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantLevel : MonoBehaviour
{
    public static PlantLevel Instance { get; private set; }
    [SerializeField] Slider levelSlider;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject plantLevelIcon;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        levelText.text = "1";
        levelSlider.value = 0;
    }

    public void SetLevel(int? level, float progress)
    {
        if (level.HasValue)
            levelText.text = (level + 1).ToString();
        else
            levelText.text = "1";

        levelSlider.value = progress;

        plantLevelIcon.SetActive(progress >= 1);
    }
}
