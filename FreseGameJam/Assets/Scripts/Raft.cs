using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Raft : MonoBehaviour
{
	public bool started = false;


	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	private float obstacleAvoidance;
	private ObstacleAvoidanceType type;
	private float height;
	private float speed;




	private void Start()
	{


		agent = GetComponent<NavMeshAgent>();

		obstacleAvoidance = agent.radius;
		type = agent.obstacleAvoidanceType;
		height = agent.height;
		speed = agent.speed;
		//Debug.Log(obstacleAvoidance);


		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;

		

	}

	void GotoNextPoint()
	{
		// Returns if no points have been set up
		if (points.Length == 0)
		{

			return;
		}

		



			// Set the agent to go to the currently selected destination.
			agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;

		// if Destination reached stop Movement
		if (destPoint == 1 && started)
		{
			Invoke("StopMoving", 1);

		}

	}

	private void StopMoving()
    {
		if (destPoint == 1 && started)
		{
			GetComponent<NavMeshAgent>().speed = 0;

		}
		
	}

	void FixedUpdate()
	{
		/*
		currentPos = Vector3.Lerp(startPoint.position, endPoint.position,
			Mathf.Cos(Time.time / travelTime * Mathf.PI * 2) * -.5f + .5f);
		rb.MovePosition(currentPos);
		*/





		// Choose the next destination point when the agent gets
		// close to the current one.
		
		if (started)
		{
			if (!agent.pathPending && agent.remainingDistance < 2f)
				GotoNextPoint();
		}



	}
	private void OnTriggerEnter(Collider other)
	{
		if (started == false && other.tag == "Player")
        {
			started = true;
			GotoNextPoint();
		}
		
		



	}
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			//cc.Move(rb.velocity * Time.deltaTime);
			//cc.Move(transform.position.normalized * Time.deltaTime*3.5f );
			other.transform.parent = transform;
		}

	}
	private void OnTriggerExit(Collider other)
	{
		
		if (other.tag == "Player")
		{
			other.transform.parent = null;
			
		}



	}

	
}
