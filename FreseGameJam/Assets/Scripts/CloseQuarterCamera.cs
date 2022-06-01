using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CloseQuarterCamera : MonoBehaviour
{
    //[SerializeField] float newCameraMaxDistance_top = 8;
    //[SerializeField] float newCameraMaxDistance_middle = 8;
    private GameObject _player;
    private GameObject _cameraRigFar;
    //private GameObject _cameraRigClose;


    public bool _closeQuarterCameraIsActive = false;
    public bool _currentlyInsideTrigger = false;

    void Start()
    {
        _player = GameObject.Find("Third Person Player");
        _cameraRigFar = _player.transform.GetChild(0).gameObject;
        //_cameraRigClose = _player.transform.GetChild(1).gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_currentlyInsideTrigger)
        {
            _currentlyInsideTrigger = true;

            if (!_closeQuarterCameraIsActive)
            {
                _cameraRigFar.SetActive(false);
                _closeQuarterCameraIsActive = true;
            }
            else
            {
                _cameraRigFar.SetActive(true);
                _closeQuarterCameraIsActive = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _currentlyInsideTrigger = false;
        }
    }
}
