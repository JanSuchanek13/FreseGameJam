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

    [Header("Do you want anything turnt on/off?")]
    [SerializeField] GameObject _turnThisOffAfterCutscene;
    [SerializeField] GameObject _turnThisOnAfterCutscene;


    // local variables:
    GameObject _playerCharacter;

    bool _calledTheNextFocusPointOnce = false;


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
        if (_activateFocusPlayerViewOnObjectWhenSoundPlays)
        {
            if(_lookAtThisObjectAfterwards != null && !_calledTheNextFocusPointOnce)
            {
                _calledTheNextFocusPointOnce = true; // without this, play sound keeps triggering and calling this!

                Invoke("LookAtThisNext", _lookAtThisForThisLong);
                _playerCharacter.GetComponent<FocusPlayerViewOnObject>().waitLonger = true; // prevents player from starting to move between chained cutscenes
                Debug.Log("wait longer = true!");
            }

            _playerCharacter.GetComponent<FocusPlayerViewOnObject>().FocusTarget(_pointToLookAt, _lookAtThisForThisLong, _thisCutscene_Nr);

        }

        if (_targetAudioSource != null)
        {
            _targetAudioSource.Play();
        }

        // if anything is entered in these variables they will be turnt on/off after the time it takes to look at the cutscene:
        // these are seperated, as you may only wish to hide something, or turn something on, but you can do both by filling both!
        if(_turnThisOffAfterCutscene != null)
        {
            Invoke("TurnSomethingOff", _lookAtThisForThisLong);
        }
        if (_turnThisOnAfterCutscene != null)
        {
            Invoke("TurnSomethingOn", _lookAtThisForThisLong);
        }
    }

    void TurnSomethingOff()
    {
        _turnThisOffAfterCutscene.SetActive(false);
    }
    void TurnSomethingOn()
    {
        _turnThisOnAfterCutscene.SetActive(true);
    }

    void LookAtThisNext()
    {
        _pointToLookAt = _lookAtThisObjectAfterwards;
        _lookAtThisObjectAfterwards = null; // prevents this from running a loop!
        _thisCutscene_Nr++; // when using multiple focus points make sure that the next stop has a +2 increase of "_thisCutscene_Nr"
        
        // check if the cutscene was skipped before proceeding (skipping will turn the bool "waitLonger" false):
        if (_playerCharacter.GetComponent<FocusPlayerViewOnObject>().waitLonger)
        {
            Debug.Log("this was called 3");

            _playerCharacter.GetComponent<FocusPlayerViewOnObject>().waitLonger = false; // enables players to move after chained cutscenes
            PlaySound();
        }
    }
}
