using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour
{
    int currentLevel;

    int[] crowns = new int[3];

    public bool availableFrog;
    public bool availableCrane;
    public bool availableCapricorn;
    public bool availableLama;

    public bool ball;
    public bool human = true;
    public bool frog;
    public bool crane;
    public bool capricorn;
    public bool lama;
    bool isChanging;
    public int StateNumber = 0;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };


    private IEnumerator coroutine;
    public GameObject ballVisuell;
    [SerializeField] GameObject humanVisuell;
    [SerializeField] GameObject frogVisuell;
    [SerializeField] GameObject craneVisuell;
    [SerializeField] GameObject capricornVisuell;
    [SerializeField] GameObject lamaVisuell;


    [SerializeField] AudioSource PickUp_Sound;
    [SerializeField] AudioSource PickUp_Sound2;
    [SerializeField] AudioSource Friend_Sound;

    CollectedCrowns CollectedCrowns;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Crane"))
        {
            Debug.Log("hit:Crane");
            availableCrane = true;
            PickUp_Sound2.enabled = true;
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 1);
        }
        if (other.gameObject.CompareTag("Frog"))
        {
            Debug.Log("hit:Frog");
            availableFrog = true;
            PickUp_Sound2.enabled = true;
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 4);
        }

        if (other.gameObject.CompareTag("Capricorn"))
        {
            Debug.Log("hit:Capricorn");
            availableCapricorn = true;
            PickUp_Sound2.enabled = true;
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 2);
        }
        if (other.gameObject.CompareTag("Lama"))
        {
            Debug.Log("hit:Lama");
            availableLama = true;
            PickUp_Sound2.enabled = true;
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + (currentLevel - 2), 3);
        }
        if (other.gameObject.CompareTag("Crown"))
        {
            Debug.Log("hit:Crown");

            // for Level Stats
            currentLevel = SceneManager.GetActiveScene().buildIndex;
            crowns[(currentLevel - 2)] = PlayerPrefs.GetInt("crowns" + (currentLevel-2));
            PlayerPrefs.SetInt("crowns"+ (currentLevel - 2), crowns[(currentLevel - 2)] + 1);
            Debug.Log(PlayerPrefs.GetInt("crowns"));

            PickUp_Sound.Play(0);
            //PickUp_Sound.enabled = true;
            // PickUp_Sound.enabled = false;
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);

            // for CollectedCrowns
            CollectedCrowns.MakeBoolArray(false);
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
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        human = true;
        CollectedCrowns = GameObject.Find("GameManager").GetComponent<CollectedCrowns>();

        //PlayerPrefs.SetInt("State" + currentLevel, 0);
        // activate the available States
        bool[] availableStates = new bool[] { availableCrane, availableCapricorn, availableLama, availableFrog };
        StateNumber = PlayerPrefs.GetInt("State" + (currentLevel - 2));
        for (int i = 0; i < StateNumber; i++)
        {
            switch (i+1)
            {
                case 1:
                    availableCrane = true;
                    break;

                case 2:
                    availableCapricorn = true;
                    break;

                case 3:
                    availableLama = true;
                    break;

                case 4:
                    availableFrog = true;
                    break;

                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isChanging == false)
        {
            

            for (int i = 0; i < keyCodes.Length; ++i)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    if (isChanging == false)
                    {

                    
                        //Debug.Log(i * 3);
                        isChanging = true;

                        switch (i)//change Movement state
                        {
                            case 1:
                                ball = false;
                                human = true;
                                frog = false;
                                crane = false;
                                capricorn = false;
                                lama = false;
                                StartCoroutine(changeModell(i));
                                break;

                            case 5:
                                if (availableFrog)
                                {
                                    ball = false;
                                    human = false;
                                    frog = true;
                                    crane = false;
                                    capricorn = false;
                                    lama = false;
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
                                    lama = false;
                                    StartCoroutine(changeModell(i));
                                }
                                break;

                            case 3:
                                if (availableCapricorn)
                                {
                                    ball = false;
                                    human = false;
                                    frog = false;
                                    crane = false;
                                    capricorn = true;
                                    lama = false;
                                    StartCoroutine(changeModell(i));
                                }
                                break;

                            case 4:
                                if (availableLama)
                                {
                                    ball = false;
                                    human = false;
                                    frog = false;
                                    crane = false;
                                    capricorn = false;
                                    lama = true;
                                    StartCoroutine(changeModell(i));
                                }
                                break;

                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 0:
                                isChanging = false;
                                break;

                            default:
                                break;
                        }
                    }
                    
                }
            }
        }
    }

    public IEnumerator changeModell(int state)
    {
        //change to ball
        ballVisuell.SetActive(true);
        humanVisuell.SetActive(false);
        frogVisuell.SetActive(false);
        craneVisuell.SetActive(false);
        capricornVisuell.SetActive(false);
        lamaVisuell.SetActive(false);


        //change to new form
        yield return new WaitForSeconds(.5f);
        switch (state)
        {
            case 1:
                craneVisuell.SetActive(false);
                humanVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 5:
                frogVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 2:
                craneVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 3:
                capricornVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 4:
                lamaVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            default:
                break;
        }
        isChanging = false;
    }
}
