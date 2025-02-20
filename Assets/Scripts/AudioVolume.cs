using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image volumeIcon;

    const string MIXER_VOLUME = "Volume";
    private Sprite volumeOn;
    private Sprite volumeOff;

    void Awake()
    {
        volumeOn = Resources.Load<Sprite>("Audio/SoundOn");
        volumeOff = Resources.Load<Sprite>("Audio/SoundOff");
        float savedVolume = PlayerPrefs.GetFloat(MIXER_VOLUME, 1f);
        SetVolume(savedVolume);
    }
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(MIXER_VOLUME, 1f);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (volume <= 0.0001f)
            volumeIcon.sprite = volumeOff;
        else
            volumeIcon.sprite = volumeOn;

        float dB = Mathf.Log10(volume) * 20;
        dB = Mathf.Clamp(dB, -80f, 0f);

        audioMixer.SetFloat(MIXER_VOLUME, dB);
        PlayerPrefs.SetFloat(MIXER_VOLUME, volume);
    }
}
