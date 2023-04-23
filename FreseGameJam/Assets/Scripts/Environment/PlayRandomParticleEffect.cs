using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomParticleEffect : MonoBehaviour
{
    [Tooltip("Particle-Systems have to be active inside the inactive parent-GO. They also need to be set to loop. Consider using pre-warm to make the effect look as if it's in full swing from the get-go.")]
    [SerializeField] GameObject[] _arrayOfParticleSystems;

    /// <summary>
    /// Add all desired particle-system-parents (GO) to the array. At game start a random one will be selected and played from the start. 
    /// </summary>
    void Start()
    {
        GameObject _randomParticleSystem = _arrayOfParticleSystems[Random.Range(0, _arrayOfParticleSystems.Length)];
        _randomParticleSystem.SetActive(true);
    }
}
