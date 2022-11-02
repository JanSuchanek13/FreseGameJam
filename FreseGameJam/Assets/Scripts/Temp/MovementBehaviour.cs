using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementBehaviour : CharacterBehaviours
{
    [Header("MOVEMENT")]
    public float speed = 6f;

    [Header("TURN")]
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    [Header("DATA")]
    public Vector3 moveDirection;
    public bool isMoving = false;

    [Header("REFERENCES")]
    private CharacterController characterController;
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    private Transform camMain;

    public delegate void MovementEvent(Vector3 _direction);
    public static MovementEvent onMovementStart_Event;
    public static MovementEvent onMovementEnd_Event;

    public void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        camMain = Camera.main.transform;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

    private void ResetScript()
    {
        isMoving = false;
    }

    void Update()
    {
        if (isMoving)
            Movement();
        
        GetPlayerInput();
    }

    /// <summary>
    /// We get the Player Input
    /// </summary>
    public void GetPlayerInput()
    {
        moveDirection = new Vector3(playerInput.CharacterControls.Move.ReadValue<Vector2>().x, 0f, playerInput.CharacterControls.Move.ReadValue<Vector2>().y).normalized;

        if (moveDirection.magnitude != 0)
        {
            StartMovement();
        }
        else
        {
            EndMovement();
        }
    }

    /// <summary>
    /// We start the Dash
    /// </summary>
    public void StartMovement()
    {
        isMoving = true;

        // We modify the Variables
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + camMain.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        // We send an Event when the Movement Starts
        if (onMovementStart_Event != null)
            onMovementStart_Event(moveDirection);
    }

    /// <summary>
    /// We do the Movement
    /// </summary>
    public void Movement()
    {
        characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
    }

    public void EndMovement()
    {
        // We reset the variables
        ResetScript();

        // We send an Event when the Dash End
        if (onMovementEnd_Event != null)
            onMovementEnd_Event(moveDirection);
    }
}
