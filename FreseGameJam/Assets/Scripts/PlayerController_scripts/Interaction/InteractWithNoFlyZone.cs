using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithNoFlyZone : MonoBehaviour
{
    // ask Felix

    #region variables:
    [SerializeField] AudioSource[] arrayOfWindSounds;
    [SerializeField] ParticleSystem windGhustEffect; // optional if we want some special effect to happen around the player when beign caught in the wind?!

    // local variables:
    bool _inCoroutine;
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "no_fly_zone" && !_inCoroutine)
        {
            StartCoroutine(ReactToZone());
        }
    }


    IEnumerator ReactToZone()
    {
        // Prevent this being triggered several times at once:
        _inCoroutine = true; 

        // Get random wind sound:
        AudioSource _randomTrack = arrayOfWindSounds[Random.Range(0, arrayOfWindSounds.Length)];
        
        // Random pitch:
        float _randomPitch = Random.Range(.75f, 1.25f);
        _randomTrack.pitch = _randomPitch;
        
        // Get sound length for the reset:
        float _lengthOfSound = _randomTrack.clip.length;
        
        _randomTrack.Play();

        // @JAN <---------------------------------------------------------------------------- put your stuff to play the right animation for _lengthOfSound seconds here.

        // Play optional effect, if assigned:
        if (windGhustEffect != null)
        {
            windGhustEffect.Play();
        }

        // Reset:
        yield return new WaitForSeconds(_lengthOfSound);

        _inCoroutine = false;
    }
}
