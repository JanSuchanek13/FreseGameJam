using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Header("Cam")]

    [Tooltip("Main Cam")]
    public GameObject Cam;
    [Tooltip("CloseQuarterCam")]
    public GameObject Cam2;
    [Tooltip("Reference of the CloseQuarterCamera Script")]
    CloseQuarterCamera CamScript;
    //float gravity; gravity tauschen wenn GentleController etwas ähnliches hat| lösche gravity komplett


    [Header("Safe checkpoints for this Level")]

    [Tooltip("current Level we are moving")]
    int currentLevel;
    [Tooltip("deaths Counter for all 3 Level")]
    int[] deaths = new int[3];
    [Tooltip("List of all Checkpoints in this Level")]
    public List<GameObject> CheckpointsGO;
    [Tooltip("List with CheckPoints for Respawn")]
    List<Vector3> Checkpoints = new List<Vector3>();
    [Tooltip("int of the last Checkpoint in each Level, for Continue the Game")]
    public int[] lastCheckpoint = new int[3];


    [Header("Sound")]

    [Tooltip("Array with Sounds for death scream")]
    [SerializeField] AudioSource[] arrayOfScreams;
    [Tooltip("Should the Respawn start after the death scream finished")]
    [SerializeField] bool superDramaticDeath = true;
    //[SerializeField] AudioSource choireHymnn;
    //[SerializeField] AudioSource fireSwoosh;

    [Header("REFERENCES")]

    [Tooltip("Reference of the RespawnPoint")]
    public Vector3 RespawnPoint; // felix made this public for portable respawn position
    public bool inUse;


    private void Start()
    {
        CamScript = GetComponentInChildren<CloseQuarterCamera>();
        //gravity = GetComponent<Gentleforge.GentleController>().gravity;  gravity tauschen wenn GentleController etwas ähnliches hat| lösche gravity komplett

        //if continue the Game start at last Checkpoint
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(CheckpointsGO.Count);
        for (int i = 0; i < CheckpointsGO.Count; i++)
        {

            Checkpoints.Add(CheckpointsGO[i].transform.position);
            //Debug.Log(Checkpoints[i]);
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

            // play Sounds when reaching checkpoint:
            //choireHymnn.Play();
            //fireSwoosh.Play();

            //set RespawnPoint with Pos of Checkpoint
            RespawnPoint = other.transform.position;

            //safe last checkpoint
            for (int i = 0; i < Checkpoints.Count; i++)
            {
                if (RespawnPoint == Checkpoints[i]) //test which checkpoint we have just gone through
                {
                    lastCheckpoint[(currentLevel - 2)] = i;
                }
            }
            PlayerPrefs.SetInt("lastCheckpoint" + (currentLevel - 2), lastCheckpoint[(currentLevel - 2)]);

            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("Damage"))
        {
            StartCoroutine(deadAndRespawn());
        }
    }


    /// <summary>
    /// We respawn the Player, Count Death, stop some Movement in the World
    /// </summary>
    /// <returns></returns>
    private IEnumerator deadAndRespawn()
    {
        // test if we are currently running through a Respawn
        if (!inUse) 
        {
            inUse = true;

            //count deaths and save them
            deaths[(currentLevel - 2)] = PlayerPrefs.GetInt("deaths" + (currentLevel - 2));
            PlayerPrefs.SetInt("deaths" + (currentLevel - 2), deaths[(currentLevel - 2)] + 1);

            //stop Riverboat
            GameObject.Find("riverBoat_Friend_Fire").GetComponent<SpeedUpNavMeshAgent>().StopForDead();

            //Stop Cam
            CamScript.enabled = false;
            Cam2.SetActive(false);
            Cam.SetActive(false);
            //GetComponent<Gentleforge.GentleController>().gravity = 0; gravity tauschen wenn GentleController etwas ähnliches hat | lösche gravity komplett

            //Sink into Ground
            GetComponent<Gentleforge.GentleController>().enabled = false; 
            GetComponent<BoxCollider>().enabled = false; // no colission = sink into even shallow deathzones

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
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }

            //Set Player Pos to RespawnPoint
            gameObject.transform.position = new Vector3(0, -3, 0) + RespawnPoint;

            //disable sink into Ground
            GetComponent<Gentleforge.GentleController>().enabled = true; // no movement hopefully stops me from being able to survive death zones
            GetComponent<BoxCollider>().enabled = true;

            //activate Main Cam
            Cam2.SetActive(false);
            Cam.SetActive(true);
            CamScript.enabled = true;


            //GetComponent<Gentleforge.GentleController>().gravity = gravity; gravity tauschen wenn GentleController etwas ähnliches hat | lösche gravity komplett
            inUse = false;
        }

    }
}
