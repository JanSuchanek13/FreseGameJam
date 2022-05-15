using UnityEngine;

public class FastFall : MonoBehaviour
{
    // This script can sit on a Trigger and will cause the player (when gliding) to accellerate downwards in an increasing trajectory (no-fly-zone):

    [SerializeField] float _gravityMultiplier = 1.0005f;
    bool _makeHimCrash = false;
    GameObject _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject;
            _makeHimCrash = true;
        }
    }
    void FixedUpdate()
    {
        if (_makeHimCrash)
        {
            _player.GetComponent<ThirdPersonMovement>().gravity *= _gravityMultiplier;
            Debug.Log("current gravity: " + _player.GetComponent<ThirdPersonMovement>().gravity);
            
            if (_player.GetComponent<HealthSystem>().inUse) //eg.: when dead!
            {
                _makeHimCrash = false;
            }
        }
    }
}
