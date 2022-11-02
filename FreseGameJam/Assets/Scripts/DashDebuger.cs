using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DashBehaviour))]
public class DashDebuger : MonoBehaviour
{
    private Transform startPos;
    private Transform endPos;
    public float distance;

    private void Awake()
    {
        startPos = new GameObject().transform;
        startPos.gameObject.name = "startPos_Debuger";
        endPos = new GameObject().transform;
        endPos.gameObject.name = "endPos_Debuger";
    }

    private void OnEnable()
    {
        DashBehaviour.onDashStart_Event += OnDashStart;
        DashBehaviour.onDashEnd_Event += OnDashEnd;
    }

    private void OnDisable()
    {
        DashBehaviour.onDashStart_Event -= OnDashStart;
        DashBehaviour.onDashEnd_Event -= OnDashEnd;
    }

    public void OnDashStart(Vector3 _pos, Vector3 _direction)
    {
        startPos.position = _pos;
    }

    public void OnDashEnd(Vector3 _pos, Vector3 _direction)
    {
        endPos.position = _pos;
        distance = Vector3.Distance(endPos.position, startPos.position);
        Debug.Log("Dash Distance:" + distance.ToString("F2"));
    }

}
