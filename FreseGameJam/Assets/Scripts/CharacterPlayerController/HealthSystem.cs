using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HealthSystem : MonoBehaviour
{
    private IEnumerator coroutine;
    public Vector3 respawnPoint; // felix made this public for portable respawn position
    // felix added this to grab respawn orientation-point:
    GameObject _respawnPointGO;
    Vector3 _hardcoreRespawn; // kinda redundant...
    public bool inCoroutine;
    public bool fadeBlackOnDeath;
    public GameObject fadeToBlackBlende;

    [SerializeField] GameObject Cam;
    [SerializeField] GameObject Cam2;
    [SerializeField] GameObject Cam2_Falling;
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
        // save the spawnposition to circumvent the tori-gates
        _hardcoreRespawn = transform.position;

        CamScript = GetComponentInChildren<CloseQuarterCamera>();
        Character = GetComponentInChildren<CharacterController>();
        gravity = GetComponent<ThirdPersonMovement>().gravity;

        //if continue the Game start at last Checkpoint
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        // save respawn for HARDCORE!:
        //respawnPoint = transform.position;

        //Debug.Log(CheckpointsGO.Count);
        for (int i = 0; i < CheckpointsGO.Count; i++)
        {
            
            Checkpoints.Add(CheckpointsGO[i].transform.position);
            //Debug.Log(Checkpoints[i]);
        }
        

        
        if (Checkpoints[lastCheckpoint[0]] != new Vector3(0, 0, 0) && PlayerPrefs.GetInt("HardcoreMode", 0) == 0) // this was "currentLevel - 2"?
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
            _respawnPointGO = other.gameObject;

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
            Debug.Log("currently reached checkpoints: " + PlayerPrefs.GetInt("lastCheckpoint" + 0) + " of " + Checkpoints.Count); //felix add
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
        if (fadeBlackOnDeath && fadeToBlackBlende != null)
        {
            StartCoroutine("FadeToBlack");
        }

        if (PlayerPrefs.GetInt("HardcoreMode", 0) == 0)
        {
            inCoroutine = true;

            // hauler your scream:
            StartCoroutine(DeathScream());

            // turn off artificial shadow:
            Shadow _shadowGO = FindObjectOfType<Shadow>();
            _shadowGO.DisableShadow();

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
            GetComponent<ThirdPersonMovement>().velocity.y = 1;
            //GetComponent<ThirdPersonMovement>().enabled = false; // no movement hopefully stops me from being able to survive death zones //LavaTiles wont Work if this is aktiv
            GetComponent<CharacterController>().enabled = false; // no colission = sink into even shallow deathzones
            GetComponent<Rigidbody>().isKinematic = false;

            yield return new WaitForSeconds(1f);

            GetComponent<Rigidbody>().isKinematic = true;
            // respawn player and face him toward direction of linear level progression:
            gameObject.transform.position = new Vector3(0, -3, 0) + respawnPoint;
            gameObject.transform.LookAt(_respawnPointGO.transform.Find("Orientation Point"));

            //GetComponent<ThirdPersonMovement>().enabled = true; // no movement hopefully stops me from being able to survive death zones //LavaTiles wont Work if this is aktiv
            GetComponent<CharacterController>().enabled = true;

            EnableCameras();
            ResetCloseUpCam(); // this resets closeup-cam to the respawned pos of player
            RefocusCamera();

            GetComponent<ThirdPersonMovement>().gravity = gravity;
            inCoroutine = false;

            // turn on artificial shadow:
            _shadowGO.EnableShadow();
        } 
        else // hardcore!
        {
            inCoroutine = true;

            StartCoroutine(DeathScream());

            // turn off artificial shadow:
            Shadow _shadowGO = FindObjectOfType<Shadow>();
            _shadowGO.DisableShadow();

            //count deaths and save them
            deaths[0] = PlayerPrefs.GetInt("HardcoreDeaths" + 0, 0); // (currentLevel - 2) & (currentLevel - 2) in end
            PlayerPrefs.SetInt("HardcoreDeaths" + 0, deaths[0] + 1);// (currentLevel - 2) & (currentLevel - 2) in end

            //stop Riverboat
            if (RiverBoat_Friend.activeInHierarchy)
            {
                RiverBoat_Friend.GetComponent<SpeedUpNavMeshAgent>().StopForDead();
            }

            DisableCameras();

            GetComponent<ThirdPersonMovement>().gravity = 0;
            //GetComponent<ThirdPersonMovement>().enabled = false; // no movement hopefully stops me from being able to survive death zones //LavaTiles wont Work if this is aktiv
            GetComponent<CharacterController>().enabled = false; // no colission = sink into even shallow deathzones
            GetComponent<Rigidbody>().isKinematic = false;

            yield return new WaitForSeconds(1f);

            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.position = new Vector3(0, 2, 0) + _hardcoreRespawn;
            //GetComponent<ThirdPersonMovement>().enabled = true; // no movement hopefully stops me from being able to survive death zones //LavaTiles wont Work if this is aktiv
            GetComponent<CharacterController>().enabled = true;

            EnableCameras();
            ResetCloseUpCam(); // this resets closeup-cam to the respawned pos of player
            RefocusCamera();

            GetComponent<ThirdPersonMovement>().gravity = gravity;
            inCoroutine = false;

            // turn on artificial shadow:
            _shadowGO.EnableShadow();

            // throw away progression on crowns:
            FindObjectOfType<HardcoreMode>().ResetCurrentHardcoreCrowns();
        }
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

        if (_arrayOfScreams.Length != 0)
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

    IEnumerator FadeToBlack()
    {
        Color tempColor = fadeToBlackBlende.GetComponent<Image>().color;
        while (tempColor.a < 1)
        {
            yield return new WaitForSeconds(0.005f);
            tempColor.a += 0.2f;
            fadeToBlackBlende.GetComponent<Image>().color = tempColor;
        }
        yield return new WaitForSeconds(0.5f);
        while (tempColor.a > 0)
        {
            yield return new WaitForSeconds(0.05f);
            tempColor.a -= 0.1f;
            fadeToBlackBlende.GetComponent<Image>().color = tempColor;
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
        Cam2_Falling.SetActive(false);
        Cam2.SetActive(false);
        Cam.SetActive(false);
        _craneCam.SetActive(false); // beware, when calling this function in pause while in crane, then the crane cam would be turnt off after, though being in crane-form! 
    }
    public void EnableCameras()
    {
        CamScript.enabled = true;

        if(_lastUsedMainCam == null)
        {
            Cam.SetActive(true);

            //Cam.GetComponent<CinemachineFreeLook>().Follow = this.gameObject.transform;
            //Cam.GetComponent<CinemachineFreeLook>().LookAt = this.gameObject.transform;
            //Cam.transform.position = new Vector3(_respawnPointGO.transform.Find("Orientation Point").transform.position.x * -5, _respawnPointGO.transform.Find("Orientation Point").transform.position.y, _respawnPointGO.transform.Find("Orientation Point").transform.position.z * -5);

            // turn cam to look in maps linear-progression-direction:

            //Transform _saveTrans = this.gameObject.transform;
            //Cam.GetComponent<CinemachineFreeLook>().LookAt = _respawnPointGO.transform.Find("Orientation Point");
            //Cam.GetComponent<CinemachineFreeLook>().LookAt = _saveTrans;

            //_cinemachineCamComponent.m_LookAt = _respawnPointGO.transform.Find("Orientation Point");
            //GetComponent<CinemachineFreeLook>().LookAt = _focusTargetObject.transform;
            //_cinemachineCamComponent.LookAt(_respawnPointGO.transform.Find("Orientation Point"));

            //}

            //RefocusCamera(Cam);
        }
        else
        {
            _lastUsedMainCam.SetActive(true);

            //Transform _saveTrans = this.gameObject.transform;
            //_lastUsedMainCam.transform.position = _respawnPointGO.transform.Find("Orientation Point").transform.position;

            //_lastUsedMainCam.GetComponent<CinemachineFreeLook>().LookAt = _respawnPointGO.transform.Find("Orientation Point");
            //_lastUsedMainCam.GetComponent<CinemachineFreeLook>().LookAt = _saveTrans;

            // turn cam to look in maps linear-progression-direction:
            /*if (_respawnPointGO != null)
            {
                Cinemachine.CinemachineVirtualCamera _cinemachineCamComponent = _lastUsedMainCam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
                _cinemachineCamComponent.m_LookAt = _respawnPointGO.transform.Find("Orientation Point");
            }*/
            //RefocusCamera(_lastUsedMainCam);

        }
    }

    public void RefocusCamera()
    {
        GameObject _cam;
        if (_lastUsedMainCam == null)
        {
            Cam.SetActive(true);
            _cam = Cam;
        }else
        {
            _lastUsedMainCam.SetActive(true);
            _cam = _lastUsedMainCam;
        }

        if (_cam == null)
        {
            Debug.LogError("The provided camera GameObject is null!");
            return;
        }

        Cinemachine.CinemachineFreeLook freeLookCam = _cam.GetComponent<Cinemachine.CinemachineFreeLook>();

        if (freeLookCam == null)
        {
            Debug.LogError("The provided GameObject doesn't have a CinemachineFreeLook component!");
            return;
        }

        freeLookCam.LookAt = this.transform;

        // Adjust the camera's horizontal axis to match the respawn point's intended direction.
        freeLookCam.m_XAxis.Value = this.transform.eulerAngles.y;
        freeLookCam.m_YAxis.Value = 0.65f; 
    }

    private IEnumerator WaitAndReleaseCamera(Cinemachine.CinemachineFreeLook freeLookCam, float _storedSettings)
    {
        yield return new WaitForSeconds(0.1f); // wait for 0.1 seconds
                                               // Reset the axis control to allow player input to control the camera.
        freeLookCam.m_XAxis.m_MaxSpeed = 300; // reset to a typical value or whatever value you use
    }

    void ResetCloseUpCam()
    {
        if (FindObjectOfType<SmoothCloseUpCameraAdjuster>())
        {
            FindObjectOfType<SmoothCloseUpCameraAdjuster>().ResetPosition(); // in case you died far from the last respawn in closeUpCam!
        }
    }
}
