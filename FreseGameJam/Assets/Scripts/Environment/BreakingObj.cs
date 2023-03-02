using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObj : MonoBehaviour
{
    [Header("Breaking Objects Settings:")]
    public GameObject origamiFriend;
    public bool levelEnd;
    public bool onlyCapricorn;
    public bool breakingChildren;
    public float timeTillBreak;
    [SerializeField] float _timeTillReset = 5.0f; // F. added this to determine resetTime
    public bool reset;

    [SerializeField] bool _turnOffCollidersAfterBreak = false; // making this optional allows to have breaking objects which become part of the traversable environment

    // local variables:
    Vector3 resetPos;
    Vector3 resetRot;
    List<Vector3> resetPosChildren = new List<Vector3>();
    List<Vector3> resetRotChildren = new List<Vector3>();

    private void Start()
    {
        if (reset)
        {
            if (breakingChildren)
            {
                foreach (Transform child in transform)
                {
                    resetPosChildren.Add(child.gameObject.GetComponent<Transform>().position);
                    resetRotChildren.Add(child.gameObject.GetComponent<Transform>().rotation.eulerAngles);
                }
            }else
            {
                resetPos = transform.position;
                resetRot = transform.rotation.eulerAngles;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (onlyCapricorn)
        {
            if (collision.gameObject.tag == "Player")
            {  
                if (collision.gameObject.GetComponent<StateController>().capricorn && collision.gameObject.GetComponent<ThirdPersonMovement>().inDash)
                { 
                    Invoke("Break", timeTillBreak);
                }
            }
        } else
        {
            if (collision.gameObject.tag == "Player")
            {
                Invoke("Break", timeTillBreak);
                
                if (levelEnd)
                {
                    collision.gameObject.GetComponent<ThirdPersonMovement>().forcedFalling = true;
                    origamiFriend.GetComponent<Animator>().SetBool("Falling", true);
                }
                    
            }
        }
    }

    void Break()
    {
        if (breakingChildren)
        {
            foreach(Transform child in transform)
            {
                if (child.GetComponent<Rigidbody>()) // this allows to search through children who may NOT have a Rigidbody component
                {
                    child.GetComponent<Rigidbody>().isKinematic = false;

                    if (_turnOffCollidersAfterBreak)
                    {
                        child.gameObject.AddComponent<MakeTraversable>(); // this will take care of making fragments traversable
                    }
                }

                // this will also loop through all children of children: Needed to break walls of new composite-breakable walls
                foreach(Transform _grandChild in child)
                {
                    if (_grandChild.GetComponent<Rigidbody>())
                    {
                        _grandChild.GetComponent<Rigidbody>().isKinematic = false;
                        
                        if (_turnOffCollidersAfterBreak)
                        {
                            _grandChild.gameObject.AddComponent<MakeTraversable>(); // this will take care of making fragments traversable
                        }
                    }
                }
            }
        }else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }

        if (reset)
        {
            Invoke("Reset", _timeTillReset);
        }
    }
    /* // this is an inferior alternative path to make debris traversable
    IEnumerator MakeTraversable(Transform _objectToMakeTraversable)
    {
        yield return new WaitForSeconds(5);

        _objectToMakeTraversable.GetComponent<Rigidbody>().isKinematic = true;
    }*/

    private void Reset()
    {
        if (breakingChildren)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Rigidbody>()) // this allows to search through children who may NOT have a Rigidbody component
                {
                    child.GetComponent<Rigidbody>().isKinematic = true;
                    int i = 0;
                    child.transform.position = resetPosChildren[i];
                    child.transform.rotation = Quaternion.Euler(resetPosChildren[i]);
                    i++;
                }
            }
        }else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = resetPos;
            transform.rotation = Quaternion.Euler(resetRot);
        }
    }
}
