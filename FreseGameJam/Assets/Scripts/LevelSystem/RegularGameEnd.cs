using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularGameEnd : MonoBehaviour
{
    public bool RegularEnd = false;
    [SerializeField] Transform _targetTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && RegularEnd)
        {
            // stopping the clock when end is reached, not need to fall to the trigger at the bottom!
            FindAnyObjectByType<LevelScript>().StopTheClock();

            FindAnyObjectByType<ThirdPersonMovement>().MoveToTarget(_targetTransform.position);

            // end of round still needs to be called here!
        }
    }
}
