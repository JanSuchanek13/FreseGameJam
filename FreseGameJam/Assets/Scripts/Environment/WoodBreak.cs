using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBreak : MonoBehaviour
{

    //[SerializeField] AudioSource[] arrayOfBreakingSounds;
    [SerializeField] ParticleSystem Wood;
    [Space(10)]

    [SerializeField] float _delayForOtherParicle = 0.0f; // allow eg wood to break before splashing water
    [SerializeField] ParticleSystem _otherParticle;


    [SerializeField] bool _canBreak = true;

    /// <summary>
    /// If a "BreakingObj" script is attached as well, then overwrite the delay for the secondary particle with the breaktime from that script.
    /// </summary>
    private void Start()
    {
        if(GetComponent<BreakingObj>() != null)
        {
            _delayForOtherParicle = GetComponent<BreakingObj>().timeTillBreak; 
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && _canBreak )
        {
            //AudioSource randomBreakingSound = arrayOfBreakingSounds[Random.Range(0, arrayOfBreakingSounds.Length)];
            //randomBreakingSound.Play();
            other.GetComponent<InteractWithBreakingWood>().PlaySound();
           
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
