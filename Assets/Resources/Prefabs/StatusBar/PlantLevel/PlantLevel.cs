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
        levelText.text = "0";
        levelSlider.value = 0;
    }

    public void SetLevel(int level, float progress)
    {
        levelText.text = (level + 1).ToString();
        levelSlider.value = progress;

        if (progress >= 1)
        {
            plantLevelIcon.SetActive(true);
        }
        else
        {
            plantLevelIcon.SetActive(false);
        }
    }
}
