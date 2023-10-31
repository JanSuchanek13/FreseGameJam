using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithBreakingWood : MonoBehaviour
{
    [Tooltip("Add all audiosources with regular-breaking sounds here.")]
    [SerializeField] AudioSource[] arrayOfBreakingSounds;
    [Tooltip("Add all audiosources with permanently-breaking sounds here.")]
    [SerializeField] AudioSource[] arrayOfPermanentlyBreakingSounds;

    bool _inCoroutine = false;

    /// <summary>
    /// This calls this piece of woods breaking sound. Note: permanently breaking objects call from a different set of sounds.
    /// 0 = regular breaking objects. 1 = permanently breaking objects.
    /// </summary>
    /// <param name="typeOfBreakIdentifier"></param>
    public void PlaySound(int typeOfBreakIdentifier)
    {
        if (!_inCoroutine)
        {
            StartCoroutine(PlayBreakingSound(typeOfBreakIdentifier));
        }
    }

    IEnumerator PlayBreakingSound(int typeOfBreakIdentifier)
    {
        _inCoroutine = true;
        AudioSource randomBreakingSound = null;

        switch (typeOfBreakIdentifier)
        {
            case 0:
                randomBreakingSound = arrayOfBreakingSounds[Random.Range(0, arrayOfBreakingSounds.Length)];
                break;
            
            case 1:
                randomBreakingSound = arrayOfPermanentlyBreakingSounds[Random.Range(0, arrayOfPermanentlyBreakingSounds.Length)];
                break;
        }
        //AudioSource randomBreakingSound = arrayOfBreakingSounds[Random.Range(0, arrayOfBreakingSounds.Length)];
        randomBreakingSound.Play();

        yield return new WaitForSeconds(randomBreakingSound.clip.length);
        _inCoroutine = false;
    }
}
