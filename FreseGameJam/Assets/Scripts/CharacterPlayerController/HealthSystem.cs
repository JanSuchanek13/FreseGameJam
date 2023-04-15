using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;


public class HealthSystem : MonoBehaviour
{
    private IEnumerator coroutine;
    public Vector3 respawnPoint; // felix made this public for portable respawn position
    public bool inUse;

    [SerializeField] GameObject Cam;
    [SerializeField] GameObject Cam2;
    [SerializeField] GameObject _craneCam;
    GameObject _lastUsedMainCam;

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
            //Debug.Log("I spawned");
            gameObject.transform.position = new Vector3(0, -3, 0) + Checkpoints[PlayerPrefs.GetInt("lastCheckpoint" + 0)];// (currentLevel - 2)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {

            // play Sounds when reaching checkpoint:
            //choireHymnn.Play();
            //fireSwoosh.Play();

            respawnPoint = other.transform.position;

            //safe last checkpoint
            for (int i = 0; i < Checkpoints.Count ; i++)
            {
                if(respawnPoint == Checkpoints[i]) //test which checkpoint we have just gone through
                {
                    lastCheckpoint[0] = i; // (_currentLevel - 2)
                }
            }
            PlayerPrefs.SetInt("lastCheckpoint" + 0, lastCheckpoint[0]); // (_currentLevel - 2) & (_currentLevel - 2) at end
            //Debug.Log(PlayerPrefs.GetInt("lastCheckpoint" + (currentLevel - 2)));

            //other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
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
            deaths[0] = PlayerPrefs.GetInt("deaths" + 0); // (currentLevel - 2) & (currentLevel - 2) in end
            PlayerPrefs.SetInt("deaths" + 0, deaths[0] + 1);// (currentLevel - 2) & (currentLevel - 2) in end
            //Debug.Log(PlayerPrefs.GetInt("deaths" + (currentLevel - 2)));

            //stop Riverboat
            if (RiverBoat_Friend.activeInHierarchy)
            {
                RiverBoat_Friend.GetComponent<SpeedUpNavMeshAgent>().StopForDead();
            }

            //UpdateLastUsedCamera();

            DisableCameras();

            //Respawn Player
            /*
            CamScript.enabled = false;
            Cam2.SetActive(false);
            Cam.SetActive(false);
            _craneCam.SetActive(false);*/
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

            gameObject.transform.position = new Vector3(0, -3, 0) + respawnPoint;

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<ThirdPersonMovement>().enabled = true; // no movement hopefully stops me from being able to survive death zones
            GetComponent<CharacterController>().enabled = true;


            EnableCameras();/*
            Cam2.SetActive(false);
            Cam.SetActive(true);*/
            //CamScript.enabled = true;

            #endregion


            //CamScript.enabled = true;
            GetComponent<ThirdPersonMovement>().gravity = gravity;
            inUse = false;
        }
    }

    /// <summary>
    /// get and save last used main camera.
    /// </summary>
    public void UpdateLastUsedCamera()
    {
        if (Cam.activeInHierarchy) // regular cam was last active!
        {
            _lastUsedMainCam = Cam;
        }else if(Cam2.activeInHierarchy) // close up cam was last active!
        {
            _lastUsedMainCam = Cam2;
        }

        Debug.Log("last saved camerea = " + _lastUsedMainCam);
    }

    public void DisableCameras()
    {
        CamScript.enabled = false;
        Cam2.SetActive(false);
        Cam.SetActive(false);
        _craneCam.SetActive(false); // beware, when calling this function in pause while in crane, then the crane cam would be turnt off after, though being in crane-form! 
    }
    public void EnableCameras()
    {
        //Cam2.SetActive(false);
        //Cam.SetActive(true);
        CamScript.enabled = true;
        //_lastUsedMainCam.SetActive(true);
        if(_lastUsedMainCam == null)
        {
            Cam.SetActive(true);
        }else
        {
            _lastUsedMainCam.SetActive(true);
        }
    }
}
