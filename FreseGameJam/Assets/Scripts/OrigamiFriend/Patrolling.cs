using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Patrolling : MonoBehaviour
{
    // public variables:
    public Transform[] arrayOfCheckpoints;

    // private variables:
    private NavMeshAgent _navMeshAgent;

    private bool _reachedTarget = false;
    private bool _waitingForPlayer = false;

    [SerializeField] GameObject _turnOffElements;
    [SerializeField] GameObject _turnOnElements;

    [Header("Alternative ending for regular runs:")]
    [Tooltip("Put the end of game collider/trigger (box) in here. This will only be turnt on when the ship has actually arrived.")]
    [SerializeField] BoxCollider _endOfGameCollider;
    [SerializeField] GameObject _regularEndOfGameTrigger;

    private float _defaultSpeed;

    // save current position of boat:
    private int _destPoint = 0;
    private int _boatPosition = 0;

    void Start()
    {
        // if boat had reached final destination in a prior run, do not update the target position any further:
        if (PlayerPrefs.GetInt("_reachedEndOfTravel") == 0)
        {
            _boatPosition = PlayerPrefs.GetInt("_boatPosition"); // update _boatPosition:
            if (PlayerPrefs.GetInt("_boatPosition") != 0)
            {
                _destPoint = PlayerPrefs.GetInt("_boatPosition") + 1;
                this.transform.position = arrayOfCheckpoints[PlayerPrefs.GetInt("_boatPosition")].transform.position;
            }

            _navMeshAgent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            _navMeshAgent.autoBraking = false;
            _defaultSpeed = _navMeshAgent.speed;
            GotoNextPoint();
        }else
        {
            RelocateBoat(); 
        }
    }

    void GotoNextPoint()
    {

        if (_reachedTarget == false && !_waitingForPlayer)
        {
            // Returns if no points have been set up
            if (arrayOfCheckpoints.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            _navMeshAgent.destination = arrayOfCheckpoints[_destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            _destPoint = (_destPoint + 1) % arrayOfCheckpoints.Length;

            // save boat position:
            SaveBoatPosition();
        }else
        {
            _navMeshAgent.enabled = false;
        }
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_reachedTarget && !_waitingForPlayer)
        {
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
        else
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
        if (other.CompareTag("Destination") && !_reachedTarget)
        {
            RelocateBoat();
        }
    }
    void RelocateBoat()
    {
        // get rid of bug about having waypoints out of array:
        PlayerPrefs.SetInt("_reachedEndOfTravel", 1);
        
        _reachedTarget = true;
        
        _turnOnElements.SetActive(true);
        _turnOffElements.SetActive(false);

        // If we want the game to only end when the boat arrives at the end, then we have to disable the
        // Trigger on the BreakingGround object and turn off the "turnON" objects of the friend in sailboat
        // so they are off by default. Currently this line is useless, as i turnt the trigger on regardless! -F
        _endOfGameCollider.enabled = true; 

        //_regularEndOfGameTrigger.SetActive(true); // made this separate from turnOnElements, as this is a logic/trigger element.
    }

    // polish: this makes friend wait when touching trigger at a given location, needs a wake-up-trigger to call "StopWaiting()".
    public void WaitForPlayer()
    {
        _navMeshAgent.speed = 0;
    }
    public void StopWaiting()
    {
        _navMeshAgent.speed = _defaultSpeed;
    }
}
