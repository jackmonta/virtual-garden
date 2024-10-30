using UnityEngine;

public class WateringCanController : MonoBehaviour
{
    public ParticleSystem waterParticleSystem;
    public static bool isAvailable = true;
    public float rotationSpeed = 40f; // rotation speed
    
    public AudioSource audioSource;

    private float particleSystemDuration;
    private float elapsedTime;

    void Start()
    {
        isAvailable = false;
        var main = waterParticleSystem.main;
        particleSystemDuration = main.duration;

        elapsedTime = 0f;
        waterParticleSystem.Play();
        audioSource.Play();
    }

    void Update()
    {
        // Rotate the watering can while the particle system is playing
        if (waterParticleSystem.isPlaying)
        {
            elapsedTime += Time.deltaTime;
            RotateWateringCan();

            // animation ended
            if (elapsedTime > particleSystemDuration)
            {
                waterParticleSystem.Stop();
                isAvailable = true;
                audioSource.Stop();
                Destroy(gameObject);
                TutorialUI.onPlantWatered.Invoke();
            }
        }
    }

    void RotateWateringCan()
    {
        float downDuration = particleSystemDuration * 0.2f;
        float upDuration = particleSystemDuration - downDuration;

        // Determine rotation direction based on elapsed time
        if (elapsedTime < downDuration)
        {
            // Rotate right
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
        else if (elapsedTime > upDuration && elapsedTime < particleSystemDuration)
        {
            // Rotate left
            transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
        }
    }
}