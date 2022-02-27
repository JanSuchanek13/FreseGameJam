using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector = Vector3.left;
    [SerializeField] private float speed = 1f;

    void Update()
    {
        transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
    }

}

