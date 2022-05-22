using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttachPlatform : MonoBehaviour
{
	public bool MagmaJump = true;

	private IEnumerator coroutine;
	public int countdown = 3;
	public float resetSpeed = 8;
	bool isStanding;

	public Transform startPoint;
	public Transform endPoint;
	public float travelTime;
	private Rigidbody rb;
	private Vector3 currentPos;
	CharacterController cc;

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	private float obstacleAvoidance;
	private ObstacleAvoidanceType type;
	private float height;
	private float speed;

	public GameObject[] Fire;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

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

        if (MagmaJump)
        {
			GotoNextPoint();
		}
		
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

		if (MagmaJump)
        {
			if (destPoint == 1)
			{
				gameObject.transform.GetChild(0).position += new Vector3(0, -0.6f, 0);
				//gameObject.GetComponent<MeshRenderer>().enabled = false;
				//gameObject.GetComponent<BoxCollider>().enabled = false;
				//Debug.Log("ende");
				foreach (GameObject i in Fire)
				{
					i.SetActive(true);
				}
				StartCoroutine(submerge());
				agent.radius = .01f;
				agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
				agent.height = 0;
				agent.speed = resetSpeed;
				agent.avoidancePriority = 60;
			}
			else if (destPoint == 2)
			{
				gameObject.transform.GetChild(0).position += new Vector3(0, +0.6f, 0);
				agent.radius = obstacleAvoidance;
				agent.obstacleAvoidanceType = type;
				agent.height = height;
				agent.speed = speed;
				agent.avoidancePriority = 50;
			}
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
		if (MagmaJump)
        {
			if (!agent.pathPending && agent.remainingDistance < 1f)
				GotoNextPoint();
		}
		



	}
	private void OnTriggerEnter(Collider other)
	{

        if (MagmaJump)
        {
			if (other.tag == "Player")
			{
				isStanding = true;
				StartCoroutine(moveCountDown());

				cc = other.GetComponent<CharacterController>();

				foreach (GameObject i in Fire)
				{
					i.SetActive(true);
				}
			}
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
		isStanding = false;

		if (other.tag == "Player")
        {
			other.transform.parent = null;
			foreach (GameObject i in Fire)
			{
				i.SetActive(false);
			}
		}
			

		
	}

	private IEnumerator moveCountDown()
    {


		yield return new WaitForSeconds(countdown);
		if (isStanding)
        {
			gameObject.transform.GetChild(0).position += new Vector3(0, -0.6f, 0);
			yield return new WaitForSeconds(countdown);
			gameObject.transform.GetChild(0).position += new Vector3(0, +0.6f, 0);
		}
	}

	private IEnumerator submerge()
    {
		yield return new WaitForSeconds(1f);
		foreach (GameObject i in Fire)
		{
			i.SetActive(false);
		}
	}
}
