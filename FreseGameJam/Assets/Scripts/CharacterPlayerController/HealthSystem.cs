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
    public bool inCoroutine;

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

    [Header("Death-Screams:")]
    [SerializeField] AudioSource[] _humanScreams;
    [SerializeField] AudioSource[] _craneScreams;
    [SerializeField] AudioSource[] _goatScreams;
    [SerializeField] AudioSource[] _llamaScreams;
    [SerializeField] AudioSource[] _jesusScreams;
    [SerializeField] AudioSource[] _frogScreams;

    AudioSource[] _arrayOfScreams;
    [SerializeField] bool useFullLengthDeathScreams = true;


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
            if (!inCoroutine)
            {
                StartCoroutine(DieAndRespawn());
            }
        }
    }

    IEnumerator DieAndRespawn()
    {
        /*if (!inCoroutine)
        {*/
            inCoroutine = true;
        
            // hauler your scream:
            StartCoroutine(DeathScream());

            //count deaths and save them
            deaths[0] = PlayerPrefs.GetInt("deaths" + 0); // (currentLevel - 2) & (currentLevel - 2) in end
            PlayerPrefs.SetInt("deaths" + 0, deaths[0] + 1);// (currentLevel - 2) & (currentLevel - 2) in end

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

        yield return new WaitForSeconds(1f);



        gameObject.transform.position = new Vector3(0, -3, 0) + respawnPoint;

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<ThirdPersonMovement>().enabled = true; // no movement hopefully stops me from being able to survive death zones
            GetComponent<CharacterController>().enabled = true;


            EnableCameras();
            #endregion

            GetComponent<ThirdPersonMovement>().gravity = gravity;
            inCoroutine = false;
        /*}
        else
        {
            return;
        }*/
    }

    /// <summary>
    /// Gets current form to allow various deathscreams depending on current state while dying.
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathScream()
    {
        // get current form-ID:
        int _stateID = FindObjectOfType<StateController>().currentFormId;

        switch (_stateID)
        {
            case 1: // human:
                _arrayOfScreams = _humanScreams;
                break;

            case 2: // crane:
                _arrayOfScreams = _craneScreams;
                break;

            case 3: // goat:
                _arrayOfScreams = _goatScreams;
                break;

            case 4: // llama:
                _arrayOfScreams = _llamaScreams;
                break;

            case 5: // jesus:
                _arrayOfScreams = _jesusScreams;
                break;

            case 6: // frog:
                _arrayOfScreams = _frogScreams;
                break;

            case 7: // the next big thing in Holliwood!
                //_arrayOfScreams = _alienRobotDragonQueensScreams;
                break;
        }

        float _lengthOfScream = 0.0f;

        if (_arrayOfScreams != null)
        {
            AudioSource _randomDeathScream = _arrayOfScreams[UnityEngine.Random.Range(0, _arrayOfScreams.Length)];
            _lengthOfScream = _randomDeathScream.clip.length;
            float _randomPitch = UnityEngine.Random.Range(0.9f, 1.1f);
            _randomDeathScream.pitch = _randomPitch;

            _randomDeathScream.Play();
        }
        
        /*
        _randomDeathScream = _arrayOfScreams[UnityEngine.Random.Range(0, _arrayOfScreams.Length)];
        _lengthOfScream = _randomDeathScream.clip.length;
        _randomPitch = UnityEngine.Random.Range(0.9f, 1.1f);
        _randomDeathScream.pitch = _randomPitch;

        _randomDeathScream.Play();*/

        if (useFullLengthDeathScreams)
        {
            yield return new WaitForSeconds(_lengthOfScream);
        }else
        {
            // clamp death scream to last a max. of 1 seconds (which is the time of respawn)
            yield return new WaitForSeconds(Mathf.Clamp(_lengthOfScream, 0.0f, 1.0f));
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
