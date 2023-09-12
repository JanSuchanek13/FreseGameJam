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
    public Ease moveEase = Ease.Linear; // Easing-Funktion f�r die Bewegung
    public float wiggleDuration = 0.5f; // Dauer des Wackelns
    public float wiggleStrength = 0.1f; // St�rke des Wackelns
    public float bounceDistance = 0.5f; // H�he des Auf- und Abbewegens
    public float stopDuration = 1.0f; // Dauer des Stopps

    private int currentPositionIndex = 0;
    private bool isAtLastPosition = false;

    [Header("Fire")]
    public GameObject[] Fire;
    bool activeFireISRunning;

    private void Start()
    {
        // Pr�fe, ob mindestens eine Zielposition vorhanden ist
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
        // Verwende DOTween, um das GameObject zur n�chsten Zielposition zu bewegen
        if (currentPositionIndex < targetPositions.Length)
        {
            Transform targetPosition = targetPositions[currentPositionIndex];

            // Berechne die Entfernung zur n�chsten Zielposition
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
                        // Wenn am letzten Positionspunkt, f�hre das Wackeln und Auf- und Abbewegen aus
                        StartCoroutine(PerformWiggleAndBounce());
                    }
                    else if(currentPositionIndex == 0)
                    {
                        // Nach dem Abw�rtsbewegen, setze die Position zur�ck und bewege zum n�chsten Punkt
                        transform.position = targetPositions[currentPositionIndex].position;
                        gameObject.GetComponent<Collider>().enabled = true;
                        currentPositionIndex++;
                        MoveToNextPosition();
                    }
                    else
                    {
                        // Ansonsten, bewege einfach zum n�chsten Punkt
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

        // Wackeln
        transform.DOPunchPosition(Vector3.right * wiggleStrength, wiggleDuration, 1, 0);


        // Bewege das GameObject um einen halben Meter nach unten
        //Vector3 newPosition = transform.position - new Vector3(0f, bounceDistance, 0f);
        foreach (GameObject i in Fire)
        {
            i.SetActive(false);
        }
        gameObject.GetComponent<Collider>().enabled = false;


        // Setze den Index zur�ck, um die normale Bewegung fortzusetzen
        currentPositionIndex = 0;
        activeFireISRunning = false;
    }

    private IEnumerator PerformWiggleAndBounce()
    {
        // Wackeln
        transform.DOPunchPosition(Vector3.right * wiggleStrength, wiggleDuration, 1, 0);

        // Warte f�r die Dauer des Stopps
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