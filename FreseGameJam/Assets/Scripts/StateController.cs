using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour
{
    int[] crowns = new int[3];

    public bool availableFrog;
    public bool availableCrane;
    public bool availableCapricorn;

    public bool ball;
    public bool human = true;
    public bool frog;
    public bool crane;
    public bool capricorn;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };


    private IEnumerator coroutine;
    public GameObject ballVisuell;
    [SerializeField] GameObject humanVisuell;
    [SerializeField] GameObject frogVisuell;
    [SerializeField] GameObject craneVisuell;
    [SerializeField] GameObject capricornVisuell;

    [SerializeField] AudioSource PickUp_Sound;
    [SerializeField] AudioSource PickUp_Sound2;
    [SerializeField] AudioSource Friend_Sound;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Crane"))
        {
            Debug.Log("hit:Crane");
            availableCrane = true;
            PickUp_Sound2.enabled = true;
            Destroy(other.gameObject, .3f);
        }
        if (other.gameObject.CompareTag("Frog"))
        {
            Debug.Log("hit:Frog");
            availableFrog = true;
            PickUp_Sound2.enabled = true;
            Destroy(other.gameObject, .3f);
        }
        if (other.gameObject.CompareTag("Crown"))
        {
            Debug.Log("hit:Crown");

            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            crowns[(currentLevel - 2)] = PlayerPrefs.GetInt("crowns" + (currentLevel-2));
            PlayerPrefs.SetInt("crowns"+ (currentLevel - 2), crowns[(currentLevel - 2)] + 1);
            Debug.Log(PlayerPrefs.GetInt("crowns"));

            PickUp_Sound.enabled = true;
            //PickUp_Sound.enabled = true;
            // PickUp_Sound.enabled = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Friend"))
        {
            Debug.Log("hit:Friend");
            Friend_Sound.enabled = true;
            //PickUp_Sound.enabled = true;
            // PickUp_Sound.enabled = false;
            //Destroy(other.gameObject);
        }
    }

    private void Start()
    {
        human = true;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                Debug.Log(i * 3);

                switch (i)//change Movement state
                {
                    case 1:
                        ball = false;
                        human = true;
                        frog = false;
                        crane = false;
                        capricorn = false;
                        StartCoroutine(changeModell(i));
                        break;

                    case 3:
                        if (availableFrog)
                        {
                            ball = false;
                            human = false;
                            frog = true;
                            crane = false;
                            capricorn = false;
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    case 2:
                        if (availableCrane)
                        {
                            ball = false;
                            human = false;
                            frog = false;
                            crane = true;
                            capricorn = false;
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    case 4:
                        if (availableCapricorn)
                        {
                            ball = false;
                            human = false;
                            frog = false;
                            crane = false;
                            capricorn = true;
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private IEnumerator changeModell(int state)
    {
        //change to ball
        ballVisuell.SetActive(true);
        humanVisuell.SetActive(false);
        frogVisuell.SetActive(false);
        craneVisuell.SetActive(false);
        capricornVisuell.SetActive(false);


        //change to new form
        yield return new WaitForSeconds(.5f);
        switch (state)
        {
            case 1:
                humanVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 3:
                frogVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 2:
                craneVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 4:
                capricornVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            default:
                break;
        }
    }
}
