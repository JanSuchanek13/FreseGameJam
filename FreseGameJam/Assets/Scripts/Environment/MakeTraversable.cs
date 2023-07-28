using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTraversable : MonoBehaviour
{
    public bool hasSettled = false;
    public int typeOfObject = 0;

    private void OnEnable()
    {
        // change layers so player no longer collides, yet sphere cast can pick it up
        //gameObject.layer = LayerMask.NameToLayer("NonInteractiveSurfaceThatCanTurnTransluscent");
        Invoke("TurnTransluscent", 1.0f);

        StartCoroutine("CheckForMovement"); // allow debris to fly around, and turn it off when it stops moving

        Invoke("StopThisObject", 6f); // failsafe, in case debris never stops moving
    }

    IEnumerator CheckForMovement()
    {
        Vector3 _lastCheckedVelocity = GetComponent<Rigidbody>().velocity;

        yield return new WaitForSeconds(0.2f);

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
        hasSettled = true;

        GetComponent<Rigidbody>().isKinematic = true; // no more moving/falling
                                                      //GetComponent<Collider>().enabled = false; // no more collision // this doesnt work as the sphere collider of MakeSurroundingsInvisible cannot detect this anymore

        // change layers so player no longer collides, yet sphere cast can pick it up
        //gameObject.layer = LayerMask.NameToLayer("WaterShadow");Ignore Raycast
        //gameObject.layer = LayerMask.NameToLayer("NonInteractiveSurfaceThatCanTurnTransluscent");
    }

    void TurnTransluscent()
    {
        // change layers so player no longer collides, yet sphere cast can pick it up
        gameObject.layer = LayerMask.NameToLayer("NonInteractiveSurfaceThatCanTurnTransluscent");
    }
}
