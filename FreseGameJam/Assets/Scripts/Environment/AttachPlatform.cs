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
	bool isSubmerged = true;

	public Transform startPoint;
	public Transform endPoint;
	public float travelTime;
	private Rigidbody rb;
	private Vector3 currentPos;
	CharacterController cc;
	Vector2 offset;
	bool firstContact = true;

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	private float obstacleAvoidance;
	private ObstacleAvoidanceType type;
	private float height;
	private float speed;

	public GameObject[] Fire;

	//Boxcast
	float m_MaxDistance;
	float m_Speed;
	float m_Scale = 1;
	bool m_HitDetect;
	Collider m_Collider;
	RaycastHit m_Hit;
	Vector3 oldPos;
	


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

		//Boxcast
		//Choose the distance the Box can reach to
		m_MaxDistance = 1.0f;
		m_Speed = 20.0f;
		m_Collider = GetComponent<Collider>();
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
				isSubmerged = true;
				
				//gameObject.transform.GetChild(0).position += new Vector3(0, -0.6f, 0);
				
				GetComponent<NavMeshAgent>().baseOffset = -1.0f;
				//GetComponent<NavMeshAgent>().areaMask = NavMesh.GetAreaFromName("Walkable");

				//gameObject.GetComponent<MeshRenderer>().enabled = false;
				//gameObject.GetComponent<BoxCollider>().enabled = false;
				//Debug.Log("ende");
				foreach (GameObject i in Fire)
				{
					i.SetActive(true);
				}
				StartCoroutine(submerge());
				agent.radius = .01f;
				//agent.radius = .25f;
				agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
				agent.height = 0;
				agent.speed = resetSpeed;
				agent.avoidancePriority = 60;
				GetComponent<BoxCollider>().enabled = false;
			}
			else if (destPoint == 2)
			{
				GetComponent<NavMeshAgent>().baseOffset = .25f;
				//GetComponent<NavMeshAgent>().areaMask = NavMesh.GetAreaFromName("Everything");


				isSubmerged = false;
				//gameObject.transform.GetChild(0).position += new Vector3(0, +0.6f, 0);
				agent.radius = obstacleAvoidance;
				agent.obstacleAvoidanceType = type;
				agent.height = height;
				agent.speed = speed;
				agent.avoidancePriority = 50;
				GetComponent<BoxCollider>().enabled = true;
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
			if (!agent.pathPending && agent.remainingDistance < 4.0f) // was 2!
				GotoNextPoint();
		}


		//boxCast
		DrawBoxCastBox(m_Collider.bounds.center + new Vector3(0, -1f, 0), transform.localScale * m_Scale, transform.rotation, transform.up, m_MaxDistance, Color.red);
		m_HitDetect = Physics.BoxCast(m_Collider.bounds.center + new Vector3(0,-1f,0), transform.localScale * m_Scale, transform.up, out m_Hit, transform.rotation, m_MaxDistance);
		if (m_HitDetect && m_Hit.transform.gameObject.GetComponent<CharacterController>() != null && !isSubmerged)
		{
			/*
			//Debug.Log("Hit : " + m_Hit.collider.name);
			Vector3 direction = transform.position - oldPos;
			m_Hit.transform.gameObject.GetComponent<CharacterController>().Move(direction.normalized / 50);
			oldPos = transform.position;
			if (!isStanding)
			{
				//Output the name of the Collider your Box hit
				//Debug.Log("Hit : " + m_Hit.collider.name);



				if (m_Hit.transform.tag == "Player")
				{
					isStanding = true;


					//m_Hit.transform.gameObject.transform.SetParent(gameObject.transform, true);
					StartCoroutine(moveCountDown());
				}
			}
			*/ 
			//New System doesnt work after you die
			if (m_Hit.transform.gameObject.GetComponent<InputHandler>().moveValue.magnitude == 0)
            {
				Debug.Log("Hit : " + m_Hit.collider.name);
                if (firstContact)
                {
					offset.x = m_Hit.transform.position.x - this.transform.position.x;
					offset.y = m_Hit.transform.position.z - this.transform.position.z;
					firstContact = false;
				}
				m_Hit.transform.position = new Vector3(this.transform.position.x + offset.x, m_Hit.transform.position.y, this.transform.position.z + offset.y);
				
				

			}
            else
            {
				offset.x = m_Hit.transform.position.x - this.transform.position.x;
				offset.y = m_Hit.transform.position.z - this.transform.position.z;
				//Debug.Log("Hit : " + m_Hit.collider.name);
				Vector3 direction = transform.position - oldPos;
				m_Hit.transform.gameObject.GetComponent<CharacterController>().Move(direction.normalized / 50);
				oldPos = transform.position;
				if (!isStanding)
				{
					//Output the name of the Collider your Box hit
					//Debug.Log("Hit : " + m_Hit.collider.name);



					if (m_Hit.transform.tag == "Player")
					{
						isStanding = true;


						//m_Hit.transform.gameObject.transform.SetParent(gameObject.transform, true);
						StartCoroutine(moveCountDown());
					}
				}
			}
			
			
		}
        else
        {
			isStanding = false;
			if(m_Hit.transform != null)
            {
				//m_Hit.transform.parent = null;
			}
			StartCoroutine(clearFire());
			StopCoroutine(moveCountDown());
		}

	}

	/*
	private void OnTriggerEnter(Collider other)
	{

        if (MagmaJump)
        {
			if (other.tag == "Player")
			{
				Debug.Log("enter");

				isStanding = true;
				StartCoroutine(moveCountDown());
				foreach (GameObject i in Fire)
				{

					i.SetActive(true);
				}

				cc = other.GetComponent<CharacterController>();

				
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
			Debug.Log("exit");

			other.transform.parent = null;
			//StartCoroutine(clearFire());
			foreach (GameObject i in Fire)
			{

				i.SetActive(false);
			}
		}
			

		
	}
	*/

	private IEnumerator clearFire()	//made Coroutine so that the Frame time for enable/disable fire is the same
    {
		
		foreach (GameObject i in Fire)
		{

			i.SetActive(false);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator moveCountDown()
    {
		
		foreach (GameObject i in Fire)
		{
			yield return new WaitForSeconds(0.75f);
            if (!isStanding)
            {
				break;
            }
			i.SetActive(true);
		}
		
		//yield return new WaitForSeconds(countdown);
		if (isStanding)
        {
			GetComponent<Collider>().enabled = false;
			gameObject.transform.GetChild(0).position += new Vector3(0, -0.6f, 0);
			StartCoroutine(clearFire());
			GetComponent<Collider>().enabled = false;
			yield return new WaitForSeconds(countdown);
			gameObject.transform.GetChild(0).position += new Vector3(0, +0.6f, 0);
			GetComponent<Collider>().enabled = true;
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

	/*
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		//Check if there has been a hit yet
		if (m_HitDetect)
		{
			//Draw a Ray forward from GameObject toward the hit
			Gizmos.DrawRay(transform.position, transform.up * m_Hit.distance);
			//Draw a cube that extends to where the hit exists
			Gizmos.DrawWireCube(transform.position + transform.up * m_Hit.distance*2, transform.localScale * m_Scale);
		}
		//If there hasn't been a hit yet, draw the ray at the maximum distance
		else
		{
			//Draw a Ray forward from GameObject toward the maximum distance
			Gizmos.DrawRay(transform.position, transform.up * m_MaxDistance);
			//Draw a cube at the maximum distance
			Gizmos.DrawWireCube(transform.position + transform.up * m_MaxDistance*2, transform.localScale * m_Scale);
		}
	}
	*/
	public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
	{
		direction.Normalize();
		Box bottomBox = new Box(origin, halfExtents, orientation);
		Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

		Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
		Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
		Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
		Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
		Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
		Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
		Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
		Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

		DrawBox(bottomBox, color);
		DrawBox(topBox, color);
	}

	public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
	{
		DrawBox(new Box(origin, halfExtents, orientation), color);
	}
	public static void DrawBox(Box box, Color color)
	{
		Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
		Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
		Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
		Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

		Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
		Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
		Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
		Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

		Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
		Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
		Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
		Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
	}

	public struct Box
	{
		public Vector3 localFrontTopLeft { get; private set; }
		public Vector3 localFrontTopRight { get; private set; }
		public Vector3 localFrontBottomLeft { get; private set; }
		public Vector3 localFrontBottomRight { get; private set; }
		public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
		public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
		public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
		public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

		public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
		public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
		public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
		public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
		public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
		public Vector3 backTopRight { get { return localBackTopRight + origin; } }
		public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
		public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

		public Vector3 origin { get; private set; }

		public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
		{
			Rotate(orientation);
		}
		public Box(Vector3 origin, Vector3 halfExtents)
		{
			this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
			this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
			this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
			this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

			this.origin = origin;
		}


		public void Rotate(Quaternion orientation)
		{
			localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
			localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
			localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
			localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
		}
	}


	//This should work for all cast types
	static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
	{
		return origin + (direction.normalized * hitInfoDistance);
	}

	static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
	{
		Vector3 direction = point - pivot;
		return pivot + rotation * direction;
	}
}

