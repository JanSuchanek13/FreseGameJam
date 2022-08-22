using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractOnContact : MonoBehaviour
{
    [Header("Counting Crowns")]

    int[] crowns = new int[3];


    [Header("Set Available States")]

    [Tooltip("The Number of collected Models in last Round")]
    public int StateNumber = 0;


    [Header("Sounds")]

    public AudioSource Sound_PickUp;
    public AudioSource Sound_CollectModel;
    public AudioSource Sound_ReachedFriend;


    [Header("REFERENCES")]

    [Tooltip("Reference to collectedCrowns on GameManager")]
    CollectedCrowns collectedCrowns;
    [Tooltip("Reference to stateManager on Player")]
    public OrigamiController origamiController;
    [Tooltip("Reference of the currentLevel with LevelIndex")]
    private int currentLevel;
    

    private void Start()
    {
        //get current Level
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //get CollectedCrowns Reference
        collectedCrowns = GameObject.Find("GameManager").GetComponent<CollectedCrowns>();

        // activate the available States
        StateNumber = PlayerPrefs.GetInt("State" + (currentLevel - 2));
        for (int i = 0; i < StateNumber; i++)
        {
            switch (i + 1)
            {
                case 1:
                    origamiController.possibleStatesList[1].isAvailable = true;
                    break;

                case 2:
                    origamiController.possibleStatesList[2].isAvailable = true;
                    break;

                case 3:
                    origamiController.possibleStatesList[3].isAvailable = true;
                    break;

                case 4:
                    origamiController.possibleStatesList[4].isAvailable = true;
                    break;

                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Crane"))
        {
            Debug.Log("hit:Crane");
            origamiController.possibleStatesList[1].isAvailable = true;
            Sound_CollectModel.Play(0);
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 1);
        }
        if (other.gameObject.CompareTag("Frog"))
        {
            Debug.Log("hit:Frog");
            origamiController.possibleStatesList[3].isAvailable = true;
            Sound_CollectModel.Play(0);
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 4);
        }

        if (other.gameObject.CompareTag("Capricorn"))
        {
            Debug.Log("hit:Capricorn");
            origamiController.possibleStatesList[2].isAvailable = true;
            Sound_CollectModel.Play(0);
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 2);
        }
        if (other.gameObject.CompareTag("Lama"))
        {
            Debug.Log("hit:Lama");
            origamiController.possibleStatesList[4].isAvailable = true;
            Sound_CollectModel.Play(0);
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 3);
        }
        if (other.gameObject.CompareTag("Crown"))
        {
            Debug.Log("hit:Crown");

            //play Sound
            Sound_PickUp.Play(0);

            // for Level Stats
            crowns[(currentLevel - 2)] = PlayerPrefs.GetInt("crowns" + (currentLevel - 2));
            PlayerPrefs.SetInt("crowns" + (currentLevel - 2), crowns[(currentLevel - 2)] + 1);
            Debug.Log(PlayerPrefs.GetInt("crowns"));

            //disable the crown
            other.gameObject.SetActive(false);

            // for CollectedCrowns
            collectedCrowns.MakeBoolArray(false);
        }
        if (other.gameObject.CompareTag("Friend"))
        {
            Debug.Log("hit:Friend");

            //play Sound
            Sound_ReachedFriend.Play(0);

        }
    }

    
}
