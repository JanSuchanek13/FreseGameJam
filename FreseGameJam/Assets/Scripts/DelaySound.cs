using UnityEngine;
using Cinemachine;


public class DelaySound : MonoBehaviour
{
    // put the "TriggerSound"-Script on the trigger-GO that you want to trigger the desired sound, then put that trigger-GO in the SerializeField in this script.
    #region variables
    [Header("Chosen AudioSource will play after [audioDelayTime] seconds:")]
    [SerializeField] AudioSource targetAudioSource; // sound you want to have played
    [SerializeField] float audioDelayTime; // delay (from start) before playing the sound | this is automatically ignored, if you check "playOnTrigger"!

    [Header("Use Trigger with \"TriggerSound\"-Script to trigger this AudioSource?")]
    [SerializeField] bool playOnTrigger = false;
    [SerializeField] GameObject targetTrigger;

    [Header("Do you want the player to look at this audiosource when it plays?")]
    // do you want the player to look at this audiosource when it plays?
    [SerializeField] bool activateFocusPlayerViewOnObjectWhenSoundPlays = false;
    [SerializeField] float lookAtThisForThisLong = 4f;
    [SerializeField] GameObject playerCharacter;
    [SerializeField] GameObject lookAtThisObjectAfterwards;
    [SerializeField] int _thisCutscene_Nr;


    #endregion

    void Start()
    {
        // when using a delay:
        if (!playOnTrigger)
        {
            Invoke("PlaySound", audioDelayTime);
        }
    }
    public void PlaySound()
    {
        //_currentCutscene = GameObject.Find("GameManager").GetComponent<GameManager>()._currentCutscene;

        if (activateFocusPlayerViewOnObjectWhenSoundPlays) // when having the camera look at AudioSource:
        {
            playerCharacter.GetComponent<FocusPlayerViewOnObject>().FocusTarget(this.gameObject, lookAtThisForThisLong, _thisCutscene_Nr);
            Debug.Log("called ForcusTarget with cutscene nr. " + _thisCutscene_Nr);

            if(lookAtThisObjectAfterwards != null)
            {
                Invoke("LookAtThisNext", lookAtThisForThisLong);
            }
        }
        if(targetAudioSource != null)
        {
            targetAudioSource.Play();
        }
    }

    void LookAtThisNext()
    {
        lookAtThisObjectAfterwards.GetComponent<DelaySound>().PlaySound();
    }
    void Update() // when using a trigger:
    {
        if (playOnTrigger)
        {
            if (targetTrigger.GetComponent<TriggerSound>().gotActivated)
            {
                targetTrigger.GetComponent<TriggerSound>().enabled = false;
                PlaySound();
            }
        }
    }
}
