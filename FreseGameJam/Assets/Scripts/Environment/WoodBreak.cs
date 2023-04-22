using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBreak : MonoBehaviour
{

    //[SerializeField] AudioSource[] arrayOfBreakingSounds;
    [SerializeField] ParticleSystem Wood;

    [SerializeField] bool _canBreak = true;

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
        }
    }

  
}
