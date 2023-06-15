using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCloseUpCameraAdjuster : MonoBehaviour
{
    public bool isFalling = false; // when this is set true, this object snaps to the player and thus the camera will no longer lag!
    //[SerializeField] GameObject _thirdPersonCam;
    //[SerializeField] GameObject _closeUpCam;

    [SerializeField] Transform _target;
    [SerializeField] float _smoothTime = 0.05f;

    //float _tempSmoothTime = 0.0f;
    
    //Vector3 velocity = Vector3.zero; // The velocity of the camera
    float _vel = 0.0f;

    
    /*
    void Update()
    {
        // Get the target position:
        Vector3 _targetPosition = _target.TransformPoint(new Vector3(0, 0.5f, 0));

        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref velocity, _smoothTime); // this dampends all axis

        //float newPosition = Mathf.SmoothDamp(transform.position.y, target.position.y, ref yVelocity, smoothTime);
        //float newPosition = Mathf.SmoothDamp(transform.position.y, _targetPosition.y, ref _vel, _smoothTime);
        //transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);

        // Smoothly dampen the adjustment of the y position:
        //Vector3 _gettingThere = Vector3.SmoothDamp(transform.position, _targetPosition, ref velocity, _smoothTime);
        //transform.position = new Vector3(_targetPosition.x, _gettingThere.y, _targetPosition.z);

        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, targetPosition.y, transform.position.z), ref velocity, _smoothTime);
    }*/
    /*private void Start()
    {
        _tempSmoothTime = _smoothTime;
    }*/
    public void ResetPosition()
    {
        transform.position = _target.position;
    }
    
    void Update()
    {
        // Get the target position:
        Vector3 targetPosition = _target.position;

        float maxSpeed = Mathf.Infinity;
        float deltaTime = Time.deltaTime;

        // Apply smoothing to the y-axis position with a delay:
        float smoothedY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref _vel, _smoothTime, maxSpeed, deltaTime);

        // Update the position with smoothed y-coordinate:
        Vector3 newPosition = new Vector3(targetPosition.x, smoothedY, targetPosition.z);

        // Update the camera's position to the new position:
        transform.position = newPosition;

        /*
        if (isFalling)
        {
            _thirdPersonCam.SetActive(true);   
            //_closeUpCam.SetActive(false);
        }
        else
        {
            _thirdPersonCam.SetActive(false);
        }*/
        /*
        if (!isFalling)
        {
            
        }else
        {
            transform.position = _target.position; // have camera look at the player

            // alternative, and this may be better: turn off the close up cam when falling and switch to 
            // third-person while falling for nice zoom out effect
        }*/

        /*
        // Get the target position:
        Vector3 targetPosition = _target.position;

        float maxSpeed = Mathf.Infinity;
        float deltaTime = Time.deltaTime;


        // Apply smoothing to the y-axis position with a delay:
        //float smoothedY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref _vel, _smoothTime);
        float smoothedY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref _vel, _smoothTime, maxSpeed, deltaTime);

        /* // this segment will stop the delay when falling:
        // Check if the player has fallen below a certain threshold:
        float fallThreshold = -2.0f; // Adjust this value as needed
        
        // Calculate the deviation between the dampened position and the tracked position
        positionDeviation = Mathf.Abs(smoothedY - targetPosition.y);

        //if (transform.position.y - targetPosition.y < fallThreshold)
        if (positionDeviation > deviationThreshold)
        {
            Debug.Log("1");
            // Limit the camera's vertical movement when player falls down:
            //_tempSmoothTime = _smoothTime;
            _smoothTime = 0.1f;
        }else
        {
            Debug.Log("2");
            _smoothTime = _tempSmoothTime;
        }/

        // Update the position with smoothed y-coordinate:
        Vector3 newPosition = new Vector3(targetPosition.x, smoothedY, targetPosition.z);

        // Update the camera's position to the new position:
        transform.position = newPosition;

        */
    }

    float positionDeviation = 0f;
    float deviationThreshold = 1.0f;

    /*void Update()
    {
        // Get the target position
        Vector3 targetPosition = _target.position;

        // Apply smoothing to the y-axis position with a delay
        float smoothedY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref _vel, _smoothTime);

        // Calculate the deviation between the dampened position and the tracked position
        positionDeviation = Mathf.Abs(smoothedY - targetPosition.y);

        // Check if the deviation exceeds the threshold
        if (positionDeviation > deviationThreshold)
        {
            // Limit the camera's vertical movement when the deviation is large
            smoothedY = Mathf.Lerp(transform.position.y, targetPosition.y, deviationThreshold / positionDeviation);
        }

        // Update the position with smoothed y-coordinate
        Vector3 newPosition = new Vector3(targetPosition.x, smoothedY, targetPosition.z);

        // Update the camera's position to the new position
        transform.position = newPosition;
    }*/
}
