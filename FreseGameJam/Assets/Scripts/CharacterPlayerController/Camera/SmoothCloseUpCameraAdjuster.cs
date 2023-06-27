using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCloseUpCameraAdjuster : MonoBehaviour
{
    #region variables:
    // accessible variables:
    [Header("Smooth-Follow Settings:")]
    [Space(2)]

    [Tooltip("Insert the object this object should follow")]
    [SerializeField] Transform _target;
    [Space(2)]

    [Tooltip("How long does it take this object to match the followed objects position")]
    [SerializeField] float _smoothTime = 0.5f;
    [Space(2)]

    [Tooltip("Optionally declare an offset to the followed object. These values may be negative")]
    [SerializeField] Vector3 _followOffset = new Vector3(0.0f, 0.0f, 0.0f);

    // inaccessible variables:
    float _vel = 0.0f;
    #endregion

    /// <summary>
    /// This function is called by the "DieAndRespawn" function inside the "HealthSystem"-Script.
    /// It's puroise is, to jump the camera back to the players respawn position, so there is no camera-flight
    /// to that location after dying.
    /// </summary>
    public void ResetPosition()
    {
        transform.position = new Vector3(_target.position.x + _followOffset.x, _target.position.y + _followOffset.y, _target.position.z + _followOffset.z);
    }

    void Update()
    {
        // get target location including any offset:
        Vector3 targetPosition = new Vector3(_target.position.x + _followOffset.x, _target.position.y + _followOffset.y, _target.position.z + _followOffset.z);

        // smooth the transition on the y-axis only:
        float maxSpeed = Mathf.Infinity;
        float smoothedY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref _vel, _smoothTime, maxSpeed, Time.deltaTime);
        Vector3 newPosition = new Vector3(targetPosition.x, smoothedY, targetPosition.z);

        // change the position to the next increment:
        transform.position = newPosition;
    }
}
