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

    GameObject Cam;
    float gravity;

    private void Start()
    {
        Cam = GetComponentInChildren<CinemachineFreeLook>().gameObject;

        gravity = GetComponent<ThirdPersonMovement>().gravity;
    }

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

    

    private IEnumerator deadAndRespawn()
    {
        

        if (!inUse)
        {
            GameObject.Find("riverBoat_Friend_Fire").GetComponent<SpeedUpNavMeshAgent>().StopForDead();
            inUse = true;
            Debug.Log(RespawnPoint);
           
            Cam.SetActive(false);
            GetComponent<ThirdPersonMovement>().gravity = gravity / 10;
            yield return new WaitForSeconds(1f);
            gameObject.transform.position = new Vector3(0, -3, 0) + RespawnPoint;
            Cam.SetActive(true);
            GetComponent<ThirdPersonMovement>().gravity = gravity;
            inUse = false;
        }
        
    }
}
