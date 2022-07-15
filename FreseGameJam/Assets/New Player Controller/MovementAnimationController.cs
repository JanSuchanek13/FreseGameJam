using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementAnimationController : MonoBehaviour
{
    public Transform cam;
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    //variables to balance Movement
    [SerializeField] float speed = 1;
    float rotationFactorPerFrame = 15.0f; //how fast the player can turn
    

    //variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;

    //for gravity
    float gravity = -9.8f;
    float groundedGravity = -.05f;

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

    private void Awake()
    {
        //intially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        
        //Set Player input Callbacks
        
        playerInput.CharacterControls.Move.started += onMovementInput; //Key down 
        playerInput.CharacterControls.Move.canceled += onMovementInput; //Key up
        playerInput.CharacterControls.Move.performed += onMovementInput; //for controller
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            jumping = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity * .5f;
        }
        else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void handleRotation()
    {

        Vector3 positionToLookAt;
        //the change in position our character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        //the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            //old rotation
            //creates a new rotation bsed on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

            /* //new Rotation
            Quaternion targetRotation = Quaternion.Euler(0, cam.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
            */
        }
        
    }

    void onMovementInput (InputAction.CallbackContext context)
    {
        /* //Versuch 
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement += context.ReadValue<Vector2>().x * GetCameraRight(cam.GetComponent<Camera>()) * speed;
        currentMovement += context.ReadValue<Vector2>().y * GetCameraForward(cam.GetComponent<Camera>()) * speed;
        



        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        if (!isMovementPressed)
        {
            currentMovement = Vector3.zero;
        }
        */

        
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * speed;
        currentMovement.z = currentMovementInput.y * speed;
        changeMovementwithRotation();
        currentMovement.y = 0; //delete this if there is any gravity Problem
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        
        
    }

    void changeMovementwithRotation()
    {
        currentMovement = currentMovement.x * cam.right.normalized + currentMovement.z * cam.forward.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    void handleAnimation()
    {
        

        if (isMovementPressed && !walking)
        {
            walking = true;
        }
        if (!isMovementPressed && walking)
        {
            walking = false;
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f; //how fast to fall after jump

        if (characterController.isGrounded)
        {
            jumping = false;
            currentMovement.y = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
            currentMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        handleRotation();
        handleAnimation();



        
        characterController.Move(currentMovement * Time.deltaTime);

        handleGravity();
        handleJump();
        Debug.Log(characterController.isGrounded);
        

    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
