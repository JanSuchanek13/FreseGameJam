using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loneflower; // need this as Jan established this in the SimpleAnimationController to find it.

public class RegularGameEnd : MonoBehaviour
{
    public bool RegularEnd = false;
    [SerializeField] Transform _targetTransform;

    private GameObject _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && RegularEnd)
        {
            Debug.Log(other.name);

            // stopping the clock when end is reached, not need to fall to the trigger at the bottom!
            FindAnyObjectByType<LevelScript>().StopTheClock();

            FindAnyObjectByType<ThirdPersonMovement>().MoveToTarget(_targetTransform.position);

            StartCoroutine("WaitUntilAtFriend");
            _player = other.gameObject;
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
            _player.transform.Find("GFX:/PaperMan_Form").GetComponent<SimpleAnimationController>().StartWaving();
        }else
        {
            StartCoroutine("WaitUntilAtFriend");
        }
    }
}
