using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObj : MonoBehaviour
{
    public bool onlyCapricorn;
    public bool breakingChildren;
    public float timeTillBreak;

    void OnTriggerEnter(Collider collision)
    {
        if (onlyCapricorn)
        {
            Debug.Log("1");
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("2");
                if (collision.gameObject.GetComponent<StateController>().capricorn)
                {
                    Debug.Log("3");
                    Invoke("Break", timeTillBreak);
                }
            }
        }
        else
        {
            Debug.Log("1");
            if (collision.gameObject.tag == "Player")
            {
                Invoke("Break", timeTillBreak);
            }
        }

        
    }

    void Break()
    {
        if (breakingChildren)
        {
            foreach(Transform child in transform)
            {
                child.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
