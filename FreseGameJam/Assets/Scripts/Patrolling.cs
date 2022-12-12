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
    // save current position of boat:
    int _boatPosition = 0;

    void Start()
    {
        // if boat had reached final destination in a prior run, do not update the target position any further:
        if (PlayerPrefs.GetInt("_reachedEndOfTravel") == 0)
        {
            _boatPosition = PlayerPrefs.GetInt("_boatPosition"); // update _boatPosition:
            if (PlayerPrefs.GetInt("_boatPosition") != 0)
            {
                destPoint = PlayerPrefs.GetInt("_boatPosition") + 1;
                this.transform.position = points[PlayerPrefs.GetInt("_boatPosition")].transform.position;
            }

            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            _defaultSpeed = agent.speed;
            GotoNextPoint();
        }else
        {
            ReachedTarget = true;   
        }
    }

    // save if cutscene has played:
    // _cutSceneHasAlreadyPlayed++;
    //        PlayerPrefs.SetInt("_cutSceneHasAlreadyPlayed", _cutSceneHasAlreadyPlayed);

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

            // save boat position:
            SaveBoatPosition();
        }else
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
    void SaveBoatPosition()
    {
        PlayerPrefs.SetInt("_boatPosition", _boatPosition);
        _boatPosition++;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destination") && !ReachedTarget)
        {
            ReachedTarget = true;

            // get rid of bug about having waypoints out of array:
            PlayerPrefs.SetInt("_reachedEndOfTravel", 1);

            //Debug.Log("I reached my destination!!!!");
            //_turnOffElements.SetActive(false);
            _turnOnElements.SetActive(true);
            agent.isStopped = true;
            _turnOffElements.SetActive(false);
        }
    }


    // pollish: this makes friend wait when touching trigger at a given location, needs a wake-up-trigger to call "StopWaiting()".
    public void WaitForPlayer()
    {
        agent.speed = 0;
        //Debug.Log("now waiting for player");
    }
    public void StopWaiting()
    {
        agent.speed = _defaultSpeed;
        //Debug.Log("continuing travel");
    }
}
