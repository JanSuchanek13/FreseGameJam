using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    private IEnumerator coroutine;
    public Vector3 RespawnPoint; // felix made this public for portable respawn position
    public bool inUse;

    GameObject Cam;
    float gravity;

    //safe Level
    int currentLevel;
    int[] deaths = new int[3];
    public List<GameObject> CheckpointsGO;
    List<Vector3> Checkpoints = new List<Vector3>(); // list of all Checkpoints in this Level
    int[] lastCheckpoint = new int[3];// int of the last Checkpoint in each Level, for Continue the Game


    private void Start()
    {
        Cam = GetComponentInChildren<CinemachineFreeLook>().gameObject;

        gravity = GetComponent<ThirdPersonMovement>().gravity;

        //if continue the Game start at last Checkpoint
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(CheckpointsGO.Count);
        for (int i = 0; i < CheckpointsGO.Count; i++)
        {
            
            Checkpoints.Add(CheckpointsGO[i].transform.position);
            Debug.Log(Checkpoints[i]);
        }
        

        
        if (Checkpoints[lastCheckpoint[(currentLevel - 2)]] != new Vector3(0, 0, 0))
        {
            Debug.Log("changed Pos");
            gameObject.transform.position = new Vector3(0, -3, 0) + Checkpoints[PlayerPrefs.GetInt("lastCheckpoint" + (currentLevel - 2))];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {

            RespawnPoint = other.transform.position;

            //safe last checkpoint
            for (int i = 0; i < Checkpoints.Count ; i++)
            {
                if(RespawnPoint == Checkpoints[i]) //test which checkpoint we have just gone through
                {
                    lastCheckpoint[(currentLevel - 2)] = i;
                }
            }
            PlayerPrefs.SetInt("lastCheckpoint" + (currentLevel - 2), lastCheckpoint[(currentLevel - 2)]);
            Debug.Log(PlayerPrefs.GetInt("lastCheckpoint" + (currentLevel - 2)));

            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            //Debug.Log("updated checkpouin"); //felix add
        }
        if (other.gameObject.CompareTag("Damage"))
        {
            StartCoroutine(deadAndRespawn());
            //count deaths and save them
            
        }
    }

    

    private IEnumerator deadAndRespawn()
    {
        

        if (!inUse)
        {
            //count deaths and save them
            deaths[(currentLevel - 2)] = PlayerPrefs.GetInt("deaths" + (currentLevel - 2));
            PlayerPrefs.SetInt("deaths" + (currentLevel - 2), deaths[(currentLevel - 2)] + 1);
            Debug.Log(PlayerPrefs.GetInt("deaths" + (currentLevel - 2)));

            //stop Riverboat
            GameObject.Find("riverBoat_Friend_Fire").GetComponent<SpeedUpNavMeshAgent>().StopForDead();
            inUse = true;
            Debug.Log(RespawnPoint);
            
            //Respawn Player
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
