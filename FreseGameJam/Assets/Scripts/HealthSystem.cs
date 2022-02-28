using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class HealthSystem : MonoBehaviour
{
    private IEnumerator coroutine;
    Vector3 RespawnPoint;
    bool inUse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {

            RespawnPoint = other.transform.position;
            
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("Damage"))
        {
            StartCoroutine(deadAndRespawn());
        }
    }

    private void Start()
    {
        
    }

    private IEnumerator deadAndRespawn()
    {
        

        if (!inUse)
        {
            inUse = true;
            Debug.Log(RespawnPoint);
            GameObject Cam = GetComponentInChildren<CinemachineFreeLook>().gameObject;

            float gravity = GetComponent<ThirdPersonMovement>().gravity;
            Cam.SetActive(false);
            GetComponent<ThirdPersonMovement>().gravity = gravity / 10;
            yield return new WaitForSeconds(1f);
            gameObject.transform.position = new Vector3(0, 0, 0);//+ RespawnPoint;
            Cam.SetActive(true);
            GetComponent<ThirdPersonMovement>().gravity = gravity;
            inUse = false;
        }
        
    }
}
