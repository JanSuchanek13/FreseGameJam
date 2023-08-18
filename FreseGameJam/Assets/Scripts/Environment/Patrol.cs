using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5.0f;
    [SerializeField] float _holdFor = 0.25f;
    [Space(10)]
    [SerializeField] Transform _point1;
    [SerializeField] Transform _point2;

    public bool patrolling = true;
    IEnumerator Start()
    {
        Transform _target = _point1;
        //while (true)
        while (patrolling)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _target.position) <= 0.01f)
            {
                _target = _target == _point1 ? _point2 : _point1;
                yield return new WaitForSeconds(_holdFor);
            }
            yield return null;
        }
    }

    public void ContinuePatrol()
    {
        patrolling = true;
        StartCoroutine(Start());
    }
}
