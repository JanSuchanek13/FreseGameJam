using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPoint : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] float speed = 3.5f;
    private bool _startMoving = false;
    public void MoveIt()
    {
        _startMoving = true;
    }
    void FixedUpdate()
    {
        if (_startMoving)
        {
            var _movementIncrement = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, _movementIncrement);
        }
    }
}
