using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGravityOnTouch : MonoBehaviour
{
    //#solean

    /*private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("somethin ran into me");


            GetComponent<Rigidbody>().isKinematic = false;

            Debug.Log("I shouldve turnt off my kinematic rigidbody now..");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("somethin ran into me");


            GetComponent<Rigidbody>().isKinematic = false;

            //Debug.Log("I shouldve turnt off my kinematic rigidbody now..");
        }
    }
}
