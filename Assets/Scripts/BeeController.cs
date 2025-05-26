using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 returnPosition;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private Quaternion returnRotation;
    
    
    public bool IsReturning { get; private set; } = false; 
    private bool canReturn = false;

    public void Setup(Vector3 start, Vector3 target)
    {
        startPosition = start;
        targetPosition = target;

        // La posizione di ritorno è vicina alla posizione iniziale
        returnPosition = start + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));

        // Calcola la rotazione iniziale verso il target
        returnRotation = Quaternion.LookRotation(targetPosition - startPosition);
        originalRotation = Quaternion.LookRotation(targetPosition - startPosition) * Quaternion.Euler(0, 205, 0);
        
        // Calcola la rotazione finale (aggiunge 45 gradi sull'asse Y)
        targetRotation = Quaternion.LookRotation(targetPosition - startPosition) * Quaternion.Euler(0, 205, 0);
        targetRotation.x = 0;
        targetRotation.z = 0;

        // Imposta la rotazione iniziale dell'ape
        transform.rotation = originalRotation;

        // Inizia la sequenza di comportamento
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        // Movimento verso il punto vicino alla telecamera
        float approachDuration = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < approachDuration)
        {
            // Interpola posizione
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / approachDuration);

            // Interpola rotazione verso targetRotation
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / approachDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Pausa davanti alla telecamera
        transform.position = targetPosition;
        transform.rotation = targetRotation; // Assicurati che abbia esattamente la rotazione target
        canReturn = true;
        yield return new WaitUntil(() => IsReturning); 

        // Movimento verso la posizione di ritorno
        elapsedTime = 0f;
        float returnDuration = 2.5f;
        while (elapsedTime < returnDuration)
        {
            // Interpola posizione
            transform.position = Vector3.Lerp(targetPosition, returnPosition, elapsedTime / returnDuration);

            // Interpola rotazione verso quella originale
            transform.rotation = Quaternion.Lerp(targetRotation, returnRotation, elapsedTime / returnDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Distruggi l'ape dopo che ha completato il movimento
        Destroy(gameObject);
    }
    
    public void StartReturnSequence()
    {
        if (canReturn) 
            IsReturning = true;
    }
}