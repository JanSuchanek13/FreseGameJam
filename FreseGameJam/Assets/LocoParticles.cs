using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocoParticles : MonoBehaviour
{
    [SerializeField] private GameObject pickupParticles;
    public void PickupParticles(Transform t)
    {
       
        var velocityModule = pickupParticles.GetComponent<ParticleSystem>().velocityOverLifetime;
        velocityModule.enabled = false;
        pickupParticles.GetComponent<particleAttractorLinear>().enabled = true;
        pickupParticles.GetComponent<particleAttractorLinear>().target = t;
        pickupParticles.transform.parent = null;
        //Destroy(pickupParticles, 0.5f); 
    }
}

