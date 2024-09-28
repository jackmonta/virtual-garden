using UnityEngine;

public class FlyAround : MonoBehaviour
{
    public Transform target; // La pianta o l'oggetto attorno a cui volare
    public float speed = 1f; // Velocità di volo
    public float radius = 1f; // Raggio del volo
    public float heightVariation = 0.1f; // Variazione di altezza

    private Vector3 offset;
    private float angle;

    private void Start()
    {
        // Posiziona l'insetto a una distanza casuale dal target
        offset = new Vector3(Random.Range(-radius, radius), Random.Range(heightVariation, 2*heightVariation), Random.Range(-radius, radius));
    }

    private void Update()
    {
        if (target == null) return;

        // Movimento circolare attorno alla pianta
        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        Vector3 newPosition = new Vector3(x, offset.y, z) + target.position;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
        transform.LookAt(target.position + Vector3.up * (2*heightVariation)); // Mantieni l'insetto rivolto verso la pianta
    }
}