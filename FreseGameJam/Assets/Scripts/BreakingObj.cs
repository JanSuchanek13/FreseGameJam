using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObj : MonoBehaviour
{
    public bool onlyCapricorn;
    public bool breakingChildren;
    public float timeTillBreak;
    public bool reset;

    Vector3 resetPos;
    Vector3 resetRot;
    List<Vector3> resetPosChildren;
    List<Vector3> resetRotChildren;

    private void Start()
    {
        if (reset)
        {
            Debug.Log("1");
            if (breakingChildren)
            {
                foreach (Transform child in transform)
                {
                    resetPosChildren.Add(child.GetComponent<Transform>().position);
                    resetRotChildren.Add(child.GetComponent<Transform>().rotation.eulerAngles);
                }
            }
            else
            {
                resetPos = transform.position;
                resetRot = transform.rotation.eulerAngles;
                Debug.Log(transform.position);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        
        if (onlyCapricorn)
        {
            
            if (collision.gameObject.tag == "Player")
            {
                
                if (collision.gameObject.GetComponent<StateController>().capricorn)
                {
                    
                    Invoke("Break", timeTillBreak);
                }
            }
        }
        else
        {
            
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

        if (reset)
        {
            Debug.Log("2");
            Invoke("Reset", 5);
        }
    }

    private void Reset()
    {
        if (breakingChildren)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Rigidbody>().isKinematic = true;
                int i = 0;
                child.transform.position = resetPosChildren[i];
                child.transform.rotation = Quaternion.Euler(resetPosChildren[i]);
                i++;
            }
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("3");
            Debug.Log(resetPos);
            transform.position = resetPos;
            transform.rotation = Quaternion.Euler(resetRot);
        }
    }
}
