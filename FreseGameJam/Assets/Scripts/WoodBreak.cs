using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBreak : MonoBehaviour
{

    [SerializeField] AudioSource[] arrayOfBreakingSounds;

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            AudioSource randomBreakingSound = arrayOfBreakingSounds[Random.Range(0, arrayOfBreakingSounds.Length)];
            randomBreakingSound.Play();
        }
    }

  
}
