using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFeedback : MonoBehaviour
{
    [SerializeField] bool _useFeedback = true;

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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") & _variablesAreUpdated)
        {
            if (_useFeedback)
            {
                transform.GetChild(0).gameObject.SetActive(true); // turn on this checkpoints fire:
                _choireHymnn.Play();
                _fireSwoosh.Play();
            }
            
            // prevent resetting the spawn location to this point:
            GetComponent<Collider>().enabled = false;
            Destroy(this); 
        }
    }
}
