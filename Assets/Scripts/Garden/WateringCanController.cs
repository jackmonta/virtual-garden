using UnityEngine;

public class WateringCanController : MonoBehaviour
{
    public ParticleSystem waterParticleSystem;
    public float rotationSpeed = 20f; // Speed of rotation

    private float particleSystemDuration;
    private float elapsedTime;

    void Start()
    {
        // Get the duration of the particle system
        var main = waterParticleSystem.main;
        particleSystemDuration = main.duration;

        // Start the particle system and reset elapsed time
        waterParticleSystem.Play();
        elapsedTime = 0f;
    }

    void Update()
    {
        // Rotate the watering can while the particle system is playing
        if (waterParticleSystem.isPlaying)
        {
            elapsedTime += Time.deltaTime;
            RotateWateringCan();

            // Stop the particle system and reset elapsed time if the duration is exceeded
            if (elapsedTime > particleSystemDuration)
            {
                waterParticleSystem.Stop();
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