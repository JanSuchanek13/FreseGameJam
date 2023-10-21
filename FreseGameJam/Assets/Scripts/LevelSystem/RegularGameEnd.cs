using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loneflower; // need this as Jan established this in the SimpleAnimationController to find it.

public class RegularGameEnd : MonoBehaviour
{
    [Header("Regular Game End Settings:")]
    [SerializeField] Transform _targetTransform;
    [SerializeField] Transform _pointToWaveAt;

    [Tooltip("This sound is played when the player arrives at the endgame-trigger and runs towards his friend")]
    [SerializeField] AudioSource _victorySound_1;
    [Tooltip("This sound is played when the player arrives at their friend after the forced-run")]
    [SerializeField] AudioSource _victorySound_2;

    [Header("DO NOT TOUCH:")]
    public bool RegularEnd = false;

    private GameObject _player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && RegularEnd)
        {
            Debug.Log(other.name);

            // stopping the clock when end is reached, not need to fall to the trigger at the bottom!
            FindAnyObjectByType<LevelScript>().StopTheClock();
            Debug.Log("1");

            // force the player to run towards friend:
            FindAnyObjectByType<ThirdPersonMovement>().MoveToTarget(_targetTransform.position);
            Debug.Log("2");
            // start waving when reaching the friend:
            StartCoroutine("WaitUntilAtFriend");
            _player = other.gameObject;
            Debug.Log("3");
            // play sound or music #1 for finishing the game upon arriving at target trigger:
            if (_victorySound_1 != null)
            {
                _victorySound_1.Play();
            }
            Debug.Log("4");
        }
    }

    IEnumerator WaitUntilAtFriend()
    {
        yield return new WaitForSeconds(0.1f);

        // the distance at which the player stops moving towards the target (in MoveToTarget)
        float _stoppingDistanceFromTarget = 1.45f;
        float _tolerance = 0.05f; // add small offset as the floating decimal may not be hit exactly
        float _toleratedDistanceToStartWaving = _stoppingDistanceFromTarget + _tolerance;

        if (Vector3.Distance(_player.transform.position, _targetTransform.position) < _toleratedDistanceToStartWaving)
        {
            _player.transform.LookAt(_pointToWaveAt);
            _player.transform.Find("GFX:/PaperMan_Form").GetComponent<SimpleAnimationController>().StartWaving();

            // play sound or music #2 after having run to the friend and starting to wave:
            if (_victorySound_2 != null)
            {
                _victorySound_2.Play();
            }
        }
        else
        {
            StartCoroutine("WaitUntilAtFriend");
        }
    }
}
