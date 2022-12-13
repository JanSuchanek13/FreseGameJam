using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverer : MonoBehaviour
{
    [SerializeField] float hoverHeight = 2f;
    [SerializeField] float speed = 1f;


    private Vector3 startPosition; 

   
    void Start()
    { 
        startPosition = transform.position; 
    }

    void Update()
    {
        Vector3 tp = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * hoverHeight * Mathf.PingPong(Time.time, 1f) * speed;
        transform.position = targetPosition;
    }

  
    public void StartHovering()
    {
        Debug.Log("i should start hovering");
        this.enabled = true;
    }

}
