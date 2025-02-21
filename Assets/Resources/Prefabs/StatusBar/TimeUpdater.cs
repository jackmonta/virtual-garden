using System.Collections;
using TMPro;
using UnityEngine;

public class TimeUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    void Start()
    {
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            timeText.text = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
            yield return new WaitForSeconds(1);
        }
    }
}
