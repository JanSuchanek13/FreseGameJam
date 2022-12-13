using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpeedUpNavMeshAgent : MonoBehaviour
{

    private IEnumerator coroutine;
    private float speed;


    private void Start()
    {
        speed = gameObject.GetComponent<NavMeshAgent>().speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(SpeedUp());
        }
            
    }

    public void StopForDead()
    {
        StartCoroutine(SlowDown());
    }

    private IEnumerator SpeedUp()
    {
        gameObject.GetComponent<NavMeshAgent>().speed = 10;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<NavMeshAgent>().speed = speed;
    }

    public IEnumerator SlowDown()
    {
        gameObject.GetComponent<NavMeshAgent>().speed = 0.01f;
        yield return new WaitForSeconds(5f);
        gameObject.GetComponent<NavMeshAgent>().speed = speed;
    }
}
