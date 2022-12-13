using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector = Vector3.left;
    [SerializeField] private float speed = 1f;
    //new variables
    [SerializeField] bool partialRotation = false;
    [SerializeField] float maxRotationLeft = 100;
    [SerializeField] float maxRotationRight = -100;

    void Update()
    {
        if (!partialRotation)
        {
            transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
            if(transform.rotation.y == maxRotationLeft)
            {
                Debug.Log("Rotation threshold reached");

            }
            if (transform.rotation.y >= maxRotationLeft || transform.rotation.y <= maxRotationRight)
            {
                Debug.Log("Rotation threshold reached");
                rotationVector *= -1; // invert direction 
            }
        }
    }

}

