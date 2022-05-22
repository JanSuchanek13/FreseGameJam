using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    [SerializeField] float shuffleDistance = 2f;
    [SerializeField] float speed = 1f;
    //[SerializeField] float minimumSpeed = .3f;
    //[SerializeField] float maximumSpeed = 3f;


    //[SerializeField] float hoverHeight = .3f;
    //[SerializeField] float hoverSpeed = 1f;

    private Vector3 startPosition;


    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // shuffle:
        //Vector3 targetPosition = startPosition + Vector3.forward * shuffleDistance * Mathf.PingPong(Time.time, 1f) * speed;
        //transform.position = targetPosition;
        
        //float speed = Random.Range(minimumSpeed, maximumSpeed);
        Vector3 targetPosition = startPosition + Vector3.forward * Mathf.PingPong(Time.time, shuffleDistance) * speed;
        transform.position = targetPosition;

        //Vector3 targetPosition = startPosition + new Vector3(0, (1 * hoverHeight * Mathf.PingPong(Time.time, 1f) * hoverSpeed), (1 * shuffleDistance * Mathf.PingPong(Time.time, 1f) * speed));
        //transform.position = targetPosition;

        // hover:
        /*float targetHover = float.RandomRange(0, 4);
            Vector3 targetHover = transform.position + Vector3.up * hoverHeight * Mathf.PingPong(Time.time, 1f) * speed;
            transform.position = targetHover;*/
    }


    /*public void StartHovering()
    {
        Debug.Log("i should start hovering");
        this.enabled = true;
    }*/

}
