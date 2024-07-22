using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab; // Prefab da istanziare
    public GameObject particleSystemPrefab; // Prefab del sistema di particelle

    public Transform spawnPosition; // Posizione di spawn per il prefab

    void Start()
    {
        SpawnPrefab();
    }

    void SpawnPrefab()
    {
        // Instanzia il prefab
        GameObject newObject = Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);

        // Avvia l'animazione di rotazione
        Animator animator = newObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Rotate"); // Assicurati di avere un trigger "Rotate" nel tuo Animator Controller
        }
        else
        {
            Debug.LogWarning("Animator not found on the prefab.");
        }

        // Instanzia e posiziona il sistema di particelle
        if (particleSystemPrefab != null)
        {
            GameObject particleSystemObject = Instantiate(particleSystemPrefab, newObject.transform.position + Vector3.up, Quaternion.identity);
            // Opzionale: distruggi il sistema di particelle dopo un certo tempo
            Destroy(particleSystemObject, 5f); // Distrugge dopo 5 secondi
        }
        else
        {
            Debug.LogWarning("Particle System prefab not assigned.");
        }
    }
}