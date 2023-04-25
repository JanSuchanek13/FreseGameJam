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

    bool _turnLeft = true;
    bool _turnRight = false;
    void Update()
    {
        if (!partialRotation)
        {
            transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
        }else
        {
            /*
            
            //transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
            if(transform.rotation.y < maxRotationLeft)
            {
                _turnLeft = true;
                _turnRight = false;
                //Debug.Log("Rotation threshold reached");
                transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);

            }else
            {
                _turnLeft = false;
                _turnRight = true;
                transform.Rotate(rotationVector*-1, speed * Time.deltaTime, Space.World);

            }
            if (transform.rotation.y >= maxRotationLeft || transform.rotation.y <= maxRotationRight)
            {
                //Debug.Log("Rotation threshold reached");
                rotationVector *= -1; // invert direction 
            }*/
        }
    }

}

