using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingOnAxis : MonoBehaviour
{
    [SerializeField] float Lenght = 2f;
    //[SerializeField] float speed = 1f;
    [SerializeField] Vector3 axis;

    public float speedmin = 10;
    public float speedmax = 10;
    float speed;


    private Vector3 startPosition;


    void Start()
    {
        startPosition = transform.localPosition;
        speed = Random.Range(speedmin, speedmax);
        speed = speed / 10;
        Lenght = Lenght / 10;
    }

    void Update()
    {
        Vector3 tp = transform.position;
        Vector3 targetPosition = startPosition + axis * Lenght * Mathf.PingPong(Time.time, 1f) * speed;
        gameObject.transform.localPosition = targetPosition;
    }


    public void StartHovering()
    {
        Debug.Log("i should start hovering");
        this.enabled = true;
    }

}
