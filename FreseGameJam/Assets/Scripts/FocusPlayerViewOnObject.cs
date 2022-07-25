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
    private GameObject _focusTargetObject;
    private GameObject _playerCharacter;
    private bool _turnPlayerTowardsObject = false;
    private GameObject _gameManager;

    // save whether the cutscene already played:
    //int _cutSceneHasAlreadyPlayed = 0;
    #endregion
    private void Start()
    {
        _playerCharacter = this.gameObject;
        _gameManager = GameObject.Find("GameManager");
    }
    public void FocusTarget(GameObject focusObject, float lookAtThisForThisLong, int cutsceneNr) // focus camera:
    {
        // pause background track:
        switch (cutsceneNr)
        {
            case 0: // cutscene at start
                if (PlayerPrefs.GetInt("_cutScene_0_HasAlreadyPlayed") == 0)
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
                    //_cutSceneHasAlreadyPlayed++;
                    PlayerPrefs.SetInt("_cutScene_0_HasAlreadyPlayed", 1);

                    // MISSING: Activate waving-animation at target GO here!
                    //_playerCharacter.GetComponent<Movement(?)>().StartWaving();
                    //jansAnimationControlls.SetActive(false);
                    //wavingAnimation.Play();
                }
                break;
            case 1: // cutscene at bird
                if (PlayerPrefs.GetInt("_cutScene_1_HasAlreadyPlayed") == 0)
                {
                    //_gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

                    _focusTargetObject = focusObject;
                    Invoke("TurnOffFocus", lookAtThisForThisLong);
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);
                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_1_HasAlreadyPlayed", 1);
                }
                break;
            case 2: // cutscene at goat #1
                if (PlayerPrefs.GetInt("_cutScene_2_HasAlreadyPlayed") == 0)
                {
                    //_gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

                    _focusTargetObject = focusObject;
                    //Vector3 _focusTargetObjectPosition = new Vector3(focusObject.transform.position.x, (focusObject.transform.position.y + focusObject.GetComponent<MeshRenderer>().bounds.size.y), focusObject.transform.position.z);
                    Invoke("TurnOffFocus", lookAtThisForThisLong);
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;

                    focusCameraRig.GetComponent<CinemachineFreeLook>().m_Lens.NearClipPlane = 0.1f;
                    focusCameraRig.GetComponent<CinemachineCollider>().enabled = true;
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:

                    focusCameraRig.SetActive(true);
                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_2_HasAlreadyPlayed", 1);
                }
                break;
            case 3:
                if (PlayerPrefs.GetInt("_cutScene_3_HasAlreadyPlayed") == 0)
                {
                    //_gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

                    _focusTargetObject = focusObject;
                    Invoke("TurnOffFocus", lookAtThisForThisLong);
                    _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
                    _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
                    _turnPlayerTowardsObject = true;
                    focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
                    focusCameraRig.SetActive(true);
                    // save if cutscene has played:
                    PlayerPrefs.SetInt("_cutScene_3_HasAlreadyPlayed", 1);
                }
                break;

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
    void TurnOffFocus() // reset camera:
    {
        focusCameraRig.SetActive(false);
        Invoke("ReactivatePlayer", juicyDelayBeforePlayerResumesControll);
        // unpause background track:
        _gameManager.GetComponent<BackgroundSoundPlayer>().UnpauseMusic();
    }
    void ReactivatePlayer() // this is seperate from TurnOffFocus, so the player sees the avatar still looking at target GO:
    {
        _focusTargetObject = null;
        _turnPlayerTowardsObject = false;
        _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = true;
        // MISSING: Deactivate waving-animation at target GO here!
        //_playerCharacter.GetComponent<Movement(?)>().StopWaving();
        //wavingAnimation.Stop();
        //jansAnimationControlls.SetActive(true);
    }
    private void Update() // keep turning player to face a moving target GO:
    {
        if (_turnPlayerTowardsObject)
        {
            _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player to normal:
        }
    }
}
