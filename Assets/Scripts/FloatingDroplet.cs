using UnityEngine;

public class FloatingDroplet : MonoBehaviour
{
    public float floatAmplitude = 0.1f; // Ampiezza del galleggiamento
    public float floatSpeed = 1f;        // Velocità del galleggiamento
    private Vector3 startPosition;        // Posizione iniziale della gocciolina

    void Start()
    {
        startPosition = transform.position; // Salva la posizione iniziale
    }

    void Update()
    {
        // Calcola il nuovo spostamento
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPosition + new Vector3(0, newY, 0);
    }
}