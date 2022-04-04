using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapricornAttack : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("1");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("2");
            if (collision.gameObject.GetComponent<StateController>().capricorn)
            {
                Debug.Log("3");
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    
}
