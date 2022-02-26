using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private IEnumerator coroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            StartCoroutine(deadAndRespawn());
        }
    }

    private IEnumerator deadAndRespawn()
    {
        float gravity = GetComponent<ThirdPersonMovement>().gravity;
        GetComponent<ThirdPersonMovement>().gravity = gravity / 10;
        yield return new WaitForSeconds(1f);
        gameObject.transform.position = new Vector3(0,0,0);
        GetComponent<ThirdPersonMovement>().gravity = gravity;
    }
}
