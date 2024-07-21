using UnityEngine;

public class WateringCanController : MonoBehaviour
{
    public ParticleSystem waterParticles;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    public float inclinationSpeed = 1.0f;
    public float emissionThreshold = 20.0f; // Degrees

    void Start()
    {
        initialRotation = transform.rotation;
        waterParticles.Stop();
    }

    void Update()
    {
        // Incline the watering can using input keys
        if (Input.GetKey(KeyCode.I)) // 'I' for incline
        {
            targetRotation = Quaternion.Euler(transform.eulerAngles.x + inclinationSpeed, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D)) // 'D' for decline
        {
            targetRotation = Quaternion.Euler(transform.eulerAngles.x - inclinationSpeed, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }

        // Check the inclination angle to control particle emission
        float angle = Quaternion.Angle(initialRotation, transform.rotation);
        if (angle > emissionThreshold)
        {
            if (!waterParticles.isEmitting)
                waterParticles.Play();
        }
        else
        {
            if (waterParticles.isEmitting)
                waterParticles.Stop();
        }
    }
}
