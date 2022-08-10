using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gentleforge;

[RequireComponent(typeof(GentleController))]
public class OrigamiController : MonoBehaviour
{
    [Header("DATA")]
    [Tooltip("Current state of the character. For me it would be OrigamiState.Anxiety")]
    public OrigamiState curState;

    [Header("DESIGN")]
    [Tooltip("List with all Scriptable Obj. States")]
    public List<PossibleState> possibleStatesList = new List<PossibleState>();

    [Header("REFERENCES")]
    private GentleController myController;

    private void OnEnable()
    {
        OrigamiEvents.onStateChange_Event += ChangeState;
    }

    private void OnDisable()
    {
        OrigamiEvents.onStateChange_Event -= ChangeState;
    }

    private void Awake()
    {
        myController = GetComponent<GentleController>();
    }

    private void Start()
    {
        ChangeState(OrigamiState.Human);
    }

    /// <summary>
    /// Change to another Origami State
    /// </summary>
    /// <param name="_state"></param>
    public void ChangeState(OrigamiState _state)
    {
        foreach (PossibleState posssibleState in possibleStatesList)
        {
            // If it is not the correct State we Continue
            if (posssibleState.state != _state)
                continue;

            // We can only change to the State if Available
            if (!posssibleState.isAvailable)
                return;

            curState = _state;

            // We change the Controller Data
            myController.moveSpeed = posssibleState.controllerData.moveSpeed;
            myController.maxMoveVelocity = posssibleState.controllerData.maxMoveVelocity;
            myController.jumpForce = posssibleState.controllerData.jumpForce;
            myController.maxJumpVelocity = posssibleState.controllerData.maxJumpVelocity;
            myController.gravityCurve = posssibleState.controllerData.gravityCurve;
            myController.gravityStrengthMultiplier = posssibleState.controllerData.gravityStrengthMultiplier;
            myController.gravityCurveSpeed = posssibleState.controllerData.gravityCurveSpeed;
            myController.baseDrag = posssibleState.controllerData.baseDrag;
            myController.airDrag = posssibleState.controllerData.airDrag;

            // We change the Visuals
            DeactivateVisuals();
            posssibleState.visualReference.SetActive(true);
            
            Debug.Log("OrigamiStateChanger: changed State to " + _state);

            return;
        }

        Debug.LogError("OrigamiStateChanger: could not change to state " + _state + "! Please check possibleStatesList or if the State is set correctly in the Data");
    }

    public void DeactivateVisuals()
    {
        foreach (PossibleState posssibleState in possibleStatesList)
        {
            posssibleState.visualReference.SetActive(false);
        }
    }

    private void Update()
    {
        GetPlayerInput();
    }

    /// <summary>
    /// We get the Input of the Player
    /// </summary>
    public void GetPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OrigamiEvents.SendStateChangeEvent(OrigamiState.Human);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            OrigamiEvents.SendStateChangeEvent(OrigamiState.Crane);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            OrigamiEvents.SendStateChangeEvent(OrigamiState.Frog);
    }
}

/// <summary>
/// Dropdown Menu with useable States
/// </summary>
[System.Serializable]
public enum OrigamiState
{
    Human,
    Crane,
    Capricorn,
    Lama,
    Frog
}

[System.Serializable]
public class PossibleState
{
    public OrigamiState state;
    public bool isAvailable = false;
    public GameObject visualReference;
    public OrigamiControllerStateData controllerData;
}
