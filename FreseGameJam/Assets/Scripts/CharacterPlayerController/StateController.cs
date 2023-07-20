using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StateController : MonoBehaviour
{
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    private Gamepad pad;
    private InputHandler input;




    int currentLevel;

    int[] crowns = new int[3];

    public bool availableFrog;
    public bool availableCrane;
    public bool availableCapricorn;
    public bool availableLama;
    public bool availableJesus;

    public bool ball;
    public bool human = true;
    public bool frog;
    public bool crane;
    public bool capricorn;
    public bool lama;
    public bool jesus;
    public bool isChanging;
    public int StateNumber = 0;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    [Tooltip("This is used to tell the HealhtSystem what form/shape the player was in when dying. Do not touch!")]
    public int currentFormId = 1;

    private IEnumerator coroutine;
    private bool toggleBlock;
    public GameObject ballVisuell;
    [SerializeField] GameObject humanVisuell;
    [SerializeField] GameObject frogVisuell;
    [SerializeField] GameObject craneVisuell;
    [SerializeField] GameObject capricornVisuell;
    [SerializeField] GameObject lamaVisuell;
    [SerializeField] GameObject jesusVisuell;


    [SerializeField] AudioSource PickUpCrown_Sound;
    [SerializeField] AudioSource PickUpNewForm_Sound;
    [SerializeField] AudioSource Friend_Sound; // currently not used
    [SerializeField] AudioSource transformationSound;

    CollectedCrowns CollectedCrowns;

    private void Awake()
    {
        playerInput = new PlayerInput();
        input = GetComponent<InputHandler>();
    }

    /// <summary>
    /// enable Player input
    /// </summary>
    private void OnEnable()
    {

        playerInput.Enable();
    }

    /// <summary>
    /// disable Player input
    /// </summary>
    private void OnDisable()
    {

        playerInput.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Crane"))
        {
            //Debug.Log("hit:Crane");
            availableCrane = true;
            PickUpNewForm_Sound.Play();
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + 0, 1);
        }
        if (other.gameObject.CompareTag("Frog"))
        {
            //Debug.Log("hit:Frog");
            availableFrog = true;
            PickUpNewForm_Sound.Play();
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + 0, 5);
        }

        if (other.gameObject.CompareTag("Capricorn"))
        {
            //Debug.Log("hit:Capricorn");
            availableCapricorn = true;
            PickUpNewForm_Sound.Play();
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + 0, 2);
        }
        if (other.gameObject.CompareTag("Lama"))
        {
            //Debug.Log("hit:Lama");
            availableLama = true;
            PickUpNewForm_Sound.Play();
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + 0, 3);
        }
        if (other.gameObject.CompareTag("Jesus"))
        {
            Debug.Log("hit:Jesus");
            availableJesus = true;
            PickUpNewForm_Sound.Play();
            Destroy(other.gameObject, .3f);
            PlayerPrefs.SetInt("State" + 0, 4);
        }
        if (other.gameObject.CompareTag("Crown"))
        {
            Debug.Log("hit:Crown");

            // for Level Stats
            currentLevel = SceneManager.GetActiveScene().buildIndex;
            crowns[0] = PlayerPrefs.GetInt("crowns" + 0);
            PlayerPrefs.SetInt("crowns"+ 0, crowns[0] + 1);
            Debug.Log(PlayerPrefs.GetInt("crowns"));

            PickUpCrown_Sound.Play(0);
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
        bool[] availableStates = new bool[] { availableCrane, availableCapricorn, availableLama, availableJesus, availableFrog };
        StateNumber = PlayerPrefs.GetInt("State" + 0);
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
                    availableJesus = true;
                    break;

                case 5:
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

            float i;
            i = playerInput.CharacterControls.SwitchState.ReadValue<float>();

            if (isChanging == false)
            {
                //FindObjectOfType<HealthSystem>().UpdateLastUsedCamera(); // this should save currently used main camera setting!
                    
                //Debug.Log(i * 3);
                isChanging = true;

                switch (i)//change Movement state
                {
                    case 1:
                        currentFormId = 1;

                        ball = false;
                        human = true;
                        frog = false;
                        crane = false;
                        capricorn = false;
                        lama = false;
                        jesus = false;
                        StartCoroutine(changeModell(i));
                        break;

                    case 5:
                        if (availableJesus)
                        {
                            currentFormId = 5;

                            ball = false;
                            human = false;
                            frog = false;
                            crane = false;
                            capricorn = false;
                            lama = false;
                            jesus = true;
                            StartCoroutine(changeModell(i));
                        }
                        else
                            isChanging = false;
                        break;

                    case 2:
                        if (availableCrane)
                        {
                            currentFormId = 2;

                            ball = false;
                            human = false;
                            frog = false;
                            crane = true;
                            capricorn = false;
                            lama = false;
                            jesus = false;
                            StartCoroutine(changeModell(i));
                        }
                        else
                            isChanging = false;
                        break;

                    case 3:
                        if (availableCapricorn && !toggleBlock)
                        {
                            toggleBlock = true;
                            switch (human)
                            {
                                case true:
                                    currentFormId = 3;

                                    ball = false;
                                    human = false;
                                    frog = false;
                                    crane = false;
                                    capricorn = true;
                                    lama = false;
                                    jesus = false;
                                    StartCoroutine(changeModell(i));
                                    break;

                                case false:
                                    currentFormId = 1;

                                    ball = false;
                                    human = true;
                                    frog = false;
                                    crane = false;
                                    capricorn = false;
                                    lama = false;
                                    jesus = false;
                                    StartCoroutine(changeModell(1));
                                    break;
                            }
                        }
                        else
                            isChanging = false;
                        break;

                    case 4:
                        if (availableLama)
                        {
                            currentFormId = 4;

                            ball = false;
                            human = false;
                            frog = false;
                            crane = false;
                            capricorn = false;
                            lama = true;
                            jesus = false;
                            StartCoroutine(changeModell(i));
                        }
                        else
                            isChanging = false;
                        break;

                    case 6:
                        if (availableFrog)
                        {
                            currentFormId = 6;

                            ball = false;
                            human = false;
                            frog = true;
                            crane = false;
                            capricorn = false;
                            lama = false;
                            jesus = false;
                            StartCoroutine(changeModell(i));
                        }
                        else
                            isChanging = false;
                        break;


                    case 7:
                    case 8:
                    case 9:
                    case 0:
                        isChanging = false;
                        break;

                    default:
                        break;
                }


                //chaet for switching
                i = playerInput.CharacterControls.Cheating.ReadValue<float>();
                switch (i)//change Movement state
                {
                    case 1:
                        ball = false;
                        human = true;
                        frog = false;
                        crane = false;
                        capricorn = false;
                        lama = false;
                        jesus = false;
                        StartCoroutine(changeModell(i));
                        break;

                    case 5:
                        ball = false;
                        human = false;
                        frog = false;
                        crane = false;
                        capricorn = false;
                        lama = false;
                        jesus = true;
                        StartCoroutine(changeModell(i));
                        
                        break;

                    case 2:
                        ball = false;
                        human = false;
                        frog = false;
                        crane = true;
                        capricorn = false;
                        lama = false;
                        jesus = false;
                        StartCoroutine(changeModell(i));
                        
                            isChanging = false;
                        break;

                    case 3:                        
                        ball = false;
                        human = false;
                        frog = false;
                        crane = false;
                        capricorn = true;
                        lama = false;
                        jesus = false;
                        StartCoroutine(changeModell(i));
                        
                            isChanging = false;
                        break;

                    case 4:
                        ball = false;
                        human = false;
                        frog = false;
                        crane = false;
                        capricorn = false;
                        lama = true;
                        jesus = false;
                        StartCoroutine(changeModell(i));
                        
                            isChanging = false;
                        break;

                    case 6:
                        ball = false;
                        human = false;
                        frog = true;
                        crane = false;
                        capricorn = false;
                        lama = false;
                        jesus = false;
                        StartCoroutine(changeModell(i));

                        break;
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

    public IEnumerator changeModell(float state)
    {
        pad = Gamepad.current;
        if (pad != null)
        {
            pad.SetMotorSpeeds(0f, 0f);
        }

        //change to ball
        transformationSound.Play();
        ballVisuell.SetActive(true);
        humanVisuell.SetActive(false);
        frogVisuell.SetActive(false);
        craneVisuell.SetActive(false);
        capricornVisuell.SetActive(false);
        lamaVisuell.SetActive(false);
        jesusVisuell.SetActive(false);

        // play sound for transformation here!


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
                jesusVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 2:
                /* // this is probably called accidentally as you have a wait call before. click - wait - abort - done waiting execute
                pad = Gamepad.current;
                if (pad != null && input.controlType == InputHandler.ControlType.Controller)
                {
                    pad.SetMotorSpeeds(0.2f, 0.2f);
                }*/
                craneVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 3:
                pad = Gamepad.current;
                if (pad != null)
                {
                    pad.SetMotorSpeeds(0f, 0f);
                }
                capricornVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 4:
                lamaVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 6:
                frogVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            default:
                break;
        }
        isChanging = false;
        toggleBlock = false;
    }
}
