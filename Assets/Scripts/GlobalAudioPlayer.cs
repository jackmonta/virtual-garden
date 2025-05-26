using UnityEngine;

public class GlobalAudioPlayer : MonoBehaviour
{
    public static GlobalAudioPlayer Instance { get; private set; }

    [SerializeField] private AudioClip audioClip;

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Setup AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = false; // Cambia a true se vuoi che sia continuo
    }

    public void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.Play();
        }
    }

    public void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}