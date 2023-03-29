using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class FocusPlayerViewOnObject : MonoBehaviour
{
    #region script description:
    // this script depends on "DelaySound"- and/or "TriggerSound"-Scripts.
    // this script activates a focus camera rig component that will zoom in on target audiosource-GO (eg. a screaming person etc.).
    // hence: you need an correctly set "CinemachineFreeLook"-Component on your player-character-GO.
    #endregion

    #region variables:
    // local variables:
    [SerializeField] GameObject focusCameraRig;
    [SerializeField] float juicyDelayBeforePlayerResumesControll = 2f;
    [SerializeField] GameObject jansAnimationControlls;
    [SerializeField] Animation wavingAnimation;
    [SerializeField] Animator _humanAnimator;

    GameObject _focusTargetObject;
    GameObject _playerCharacter;
    
    BackgroundSoundPlayer _backgroundSoundPlayer;

    bool _turnPlayerTowardsObject = false;
    bool _skippingCutscene = false;
    bool _inFocusMode = false;

    [SerializeField] GameObject _skipUI;

    // public variables:
    public bool waitLonger = false;
    #endregion


    private void Start()
    {
        _playerCharacter = this.gameObject;
        _backgroundSoundPlayer = FindObjectOfType<BackgroundSoundPlayer>();
    }

    
    /// <summary>
    /// Allows players to skip a cutscene.
    /// </summary>
    public void SkipCutscene()
    {
        if (_inFocusMode)
        {
            _inFocusMode = false; // prevents delayed turning off of the focus camera
            StartCoroutine(TurnOffFocus(0.0f)); 
            ReactivatePlayer();
            waitLonger = false; // prevents chained cutscenes to be played after oneanother

            // zooming only possible if player is NOT in cutscene:
            FindObjectOfType<CloseQuarterCamera>().inCustscene = false;
        }
    }

    void LookAtThisObject(GameObject _focusObject, float _lookAtThisForThisLong)
    {
        _backgroundSoundPlayer.PauseMusic();
        _focusTargetObject = _focusObject;

        if (!waitLonger)
        {
            StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
        }

        // stop all player input, this will cause avatar to stop, look at X and idle:
        FindObjectOfType<InputHandler>().enabled = false;
        _turnPlayerTowardsObject = true;

        // turn player-camera to focus X:
        focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform;
        focusCameraRig.SetActive(true);

        // turn on the SKIP-instructions:
        _skipUI.SetActive(true);
    }
    void FollowThisObject(GameObject _focusObject, float _lookAtThisForThisLong)
    {
        _backgroundSoundPlayer.PauseMusic();
        _focusTargetObject = _focusObject;

        if (!waitLonger)
        {
            StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
        }

        // stop all player input, this will cause avatar to stop, look at X and idle:
        FindObjectOfType<InputHandler>().enabled = false;
        _turnPlayerTowardsObject = true;

        // turn player-camera to focus X AND FOLLOW:
        focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
        focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
        focusCameraRig.SetActive(true);

        // turn on the SKIP-instructions:
        _skipUI.SetActive(true);
    }
    public void FocusTarget(GameObject _focusObject, float _lookAtThisForThisLong, int _cutsceneNr) // focus camera:
    {
        _inFocusMode = true;
        FindObjectOfType<CloseQuarterCamera>().inCustscene = true;

        switch (_cutsceneNr)
        {
            case 0: // cutscene at start
                if (PlayerPrefs.GetInt("_cutScene_0_HasAlreadyPlayed") == 0)
                {
                    LookAtThisObject(_focusObject, _lookAtThisForThisLong);

                    PlayerPrefs.SetInt("_cutScene_0_HasAlreadyPlayed", 1);
                }
                break;

            case 1: // cutscene for instructions (double)
                if (PlayerPrefs.GetInt("_cutScene_1_HasAlreadyPlayed") == 0)
                {
                    FollowThisObject(_focusObject, _lookAtThisForThisLong);

                    PlayerPrefs.SetInt("_cutScene_1_HasAlreadyPlayed", 1);
                }
                break;

            case 2: // cutscene at goat #1
                if (PlayerPrefs.GetInt("_cutScene_2_HasAlreadyPlayed") == 0)
                {
                    FollowThisObject(_focusObject, _lookAtThisForThisLong);

                    /* // Why is this here?! delete if game plays well without!
                    focusCameraRig.GetComponent<CinemachineFreeLook>().m_Lens.NearClipPlane = 0.1f;
                    focusCameraRig.GetComponent<CinemachineCollider>().enabled = true;
                    */

                    PlayerPrefs.SetInt("_cutScene_2_HasAlreadyPlayed", 1);
                }
                break;

            case 3:
                if (PlayerPrefs.GetInt("_cutScene_3_HasAlreadyPlayed") == 0)
                {
                    FollowThisObject(_focusObject, _lookAtThisForThisLong);

                    PlayerPrefs.SetInt("_cutScene_3_HasAlreadyPlayed", 1);
                }
                break;

            case 4:
                if (PlayerPrefs.GetInt("_cutScene_4_HasAlreadyPlayed") == 0)
                {
                    FollowThisObject(_focusObject, _lookAtThisForThisLong);

                    PlayerPrefs.SetInt("_cutScene_4_HasAlreadyPlayed", 1);
                }
                break;
            
            case 5:
                if (PlayerPrefs.GetInt("_cutScene_5_HasAlreadyPlayed") == 0)
                {
                    FollowThisObject(_focusObject, _lookAtThisForThisLong);

                    PlayerPrefs.SetInt("_cutScene_5_HasAlreadyPlayed", 1);
                }
                break;

            case 6:
                if (PlayerPrefs.GetInt("_cutScene_6_HasAlreadyPlayed") == 0)
                {
                    FollowThisObject(_focusObject, _lookAtThisForThisLong);

                    PlayerPrefs.SetInt("_cutScene_6_HasAlreadyPlayed", 1);
                }
                break;

            case 7:
                if (PlayerPrefs.GetInt("_cutScene_7_HasAlreadyPlayed") == 0)
                {
                    FollowThisObject(_focusObject, _lookAtThisForThisLong);

                    PlayerPrefs.SetInt("_cutScene_7_HasAlreadyPlayed", 1);
                }
                break;

                default:
                return;

        }
    }


    // if several objects are chained for looking at, this will get delayed via the bool "waitLonger":
    IEnumerator TurnOffFocus(float _lookAtThisForThisLong)
    {
        yield return new WaitForSeconds(_lookAtThisForThisLong);
        
        // if a cutscene called TurnOffFocus with its wait-time, then skipping will not be able to reach that
        // so this if statement checks if the wait time was longer than 0, thus being a regular wait time, and then cancels that instance
        // of this execution when SkipCutscene is called (which turns _skippingCutscene true)
        if(_lookAtThisForThisLong > 0.0f && !_inFocusMode) // checks if this was called normally but cancelled later
        {
            yield break;
        }

        _backgroundSoundPlayer.UnpauseMusic(); // this is currently suboptimal, as time paused is not accounted for and tracks will play next to eachother.
        focusCameraRig.SetActive(false);
        
        Invoke("ReactivatePlayer", juicyDelayBeforePlayerResumesControll);
    }


    void ReactivatePlayer() // this is seperate from TurnOffFocus, so the player sees the avatar still looking at target GO:
    {
        _focusTargetObject = null;
        _turnPlayerTowardsObject = false;

        // turn OFF the SKIP-instructions:
        _skipUI.SetActive(false);

        // give controls back to player:
        FindObjectOfType<InputHandler>().enabled = true;
    }

    private void Update() // keep turning player to face a moving target GO:
    {
        if (_turnPlayerTowardsObject)
        {
            _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards focus object:
        }

        // delete this when jan implemented this in player input!
        if (Input.GetKeyDown(KeyCode.Tab) && _inFocusMode)
        {
            SkipCutscene();
        }
    }
}
