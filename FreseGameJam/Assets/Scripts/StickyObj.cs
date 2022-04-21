using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObj : MonoBehaviour
{
    public int stickyTime = 12;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        transform.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine("Breaking");
        transform.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
    }

    IEnumerator Breaking()
    {
        yield return new WaitForSeconds(stickyTime);
        transform.GetComponent<BoxCollider>().enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}