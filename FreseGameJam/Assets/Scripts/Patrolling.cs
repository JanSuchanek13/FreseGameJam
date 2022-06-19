using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Patrolling : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    bool ReachedTarget = false;
    [SerializeField] GameObject _turnOffElements;
    [SerializeField] GameObject _turnOnElements;
    private bool _waitingForPlayer = false;
    private float _defaultSpeed;




    void Start()
    {
       agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        _defaultSpeed = agent.speed;
        GotoNextPoint();
    }


    void GotoNextPoint()
    {

        if (ReachedTarget == false && !_waitingForPlayer)

        {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }
      else
        {
            agent.enabled = false;
        }
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!ReachedTarget && !_waitingForPlayer)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }else
        {
            enabled = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destination") && !ReachedTarget)
        {
            ReachedTarget = true;
            Debug.Log("I reached my destination!!!!");
            _turnOffElements.SetActive(false);
            _turnOnElements.SetActive(true);
            agent.isStopped = true;
        }
    }


    // polish: this makes friend wait when touching trigger at a given location, needs a wake-up-trigger to call "StopWaiting()".
    public void WaitForPlayer()
    {
        agent.speed = 0;
        Debug.Log("now waiting for player");
    }
    public void StopWaiting()
    {
        agent.speed = _defaultSpeed;
        Debug.Log("continuing travel");
    }
}
