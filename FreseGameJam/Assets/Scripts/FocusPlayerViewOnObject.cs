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
    #endregion
    private void Start()
    {
        _playerCharacter = this.gameObject;
        _gameManager = GameObject.Find("GameManager");
    }
    public void FocusTarget(GameObject focusObject, float lookAtThisForThisLong) // focus camera:
    {
        // pause background track:
        _gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic();

        _focusTargetObject = focusObject;
        Invoke("TurnOffFocus", lookAtThisForThisLong);
        _playerCharacter.transform.LookAt(_focusTargetObject.transform.position); // turn player towards target GO:
        _playerCharacter.GetComponent<ThirdPersonMovement>().enabled = false;
        _turnPlayerTowardsObject = true;
        focusCameraRig.GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform; // turn camera:
        focusCameraRig.SetActive(true);
        // MISSING: Activate waving-animation at target GO here!
        //_playerCharacter.GetComponent<Movement(?)>().StartWaving();
        //jansAnimationControlls.SetActive(false);
        //wavingAnimation.Play();
    }
    void TurnOffFocus() // reset camera:
    {
        _focusTargetObject = null;
        focusCameraRig.SetActive(false);
        Invoke("ReactivatePlayer", juicyDelayBeforePlayerResumesControll);
        // unpause background track:
        _gameManager.GetComponent<BackgroundSoundPlayer>().UnpauseMusic();
    }
    void ReactivatePlayer() // this is seperate from TurnOffFocus, so the player sees the avatar still looking at target GO:
    {
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
