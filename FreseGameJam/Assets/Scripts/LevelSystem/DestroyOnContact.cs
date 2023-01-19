using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("killzone triggered");
        if (!other.CompareTag("Indestructable"))
        {
            Debug.Log("killzone destroyed: " + other.name);

            Destroy(other.gameObject);
        }
    }
}
