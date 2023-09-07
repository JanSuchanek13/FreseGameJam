using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LavaTileMovement : MonoBehaviour
{
    [Header("Positions")]
    public Transform[] targetPositions; // Array der Zielpositionen

    [Header("Movement Settings")]
    public float moveDuration = 2.0f; // Dauer der Bewegung
    public Ease moveEase = Ease.Linear; // Easing-Funktion für die Bewegung
    public float wiggleDuration = 0.5f; // Dauer des Wackelns
    public float wiggleStrength = 0.1f; // Stärke des Wackelns
    public float bounceDistance = 0.5f; // Höhe des Auf- und Abbewegens
    public float stopDuration = 1.0f; // Dauer des Stopps

    private int currentPositionIndex = 0;
    private bool isAtLastPosition = false;

    [Header("Fire")]
    public GameObject[] Fire;

    private void Start()
    {
        // Prüfe, ob mindestens eine Zielposition vorhanden ist
        if (targetPositions.Length > 0)
        {
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

            // Bewege das GameObject zur Zielposition
            transform.DOMove(targetPosition.position, moveDuration)
                .SetEase(moveEase)
                .OnComplete(() =>
                {
                    if (currentPositionIndex == targetPositions.Length - 1)
                    {
                        // Wenn am letzten Positionspunkt, führe das Wackeln und Auf- und Abbewegen aus
                        StartCoroutine(PerformWiggleAndBounce());
                    }
                    else if(currentPositionIndex == 0)
                    {
                        // Nach dem Abwärtsbewegen, setze die Position zurück und bewege zum nächsten Punkt
                        transform.position = targetPositions[currentPositionIndex].position;
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
        foreach (GameObject i in Fire)
        {
            i.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            PlayBurnSound();
        }

        // Wackeln
        transform.DOPunchPosition(Vector3.right * wiggleStrength, wiggleDuration, 1, 0);

        // Warte für die Dauer des Stopps
        yield return new WaitForSeconds(stopDuration);

        // Bewege das GameObject um einen halben Meter nach unten
        Vector3 newPosition = transform.position - new Vector3(0f, bounceDistance, 0f);
        foreach (GameObject i in Fire)
        {
            i.SetActive(false);
        }
    }

    private IEnumerator PerformWiggleAndBounce()
    {
        // Wackeln
        transform.DOPunchPosition(Vector3.right * wiggleStrength, wiggleDuration, 1, 0);

        // Warte für die Dauer des Stopps
        yield return new WaitForSeconds(stopDuration);

        // Bewege das GameObject um einen halben Meter nach unten
        Vector3 newPosition = transform.position - new Vector3(0f, bounceDistance, 0f);
        transform.DOMove(newPosition, wiggleDuration)
            .OnComplete(() =>
            {
                
                currentPositionIndex++;
                MoveToNextPosition();
            });
    }
}
