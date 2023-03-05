using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public enum ControlType { Keyboard, Controller };

    [Header("Controls")]
    [Tooltip("Set current Input Devise")]
    public ControlType controlType;

    [Header("InputActions")]
    [Tooltip("Current Jump Input")]
    public float jumpValue;
    [Tooltip("Jump triggerd Input")]
    public bool jumpTriggerd;
    [Tooltip("Current Move Input")]
    public Vector2 moveValue;

    [Header("REFERENCE")]
    private PlayerInput input;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("controlsSettings") == 0)
        {
            controlType = ControlType.Keyboard;
        }
        else
        {
            controlType = ControlType.Controller;
        }
    }

    private void OnEnable()
    {
        input = new PlayerInput(); 

        switch (controlType)
        {
            case ControlType.Keyboard:
                input.CharacterControlsKeyboard.Enable();

                input.CharacterControlsKeyboard.Move.performed += SetMove;
                input.CharacterControlsKeyboard.Move.canceled += SetMove;

                input.CharacterControlsKeyboard.Jump.performed += SetJump;
                input.CharacterControlsKeyboard.Jump.canceled += SetJump;
                break;

            case ControlType.Controller:
                input.CharacterControlsController.Enable();

                input.CharacterControlsController.Move.performed += SetMove;
                input.CharacterControlsController.Move.canceled += SetMove;

                input.CharacterControlsController.Jump.performed += SetJump;
                input.CharacterControlsController.Jump.canceled += SetJump;

                break;
        }
    }
    private void OnDisable()
    {
        switch (controlType)
        {
            case ControlType.Keyboard:
                input.CharacterControlsKeyboard.Move.performed -= SetMove;
                input.CharacterControlsKeyboard.Move.canceled -= SetMove;

                input.CharacterControlsKeyboard.Jump.performed -= SetJump;
                input.CharacterControlsKeyboard.Jump.canceled -= SetJump;

                input.CharacterControlsKeyboard.Disable();
                break;

            case ControlType.Controller:
                input.CharacterControlsController.Move.performed -= SetMove;
                input.CharacterControlsController.Move.canceled -= SetMove;

                input.CharacterControlsController.Jump.performed -= SetJump;
                input.CharacterControlsController.Jump.canceled -= SetJump;

                input.CharacterControlsController.Disable();
                break;
        }
    }

    private void SetJump(InputAction.CallbackContext ctx)
    {
        jumpValue = ctx.ReadValue<float>();
        Debug.Log(jumpValue);
    }



    private void SetMove(InputAction.CallbackContext ctx)
    {
        moveValue = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        switch (controlType)
        {
            case ControlType.Keyboard:
                jumpTriggerd = input.CharacterControlsKeyboard.Jump.triggered;
                break;

            case ControlType.Controller:
                jumpTriggerd = input.CharacterControlsController.Jump.triggered;
                break;
        }
    }
}
