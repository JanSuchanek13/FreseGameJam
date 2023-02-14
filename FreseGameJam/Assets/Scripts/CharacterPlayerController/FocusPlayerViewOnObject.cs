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
    [SerializeField] GameObject focusCameraRig;
    [SerializeField] float juicyDelayBeforePlayerResumesControll = 2f;
    [SerializeField] GameObject jansAnimationControlls;
    [SerializeField] Animation wavingAnimation;
    [SerializeField] Animator _humanAnimator;

    // local variables:
    private GameObject _focusTargetObject;
    private GameObject _playerCharacter;
    private bool _turnPlayerTowardsObject = false;
    private GameObject _gameManager;

    // public variables:
    public bool waitLonger = false;

    // save whether the cutscene already played:
    //int _cutSceneHasAlreadyPlayed = 0;
    #endregion


    private void Start()
    {
        _playerCharacter = this.gameObject;
        _gameManager = GameObject.Find("GameManager");
    }


    public void FocusTarget(GameObject _focusObject, float _lookAtThisForThisLong, int _cutsceneNr) // focus camera:
    {
        // pause background track:
        switch (_cutsceneNr)
        {
            case 0: // cutscene at start
                if (PlayerPrefs.GetInt("_cutScene_0_HasAlreadyPlayed") == 0)
                {
                    _gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

                    _focusTargetObject = _focusObject;

                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                        //Invoke("TurnOffFocus", _lookAtThisForThisLong);
                    }

                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    //focusCameraRig.GetComponent<CinemachineFreeLook>().m_Orbits.

                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);

                    // test JAN:
                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    //_cutSceneHasAlreadyPlayed++;
                    PlayerPrefs.SetInt("_cutScene_0_HasAlreadyPlayed", 1);

                    // MISSING: Activate waving-animation at target GO here!
                    //_playerCharacter.GetComponent<Movement(?)>().StartWaving();
                    //jansAnimationControlls.SetActive(false);
                    //wavingAnimation.Play();
                }
                break;

            case 1: // cutscene for instructions (double)
                if (PlayerPrefs.GetInt("_cutScene_1_HasAlreadyPlayed") == 0)
                {
                    //_gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

                    _focusTargetObject = _focusObject;
                    
                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                        //Invoke("TurnOffFocus", _lookAtThisForThisLong);
                    }

                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    
                    _turnPlayerTowardsObject = true; // what does this bool do?! -F it controlls the update function -F

                    focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);

                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_1_HasAlreadyPlayed", 1);
                }
                break;

            case 2: // cutscene at goat #1
                if (PlayerPrefs.GetInt("_cutScene_2_HasAlreadyPlayed") == 0)
                {
                    //_gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

                    _focusTargetObject = _focusObject;
                    //Vector3 _focusTargetObjectPosition = new Vector3(focusObject.transform.position.x, (focusObject.transform.position.y + focusObject.GetComponent<MeshRenderer>().bounds.size.y), focusObject.transform.position.z);
                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                        //Invoke("TurnOffFocus", _lookAtThisForThisLong);
                    }
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().m_Lens.NearClipPlane = 0.1f;
                    focusCameraRig.GetComponent<CinemachineCollider>().enabled = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:

                    focusCameraRig.SetActive(true);

                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_2_HasAlreadyPlayed", 1);
                }
                break;

            case 3:
                if (PlayerPrefs.GetInt("_cutScene_3_HasAlreadyPlayed") == 0)
                {
                    //_gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

                    _focusTargetObject = _focusObject;
                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                        //Invoke("TurnOffFocus", _lookAtThisForThisLong);
                    }
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);

                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_3_HasAlreadyPlayed", 1);
                }
                break;

            case 4:
                if (PlayerPrefs.GetInt("_cutScene_4_HasAlreadyPlayed") == 0)
                {
                    _focusTargetObject = _focusObject;
                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                    }
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);

                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_4_HasAlreadyPlayed", 1);
                }
                break;
            
            case 5:
                if (PlayerPrefs.GetInt("_cutScene_5_HasAlreadyPlayed") == 0)
                {
                    _focusTargetObject = _focusObject;
                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                    }
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);

                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_5_HasAlreadyPlayed", 1);
                }
                break;

            case 6:
                if (PlayerPrefs.GetInt("_cutScene_6_HasAlreadyPlayed") == 0)
                {
                    _focusTargetObject = _focusObject;
                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                    }
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);

                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_6_HasAlreadyPlayed", 1);
                }
                break;

            case 7:
                if (PlayerPrefs.GetInt("_cutScene_7_HasAlreadyPlayed") == 0)
                {
                    _focusTargetObject = _focusObject;
                    if (!waitLonger)
                    {
                        StartCoroutine(TurnOffFocus(_lookAtThisForThisLong));
                    }
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().Follow = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);

                    _humanAnimator.enabled = false;

                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_7_HasAlreadyPlayed", 1);
                }
                break;

                default:
                return;

        }
        /*
        if(PlayerPrefs.GetInt("_cutSceneHasAlreadyPlayed") == 0)
        {
            _gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

            _focusTargetObject = focusObject;
            Invoke("TurnOffFocus", lookAtThisForThisLong);
            _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
            _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
            _turnPlayerTowardsObject = true;
            focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
            focusCameraRig.SetActive(true);
            // save if cutscene has played:
            _cutSceneHasAlreadyPlayed++;
            PlayerPrefs.SetInt("_cutSceneHasAlreadyPlayed", _cutSceneHasAlreadyPlayed);

            // MISSING: Activate waving-animation at target GO here!
            //_playerCharacter.GetComponent<Movement(?)>().StartWaving();
            //jansAnimationControlls.SetActive(false);
            //wavingAnimation.Play();
        }else if (PlayerPrefs.GetInt("_cutSceneHasAlreadyPlayed") == 0)
        {
            _gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

            _focusTargetObject = focusObject;
            Invoke("TurnOffFocus", lookAtThisForThisLong);
            _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
            _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
            _turnPlayerTowardsObject = true;
            focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
            focusCameraRig.SetActive(true);
            // save if cutscene has played:
            _cutSceneHasAlreadyPlayed++;
            PlayerPrefs.SetInt("_cutSceneHasAlreadyPlayed", _cutSceneHasAlreadyPlayed);

            // MISSING: Activate waving-animation at target GO here!
            //_playerCharacter.GetComponent<Movement(?)>().StartWaving();
            //jansAnimationControlls.SetActive(false);
            //wavingAnimation.Play();
        }*/
    }


    // if several objects are chained for looking at, this will get delayed via the bool "waitLonger":
    IEnumerator TurnOffFocus(float _lookAtThisForThisLong)
    {
        yield return new WaitForSeconds(_lookAtThisForThisLong);
        
        focusCameraRig.SetActive(false);
        Invoke("ReactivatePlayer", juicyDelayBeforePlayerResumesControll);
        
        // unpause background track:
        _gameManager.GetComponent<BackgroundSoundPlayer>().UnpauseMusic();

        Debug.Log("TurnOffFocus was called");
    }


    void ReactivatePlayer() // this is seperate from TurnOffFocus, so the player sees the avatar still looking at target GO:
    {
        _focusTargetObject = null;
        _turnPlayerTowardsObject = false;
        _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = true;

        // test JAN:
        _humanAnimator.enabled = true;
        // MISSING: Deactivate waving-animation at target GO here!
        //_playerCharacter.GetComponent<Movement(?)>().StopWaving();
        //wavingAnimation.Stop();
        //jansAnimationControlls.SetActive(true);
        
        Debug.Log("ReactivatePlayer");
    }

    // shouldn't this be a simple function calling it once + disabling any input in the meanwhile?! -F
    private void Update() // keep turning player to face a moving target GO:
    {
        if (_turnPlayerTowardsObject)
        {
            _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player to normal:
        }
    }
}
