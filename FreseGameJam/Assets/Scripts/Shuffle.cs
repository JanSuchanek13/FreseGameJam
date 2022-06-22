using UnityEngine;

public class Shuffle : MonoBehaviour
{
    public float shuffleDistance = 2f;
    public float speed = 1f;
    [SerializeField] bool useCustomDirection = false;
    [SerializeField] GameObject patrolPoint;
    private Vector3 _startPosition;
    private Vector3 _patrolPointPosition;
    private Vector3 _positionDifference;
    private Vector3 _direction;


    void Start()
    {
        _startPosition = transform.position;

        if (useCustomDirection)
        {
            patrolPoint.transform.parent = null;
            _patrolPointPosition = patrolPoint.transform.position;
            DetermineDirection();
        }
    }
    void DetermineDirection()
    {
        _positionDifference = _startPosition - _patrolPointPosition;
        _direction = new Vector3(_positionDifference.x, 0, _positionDifference.z);
    }

    void Update()
    {
        if (!useCustomDirection)
        {
            Vector3 targetPosition = _startPosition + Vector3.forward * Mathf.PingPong(Time.time, shuffleDistance) * speed;
            transform.position = targetPosition;
        }else
        {
            Vector3 targetPosition = _startPosition - (_direction * Mathf.PingPong(Time.time, shuffleDistance) * (speed/12));
            transform.position = targetPosition;
        }
    }
}



// safety:

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    [SerializeField] float shuffleDistance = 2f;
    [SerializeField] float speed = 1f;
    float _patrolSpeed; // seperate from regular speed to adjust for different movement mechanic
    float _distanceBetweenPatrolPointAndStartPosition;
    [SerializeField] bool useCustomDirection = false;
    [SerializeField] GameObject patrolPoint;
    //GameObject _thisObject;

    private Vector3 _startPosition;
    private Vector3 _patrolPointPosition;
    private Vector3 _positionDifference;
    private Vector3 _direction;


    void Start()
    {
        _startPosition = transform.position;

        if (useCustomDirection)
        {
            /*Vector3 Destination = new Vector3(0, 0, 100);
            Vector3 Traveller = new Vector3(0, 0, 0);
            float step = 2.5f;
            while (Traveller != Destination)
            {
                Traveller = Vector3.MoveTowards(Traveller, Destination, Step);
            }

//_thisObject = this.transform.position;
//_patrolPointPosition = patrolPoint.transform.position;
//_patrolSpeed = speed / 12; // 10 seems to work well
patrolPoint.transform.parent = null;
_patrolPointPosition = patrolPoint.transform.position;

//Debug.Log("my Pos: " + this.transform.position);
//Debug.Log("target Pos: " + _patrolPointPosition);

//DetermineDistance();
DetermineDirection();
        }
    }

    /*private void FixedUpdate()
    {
        while (usePatrolPoints && this.transform.position != _patrolPointPosition)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _patrolPointPosition, .000001f);
            //this.transform.position = Vector3.MoveTowards(_startPosition, _patrolPointPosition, .0001f);
        }
    }


    void DetermineSpeed() // currently not used
{
    //_patrolSpeed = speed / (2 * _distanceBetweenPatrolPointAndStartPosition);
    _patrolSpeed = _distanceBetweenPatrolPointAndStartPosition / 2;
}
void DetermineDistance()
{
    // normalize values:
    _positionDifference = _startPosition - _patrolPointPosition;
    if (_positionDifference.x < 0)
    {
        _positionDifference.x *= -1;
    }
    if (_positionDifference.z < 0)
    {
        _positionDifference.z *= -1;
    }
    Debug.Log(_positionDifference);
    _distanceBetweenPatrolPointAndStartPosition = (_positionDifference.x + _positionDifference.z) / 2;
    Debug.Log(_distanceBetweenPatrolPointAndStartPosition);
    //DetermineDirection();
    //DetermineSpeed(); //called here as the distance had to be determined first!
}
void DetermineDirection()
{
    _positionDifference = _startPosition - _patrolPointPosition;
    _direction = new Vector3(_positionDifference.x, 0, _positionDifference.z);
    //DetermineDistance();
}

void Update()
{
    if (!useCustomDirection)
    {
        Vector3 targetPosition = _startPosition + Vector3.forward * Mathf.PingPong(Time.time, shuffleDistance) * speed;
        transform.position = targetPosition;
    }
    else
    {
        //Vector3 targetPosition = _startPosition + patrolPoint.transform.position * Mathf.PingPong(Time.time, _distanceBetweenPatrolPointAndStartPosition) * _patrolSpeed; //startPosition + Vector3.forward * Mathf.PingPong(Time.time, shuffleDistance) * speed;
        //Vector3 targetPosition = _startPosition - new Vector3(_positionDifference.x, 0, _positionDifference.z) * Mathf.PingPong(Time.time, shuffleDistance) * speed;
        //Vector3 targetPosition = _startPosition - (_direction * Mathf.PingPong(Time.time, shuffleDistance) * speed);
        //Vector3 targetPosition = _startPosition - (_direction * Mathf.PingPong(Time.time, _distanceBetweenPatrolPointAndStartPosition) * speed * Time.deltaTime);
        Vector3 targetPosition = _startPosition - (_direction * Mathf.PingPong(Time.time, shuffleDistance) * (speed / 12));
        transform.position = targetPosition;
    }
    /*while (this.transform.position != _patrolPointPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, _patrolPointPosition, .000001f * Time.deltaTime);
        //this.transform.position = Vector3.MoveTowards(_startPosition, _patrolPointPosition, .0001f);
    }
    // Move our position a step closer to the target.
    //var step = speed * Time.deltaTime; // calculate distance to move
   // transform.position = Vector3.MoveTowards(transform.position, target.position, step);

    // Check if the position of the cube and sphere are approximately equal.
    if (Vector3.Distance(transform.position, _patrolPointPosition) < 0.001f)
    {
        // Swap the position of the cylinder.
        _patrolPointPosition *= -1.0f;
    }

}


    /*public void StartHovering()
    {
        Debug.Log("i should start hovering");
        this.enabled = true;
    }

}*/
