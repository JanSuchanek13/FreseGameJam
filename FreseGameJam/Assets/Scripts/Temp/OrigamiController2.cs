using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrigamiController2 : MonoBehaviour
{
    public List<CharacterBehaviours> controllerFeatures = new List<CharacterBehaviours>();
    public PlayerInput playerInput;
    public OrigamiState state;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    public void Start()
    {
        SwitchState(OrigamiState.Capricorn);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        GetPlayerInput();
    }

    public void SwitchState(OrigamiState _newState)
    {
        DeactivateAllFeatures();

        switch (_newState)
        {
            case OrigamiState.Human:
                controllerFeatures[1].enabled = true;
                break;
            case OrigamiState.Crane:
                break;
            case OrigamiState.Capricorn:
                Debug.Log("switching to CapricornState");
                controllerFeatures[0].enabled = true;
                break;
            case OrigamiState.Lama:
                break;
            case OrigamiState.Frog:
                break;
            default:
                break;
        }
        state = _newState;
    }

    public void DeactivateAllFeatures()
    {
        foreach (CharacterBehaviours feature in controllerFeatures)
        {
            feature.enabled = false;
        }
    }

    public void GetPlayerInput()
    {
        float i = playerInput.CharacterControls.SwitchState.ReadValue<float>();
        if (i != 0)
            SwitchState((OrigamiState)i-1);
    }
}
