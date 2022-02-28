using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Patrolling : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    bool ReachedTarget = false;



    void Start()
    {
       agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {

        if (ReachedTarget == false)

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
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destination"))
        {
            ReachedTarget = true;
            Debug.Log("I reached my destination!!!!");
        }

    }



}
