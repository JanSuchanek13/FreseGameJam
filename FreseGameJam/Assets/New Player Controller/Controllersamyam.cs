using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controllersamyam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput movementController;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;

    //for Jumping
    bool isJumpPressed = false;
    float initialJumpVelocity;
    public float maxJumpHeight = 1f;
    public float maxJumpTime = 0.5f;
    bool isJumping = false;

    //for Animation
    public bool idle;
    public bool walking;
    public bool falling;
    public bool jumping;
    public bool action;

    private void OnEnable()
    {
        movementController.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        movementController.CharacterControls.Disable();
    }

    private void Awake()
    {
        movementController = new PlayerInput();
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravityValue = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            controller.stepOffset = .5f;
            jumping = false;
            playerVelocity.y = 0f;
        }
        else if(!groundedPlayer && playerVelocity.y > 10)
        {
            falling = true;
        }


        Vector2 movement = movementController.CharacterControls.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        bool isMovementPressed = movement.x != 0 || movement.y != 0;
        walking = isMovementPressed;
        controller.Move(move * Time.deltaTime * playerSpeed);
        

        

        // Changes the height position of the player.. for Jumping
        if (movementController.CharacterControls.Jump.triggered && groundedPlayer)
        {
            //playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            controller.stepOffset = 0;
            jumping = true;
            isJumping = true;
            playerVelocity.y = initialJumpVelocity * .5f;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if(movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
