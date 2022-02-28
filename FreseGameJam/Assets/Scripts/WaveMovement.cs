using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour
{
	public Transform startPoint;
	public Transform endPoint;
	public float travelTime;
	private Rigidbody rb;
	private Vector3 currentPos;
	private Vector3 startPosition;

	CharacterController cc;

	private void Start()
	{
		startPosition = transform.localPosition;
		rb = GetComponent<Rigidbody>();
	}
	void FixedUpdate()
	{
		currentPos = Vector3.Lerp(startPoint.position + transform.localPosition, endPoint.position + transform.localPosition,
			Mathf.Cos(Time.time / travelTime * Mathf.PI * 2) * -.5f + .5f);
		rb.MovePosition(currentPos + startPosition);
	}
	

}
