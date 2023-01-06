using UnityEngine;
using Cinemachine;


public class DelaySound : MonoBehaviour
{
    // put the "TriggerSound"-Script on the trigger-GO that you want to trigger the desired sound, then put that trigger-GO in the SerializeField in this script.
    #region variables

    [Header("Chosen AudioSource will play after [_audioDelayTime] seconds:")]
    [SerializeField] AudioSource _targetAudioSource; // sound you want to have played
    [SerializeField] float _audioDelayTime; // delay (from start) before playing the sound | this is automatically ignored, if you check "playOnTrigger"!

    [Header("Use Trigger with \"TriggerSound\"-Script to trigger this AudioSource?")]
    [SerializeField] bool _playOnTrigger = false;
    [SerializeField] GameObject _targetTrigger;

    [Header("Do you want the player to look at this audiosource when it plays?")]
    // do you want the player to look at this audiosource when it plays?
    [SerializeField] bool _activateFocusPlayerViewOnObjectWhenSoundPlays = false;
    [SerializeField] float _lookAtThisForThisLong = 4f;
    [SerializeField] GameObject _lookAtThisObjectAfterwards;
    [SerializeField] int _thisCutscene_Nr;
    [SerializeField] GameObject _pointToLookAt;

    // local variables:
    GameObject _playerCharacter;

    #endregion

    void Start()
    {
        _playerCharacter = GameObject.Find("Third Person Player_GameLevel_1");
        if (_playOnTrigger)
        {
            _targetTrigger.GetComponent<TriggerSound>().delaySound = this; // assign this instance of Delay sound to the correct trigger:
        }

        // when using a delay:
        // example: cutscene in beginning looking at boat.
        if (!_playOnTrigger)
        {
            Invoke("PlaySound", _audioDelayTime);
        }

        // when using a default focus point:
        // example: cutscene in beginning looking at boat.
        if (_pointToLookAt == null)
        {
            _pointToLookAt = this.gameObject;
        }
    }
    public void PlaySound()
    {
        //_currentCutscene = GameObject.Find("GameManager").GetComponent<GameManager>()._currentCutscene;

        // this is where the looking-part happens by calling "<FocusPlayerViewOnObject>().FocusTarget":
        // main camera will look at AudioSource:
        if (_activateFocusPlayerViewOnObjectWhenSoundPlays)
        {
            //_playerCharacter.GetComponent<FocusPlayerViewOnObject>().FocusTarget(_pointToLookAt, _lookAtThisForThisLong, _thisCutscene_Nr);
            //Debug.Log("called ForcusTarget with cutscene nr. " + _thisCutscene_Nr);

            if(_lookAtThisObjectAfterwards != null)
            {
                Invoke("LookAtThisNext", _lookAtThisForThisLong);
                _playerCharacter.GetComponent<FocusPlayerViewOnObject>().waitLonger = true; // prevents player from starting to move between chained cutscenes
            }

            _playerCharacter.GetComponent<FocusPlayerViewOnObject>().FocusTarget(_pointToLookAt, _lookAtThisForThisLong, _thisCutscene_Nr);

        }

        if (_targetAudioSource != null)
        {
            _targetAudioSource.Play();
        }
    }

    void LookAtThisNext() // dont think this works:
    {
        _pointToLookAt = _lookAtThisObjectAfterwards;
        _lookAtThisObjectAfterwards = null; // prevents this from running a loop!
        _thisCutscene_Nr++; // when using multiple focus points make sure that the next stop has a +2 increase of "_thisCutscene_Nr"
        _playerCharacter.GetComponent<FocusPlayerViewOnObject>().waitLonger = false; // enables players to move after chained cutscenes
        PlaySound();


        //_lookAtThisObjectAfterwards.GetComponent<DelaySound>().PlaySound();
        //_playerCharacter.GetComponent<FocusPlayerViewOnObject>().FocusTarget(_lookAtThisObjectAfterwards, _lookAtThisForThisLong, _thisCutscene_Nr);
    }


    /* // this should be directly called by the trigger! Now on TriggerSound!
    void Update() // when using a trigger:
    {
        if (_playOnTrigger)
        {
            if (_targetTrigger.GetComponent<TriggerSound>().gotActivated)
            {
                _targetTrigger.GetComponent<TriggerSound>().enabled = false;
                PlaySound();
            }
        }
    }*/
}
