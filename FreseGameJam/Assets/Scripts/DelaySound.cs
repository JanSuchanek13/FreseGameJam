using UnityEngine;

public class DelaySound : MonoBehaviour
{
    // put the TriggerSound-Script on the trigger-GO that you want to trigger the desired sound, then put that trigger-GO in the SerializeField in this script.

    [SerializeField] AudioSource targetAudioSource; // sound you want to have played
    [SerializeField] float audioDelayTime; // delay (from start) before playing the sound
    [SerializeField] bool playOnTrigger = false;
    [SerializeField] GameObject targetTrigger;

    void Start()
    {
        if (!playOnTrigger)
        {
            Invoke("PlaySound", audioDelayTime);
        }
    }
    
    void PlaySound()
    {
        targetAudioSource.Play();
    }
    void Update()
    {
        if (playOnTrigger)
        {
            if (targetTrigger.GetComponent<TriggerSound>().gotActivated)
            {
                PlaySound();
            }
        }
    }
}
