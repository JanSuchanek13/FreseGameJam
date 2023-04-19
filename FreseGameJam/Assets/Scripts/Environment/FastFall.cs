using UnityEngine;

public class FastFall : MonoBehaviour
{
    // This script can sit on a Trigger and will cause the player (when gliding) to accellerate downwards in an increasing trajectory (no-fly-zone):

    [SerializeField] float _gravityMultiplier = 1.0005f;
    bool _makeHimCrash = false;
    GameObject _player;
    float _gravity = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject;
            _gravity = _player.GetComponent<ThirdPersonMovement>().gravity; // save a default gravity value
            _makeHimCrash = true;
        }
    }
    void FixedUpdate()
    {
        if (_makeHimCrash)
        {
            _player.GetComponent<ThirdPersonMovement>().gravity *= _gravityMultiplier;
            //Debug.Log("current gravity: " + _player.GetComponent<ThirdPersonMovement>().gravity);
            
            if (_player.GetComponent<HealthSystem>().inUse || _player.GetComponent<ThirdPersonMovement>().CheckForGroundContact()) //eg.: when dead or no longer falling.
            {
                _makeHimCrash = false;
                _player.GetComponent<ThirdPersonMovement>().gravity = _gravity; // reset gravity
                //Debug.Log("stopp falling fast.");
            }
        }
    }
}
