using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithBreakingWood : MonoBehaviour
{
    [SerializeField] AudioSource[] arrayOfBreakingSounds;

    bool _inCoroutine = false;

    public void PlaySound()
    {
        //Debug.Log("called play sound");
        if (!_inCoroutine)
        {
            //Debug.Log("executing coroutine");
            StartCoroutine(PlayBreakingSound());
        }
    }
    IEnumerator PlayBreakingSound()
    {
        _inCoroutine = true;
        AudioSource randomBreakingSound = arrayOfBreakingSounds[Random.Range(0, arrayOfBreakingSounds.Length)];
        randomBreakingSound.Play();
        //Debug.Log("breaking sound length" + randomBreakingSound.clip.length);

        yield return new WaitForSeconds(randomBreakingSound.clip.length);
        _inCoroutine = false;
        //Debug.Log("executing coroutine finished");
    }


}
