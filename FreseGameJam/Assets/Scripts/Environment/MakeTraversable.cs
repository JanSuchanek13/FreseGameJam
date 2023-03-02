using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTraversable : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("CheckForMovement"); // allow debris to fly around, and turn it off when it stops moving

        Invoke("StopThisObject", 3f); // failsafe, in case debris never stops moving
    }

    IEnumerator CheckForMovement()
    {
        Vector3 _lastCheckedVelocity = GetComponent<Rigidbody>().velocity;

        yield return new WaitForSeconds(0.1f);

        if( _lastCheckedVelocity.magnitude < 0.0f)
        {
            StopThisObject();
        }

        else // check if still moving:
        {
            StartCoroutine("CheckForMovement");
        }
    }

    private void StopThisObject()
    {
        GetComponent<Rigidbody>().isKinematic = true; // no more moving/falling
        GetComponent<Collider>().enabled = false; // no more collision
    }
}
