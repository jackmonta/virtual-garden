using UnityEngine;

public class InsecticideController : MonoBehaviour
{
    public ParticleSystem insecticideParticleSystem;
    public static bool isAvailable = true;
    public float rotationSpeed = 50f; // rotation speed
    
    public int particleSystemActivations = 3;
    private float singleActivationDuration;
    private float particleSystemDuration;
    public AudioSource audioSource;
    private float elapsedTime;
    private GardenPlant insecticideSelectedPlant;

    void Start()
    {
        isAvailable = false;
        var main = insecticideParticleSystem.main;
        
        singleActivationDuration = main.duration;
        particleSystemDuration = singleActivationDuration * particleSystemActivations;

        elapsedTime = 0f;
        insecticideSelectedPlant = GardenPlant.selectedPlant;
        insecticideParticleSystem.Play();
        audioSource.Play();
    }

    void Update()
    {
        // Rotate the watering can while the particle system is playing
        if (insecticideParticleSystem.isPlaying)
        {
            elapsedTime += Time.deltaTime;
            if(particleSystemActivations == 3)
                RotateInsecticide();
            
            if(elapsedTime >= singleActivationDuration && particleSystemActivations > 0)
            {
                insecticideParticleSystem.Stop();
                insecticideParticleSystem.Play();
                audioSource.Stop(); 
                audioSource.Play();
                elapsedTime = 0f;
                particleSystemActivations--;
            } else if (particleSystemActivations <= 0 && elapsedTime >= singleActivationDuration) // animation ended
            {
                insecticideParticleSystem.Stop();
                audioSource.Stop(); 
                isAvailable = true;
                particleSystemActivations = 3;
                insecticideSelectedPlant?.RemoveInsects(); 
                Destroy(gameObject);
                TutorialUI.onInsectsKilled.Invoke();
            }
        }
    }

    void RotateInsecticide()
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