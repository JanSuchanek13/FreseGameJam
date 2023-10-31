using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBreak : MonoBehaviour
{
    [SerializeField] ParticleSystem Wood;
    
    [Space(10)]
    [SerializeField] float _delayForOtherParicle = 0.0f; // allow eg wood to break before splashing water
    [SerializeField] ParticleSystem _otherParticle;

    [SerializeField] bool _canBreak = true;
    [SerializeField] bool _breaksPermanently = false;
    int _typeOfBreakIdentifier;

    /// <summary>
    /// If a "BreakingObj" script is attached as well, then overwrite the delay for the secondary particle with the breaktime from that script.
    /// </summary>
    private void Start()
    {
        // set identifier for regular or permanently breaking wood:
        _typeOfBreakIdentifier = 0;
        if (_breaksPermanently)
        {
            _typeOfBreakIdentifier = 1;
        }

        if(GetComponent<BreakingObj>() != null)
        {
            _delayForOtherParicle = GetComponent<BreakingObj>().timeTillBreak; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && _canBreak )
        {
            // call the function to play sound, depending on the type of breaking object:
            other.GetComponent<InteractWithBreakingWood>().PlaySound(_typeOfBreakIdentifier);
           
            if(Wood != null)
            {
                Wood.Play();
            }

            // play secondary optional particle effect at an optional delay:
            if (_otherParticle != null)
            {
                Invoke("PlayOtherParticles", _delayForOtherParicle);
            }
        }
    }
    void PlayOtherParticles()
    {
        _otherParticle.Play();
    }

  
}
