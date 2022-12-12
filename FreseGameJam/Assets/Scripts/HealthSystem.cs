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

    [SerializeField] GameObject Cam;
    [SerializeField] GameObject Cam2;
    CloseQuarterCamera CamScript;
    CharacterController Character;
    public GameObject RiverBoat_Friend;
    float gravity;

    //safe Level
    int currentLevel;
    int[] deaths = new int[3];
    public List<GameObject> CheckpointsGO;
    List<Vector3> Checkpoints = new List<Vector3>(); // list of all Checkpoints in this Level
    public int[] lastCheckpoint = new int[3];// int of the last Checkpoint in each Level, for Continue the Game

    // Felix:
    [SerializeField] AudioSource[] arrayOfScreams;
    [SerializeField] bool superDramaticDeath = true;
    //[SerializeField] AudioSource choireHymnn;
    //[SerializeField] AudioSource fireSwoosh;


    private void Start()
    {
        //Cam = GetComponentInChildren<CinemachineFreeLook>().gameObject; //old
        //Debug.Log(Cam);
        CamScript = GetComponentInChildren<CloseQuarterCamera>();
        Character = GetComponentInChildren<CharacterController>();

        gravity = GetComponent<ThirdPersonMovement>().gravity;

        //if continue the Game start at last Checkpoint
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log(CheckpointsGO.Count);
        for (int i = 0; i < CheckpointsGO.Count; i++)
        {
            
            Checkpoints.Add(CheckpointsGO[i].transform.position);
            //Debug.Log(Checkpoints[i]);
        }
        

        
        if (Checkpoints[lastCheckpoint[0]] != new Vector3(0, 0, 0)) // this was "currentLevel - 2"?
        {
            //Debug.Log("changed Pos");
            gameObject.transform.position = new Vector3(0, -3, 0) + Checkpoints[PlayerPrefs.GetInt("lastCheckpoint" + (currentLevel - 2))];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {

            // play Sounds when reaching checkpoint:
            //choireHymnn.Play();
            //fireSwoosh.Play();

            RespawnPoint = other.transform.position;

            //safe last checkpoint
            for (int i = 0; i < Checkpoints.Count ; i++)
            {
                if(RespawnPoint == Checkpoints[i]) //test which checkpoint we have just gone through
                {
                    lastCheckpoint[0] = i; // (_currentLevel - 2)
                }
            }
            PlayerPrefs.SetInt("lastCheckpoint" + 0, lastCheckpoint[0]); // (_currentLevel - 2) & (_currentLevel - 2) at end
            //Debug.Log(PlayerPrefs.GetInt("lastCheckpoint" + (currentLevel - 2)));

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
            inUse = true;
            //count deaths and save them
            deaths[(currentLevel - 2)] = PlayerPrefs.GetInt("deaths" + (currentLevel - 2));
            PlayerPrefs.SetInt("deaths" + (currentLevel - 2), deaths[(currentLevel - 2)] + 1);
            //Debug.Log(PlayerPrefs.GetInt("deaths" + (currentLevel - 2)));

            //stop Riverboat
            if (RiverBoat_Friend.activeInHierarchy)
            {
                RiverBoat_Friend.GetComponent<SpeedUpNavMeshAgent>().StopForDead();
            }
            
            
            //Debug.Log(RespawnPoint);

            //Respawn Player
            CamScript.enabled = false;
            Cam2.SetActive(false);
            Cam.SetActive(false);
            GetComponent<ThirdPersonMovement>().gravity = 0;
            //yield return new WaitForSeconds(1f);
            
            #region Felix stuff:
            GetComponent<ThirdPersonMovement>().enabled = false; // no movement hopefully stops me from being able to survive death zones
            GetComponent<CharacterController>().enabled = false; // no colission = sink into even shallow deathzones
            GetComponent<Rigidbody>().isKinematic = false;

            #region Audio:
            // Get random sound:
            AudioSource _randomDeathScream = arrayOfScreams[UnityEngine.Random.Range(0, arrayOfScreams.Length)];
            float _lengthOfScream = _randomDeathScream.clip.length;

            // Random pitch:
            float _randomPitch = UnityEngine.Random.Range(1.5f, 2.5f);
            _randomDeathScream.pitch = _randomPitch;

            // Play the sound:
            _randomDeathScream.Play();
            #endregion

            if (superDramaticDeath)
            {
                yield return new WaitForSeconds(_lengthOfScream);
            }else
            {
                yield return new WaitForSeconds(1f);
            }

            gameObject.transform.position = new Vector3(0, -3, 0) + RespawnPoint;

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<ThirdPersonMovement>().enabled = true; // no movement hopefully stops me from being able to survive death zones
            GetComponent<CharacterController>().enabled = true;

            Cam2.SetActive(false);
            Cam.SetActive(true);
            #endregion


            CamScript.enabled = true;
            GetComponent<ThirdPersonMovement>().gravity = gravity;
            inUse = false;
        }
        
    }
}
