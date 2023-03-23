using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObj : MonoBehaviour
{
    [Header("Breaking Objects Settings:")] // warum muss irgednwas hiervon public sein?
    //public GameObject origamiFriend;
    public bool levelEnd;
    public bool onlyCapricorn;
    public bool breakingChildren;
    public float timeTillBreak;
    public bool reset; 

    // local variables:
    [SerializeField] bool _turnOffCollidersAfterBreak = false; // making this optional allows to have breaking objects which become part of the traversable environment
    [SerializeField] float _timeTillReset = 5.0f;

    Vector3 _resetPos;
    Vector3 _resetRot;

    List<Vector3> _resetPosListOfChildren = new List<Vector3>();
    List<Vector3> _resetRotListOfChildren = new List<Vector3>();

    private void Start()
    {
        if (reset)
        {
            // prevent resettable objects from being destructed when falling through ground, water, lava etc:
            transform.gameObject.tag = "Indestructable";

            if (breakingChildren)
            {
                foreach (Transform child in transform)
                {
                    _resetPosListOfChildren.Add(child.gameObject.GetComponent<Transform>().position);
                    _resetRotListOfChildren.Add(child.gameObject.GetComponent<Transform>().rotation.eulerAngles);
                }
            }else
            {
                _resetPos = transform.position;
                _resetRot = transform.rotation.eulerAngles;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject _player = collision.gameObject;

            if (onlyCapricorn)
            {
                if (_player.GetComponent<StateController>().capricorn && _player.GetComponent<ThirdPersonMovement>().inDash)
                {
                    Invoke("Break", timeTillBreak);
                }
            }
            else
            {
                Invoke("Break", timeTillBreak);
                
                if (levelEnd)
                {
                    _player.GetComponent<ThirdPersonMovement>().forcedFalling = true;
                    _player.GetComponent<Animator>().SetBool("Falling", true);
                }
            }
        }


        // old:
        /*
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
                    //origamiFriend.GetComponent<Animator>().SetBool("Falling", true);
                    collision.gameObject.GetComponent<Animator>().SetBool("Falling", true);
                }
                    
            }
        }*/
    }

    void Break()
    {
        if (!reset)
        {
            GetComponent<Collider>().enabled = false; // turn off trigger so it no longer triggers sound or particles
        }

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
                    child.transform.position = _resetPosListOfChildren[i];
                    child.transform.rotation = Quaternion.Euler(_resetPosListOfChildren[i]);
                    i++;
                }
            }
        }else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = _resetPos;
            transform.rotation = Quaternion.Euler(_resetRot);
        }
    }
}
