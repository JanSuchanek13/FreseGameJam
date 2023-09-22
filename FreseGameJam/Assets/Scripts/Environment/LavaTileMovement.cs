using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LavaTileMovement : MonoBehaviour
{
    [Header("Positions")]
    public Transform[] targetPositions; // Array der Zielpositionen

    [Header("Movement Settings")]
    public float moveSpeed = 2.0f; // Konstante Geschwindigkeit
    public Ease moveEase = Ease.Linear; // Easing-Funktion für die Bewegung
    public float wiggleDuration = 0.5f; // Dauer des Wackelns
    public float wiggleStrength = 0.1f; // Stärke des Wackelns
    public float bounceDistance = 0.5f; // Höhe des Auf- und Abbewegens
    public float stopDuration = 1.0f; // Dauer des Stopps
    public  int currentPositionIndex = 0;

    private bool isAtLastPosition = false;

    [Header("Fire")]
    public GameObject[] Fire;
    bool activeFireISRunning;

    private void Start()
    {
        // Prüfe, ob mindestens eine Zielposition vorhanden ist
        if (targetPositions.Length > 0)
        {
            if(currentPositionIndex == 0)
            {
                transform.GetChild(0).position = transform.position - new Vector3(0f, bounceDistance, 0f);
                gameObject.GetComponent<Collider>().enabled = false;
            }
            else 
            {
                transform.GetChild(0).position = transform.position;
            }

            // Starte die Bewegung zur ersten Zielposition
            MoveToNextPosition();
        }
        else
        {
            Debug.LogError("Keine Zielpositionen festgelegt.");
        }
    }

    private void MoveToNextPosition()
    {
        // Verwende DOTween, um das GameObject zur nächsten Zielposition zu bewegen
        if (currentPositionIndex < targetPositions.Length)
        {
            Transform targetPosition = targetPositions[currentPositionIndex];

            // Berechne die Entfernung zur nächsten Zielposition
            float distance = Vector3.Distance(transform.position, targetPosition.position);

            // Berechne die Dauer der Bewegung basierend auf der Entfernung und der konstanten Geschwindigkeit
            float moveDuration = distance / moveSpeed;

            // Bewege das GameObject zur Zielposition
            transform.DOMove(targetPosition.position, moveDuration)
                .SetEase(moveEase)
                .OnComplete(() =>
                {
                    if (currentPositionIndex == targetPositions.Length - 1)
                    {
                        // Wenn am letzten Positionspunkt, führe das Wackeln und Auf- und Abbewegen aus
                        PerformWiggleAndBounce();
                    }
                    else if(currentPositionIndex == 0)
                    {
                        // Nach dem Abwärtsbewegen, setze die Position zurück und bewege zum nächsten Punkt
                        transform.position = targetPositions[currentPositionIndex].position;
                        gameObject.GetComponent<Collider>().enabled = true;
                        transform.GetChild(0).position = transform.position; 
                        currentPositionIndex++;
                        MoveToNextPosition();
                    }
                    else
                    {
                        // Ansonsten, bewege einfach zum nächsten Punkt
                        currentPositionIndex++;
                        MoveToNextPosition();
                    }
                });
        }
        else
        {
            // Wenn alle Positionen durchlaufen wurden, fange von vorne an
            
            currentPositionIndex = 0;
            MoveToNextPosition();
        }
    }

    private void PlayBurnSound()
    {

    }

    public IEnumerator ActivateFire()
    {
        if (activeFireISRunning)
        {
            yield break;
        }
        activeFireISRunning = true;
        Debug.Log("Fire");

        foreach (GameObject i in Fire)
        {
            i.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            PlayBurnSound();
        }

        // Bewege das GameObject um einen halben Meter nach unten
        //Vector3 newPosition = transform.position - new Vector3(0f, bounceDistance, 0f);
        foreach (GameObject i in Fire)
        {
            i.SetActive(false);
        }
        activeFireISRunning = false;
        transform.GetChild(0).position = transform.position - new Vector3(0f, bounceDistance, 0f);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    private void PerformWiggleAndBounce()
    {
        // Wackeln


        // Bewege das GameObject um einen halben Meter nach unten
        transform.GetChild(0).position = transform.position - new Vector3(0f, bounceDistance, 0f);
        gameObject.GetComponent<Collider>().enabled = false;
        currentPositionIndex++;
        MoveToNextPosition();
    }
}
