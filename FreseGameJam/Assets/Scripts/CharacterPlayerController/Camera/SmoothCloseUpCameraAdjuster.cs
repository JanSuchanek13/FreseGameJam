using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCloseUpCameraAdjuster : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _smoothTime = 0.05f;
    
    Vector3 velocity = Vector3.zero; // The velocity of the camera

    void Update()
    {
        // Get the target position:
        Vector3 targetPosition = _target.TransformPoint(new Vector3(0, 0.5f, 0));

        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime); // this dampends all axis

        // Smoothly dampen the adjustment of the y position:
        Vector3 _gettingThere = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
        transform.position = new Vector3(targetPosition.x, _gettingThere.y, targetPosition.z);
    }
}
