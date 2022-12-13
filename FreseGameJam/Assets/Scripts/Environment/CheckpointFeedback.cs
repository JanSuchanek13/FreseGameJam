using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFeedback : MonoBehaviour
{
    AudioSource _choireHymnn;
    AudioSource _fireSwoosh;
    bool _variablesAreUpdated = false;

    private void Awake()
    {
        Invoke("UpdateMyVariables", 0.2f);
    }
    void UpdateMyVariables()
    {
        _choireHymnn = GameObject.Find("GameManager").GetComponent<GameManager>().choireHymnSound;
        _fireSwoosh = GameObject.Find("GameManager").GetComponent<GameManager>().fireSwooshSound;
        _variablesAreUpdated = true;
        //Debug.Log("choire = " + _choireHymnn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") & _variablesAreUpdated)
        {
            // play Sounds when reaching checkpoint:
            _choireHymnn.Play();
            _fireSwoosh.Play();
            Destroy(this);
        }
    }
}
