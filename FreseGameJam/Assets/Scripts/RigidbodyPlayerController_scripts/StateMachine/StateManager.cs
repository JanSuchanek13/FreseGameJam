using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    /// <summary>
    /// Reference: https://www.youtube.com/watch?v=Vt8aZDPzRjI&t=614s
    /// </summary>


    [Header("State Machine")]

    [Tooltip("holds a Reference to the active state in a state machine")]
    BaseState currentState;
    [Tooltip("Instance of HumanState in a state machine")]
    public PlayerHumanState HumanState = new PlayerHumanState();
    [Tooltip("Instance of CraneState in a state machine")]
    public PlayerCraneState CraneState = new PlayerCraneState();
    [Tooltip("Instance of CapricornState in a state machine")]
    public PlayerCapricornState CapricornState = new PlayerCapricornState();
    [Tooltip("Instance of LamaState in a state machine")]
    public PlayerLamaState LamaState = new PlayerLamaState();
    [Tooltip("Instance of FrogState in a state machine")]
    public PlayerFrogState FrogState = new PlayerFrogState();


    [Header("Input for switching")]

    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;


    [Header("Model switching")]

    [Tooltip("is the Player changing his Model")]
    bool isChanging;
    [Tooltip("In which state is the Player able to move in")]
    public bool availableFrog;
    public bool availableCrane;
    public bool availableCapricorn;
    public bool availableLama;
    [Tooltip("Reference to the Model Visuells")]
    [SerializeField] GameObject ballVisuell;
    [SerializeField] GameObject humanVisuell;
    [SerializeField] GameObject frogVisuell;
    [SerializeField] GameObject craneVisuell;
    [SerializeField] GameObject capricornVisuell;
    [SerializeField] GameObject lamaVisuell;

    private void Awake()
    {
        playerInput = new PlayerInput();
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

    private void Start()
    {
        //starting State for State machine
        currentState = HumanState;
        //"this" is a reference to the context (this EXACT Monobehavier script)
        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        currentState.UpdateState(this);
    }

    private void Update()
    {

        if (playerInput.CharacterControls.SwitchState.ReadValue<float>() != 0)
        {
            
            //Debug.Log(i);
            if (isChanging == false)
            {
                isChanging = true;
                float i = playerInput.CharacterControls.SwitchState.ReadValue<float>();
                Debug.Log(i);

                switch (i)//change Movement state
                {
                    case 1:
                        SwitchState(HumanState);
                        StartCoroutine(changeModell(i));
                        break;

                    case 5:
                        if (availableFrog)
                        {
                            SwitchState(FrogState);
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    case 2:
                        if (availableCrane)
                        {
                            SwitchState(CraneState);
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    case 3:
                        if (availableCapricorn)
                        {
                            SwitchState(CapricornState);
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    case 4:
                        if (availableLama)
                        {
                            SwitchState(LamaState);
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

    /// <summary>
    /// We switch the State
    /// </summary>
    /// <param name="state"></param>
    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    /// <summary>
    /// changing the Model to current state
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public IEnumerator changeModell(float state)
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
