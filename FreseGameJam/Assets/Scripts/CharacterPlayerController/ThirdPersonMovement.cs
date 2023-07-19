using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    public InputHandler input;



    public CharacterController controller;
    private StateController stateController;
    public Transform cam;
    public GameObject thirdPersonCam;
    public GameObject craneCam;
    private Gamepad pad;

    public float speed = 6f;
    public float humanSpeed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    public Vector3 velocity;
    public float gravity = -9.81f;
    public int gravityClamp = -15;
    public float jumpHeight = 3f;
    public int flyCurve = 3000;
    public float capricornSpeed = 25;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    //public bool CheckForGroundContact;
    public bool onBridge;
    public bool movingDownwards;
    public bool forcedFalling;
    Vector3 oldtransform;


    //Slide Down
    private bool willSlideOnSlopes = true;
    private float slopeSpeed = 8f;
    private Vector3 hitPointNormal;
    public LayerMask SlideMasks;
    private float rayDistance = 0.2f;
    private float rayDistanceCorner = 0.141f;
    private float rayLength = 0.8f;
    private float fallOnSlopes = 90;

    

    //for Animation
    public bool idle;
    public bool walking;
    public bool falling;
    public bool jumping;
    public bool action;
    public float animationSpeed;

    //for VFX
    [Header("Goat Effects:")]
    public ParticleSystem dash;
    public ParticleSystem dash02;
    public ParticleSystem buildingup;
    [SerializeField] ParticleSystem _impactEffect;
    [Space(10)]
    [Header("Walking Effects:")]
    public ParticleSystem runningDust;
    public ParticleSystem jumpingDust;


    //cooldown
    bool isInCooldown;
    public float cooldownTime = 0.5f;

    //jumpChange
    bool jumpChange;

    //Sound
    [Header("Sounds:")]
    public AudioSource jumpSound;

    //coyote time
    [Header("CoyoteTime:")]
    public bool coyoteTime = true;
    public float coyoteTimeDelay = 0.8f;
    public bool isCoyoteGrounded;

    //hold for gliding
    [Header("hold for Gliding:")]
    public bool holdForGliding;

    [Header("Crane Juice:")]
    public GameObject craneSound;
    public GameObject craneVFX;

    //for capricorn Dash
    [Header("Capricorn Dash Stats:")]
    public float chargeTime = 0.8f;
    public float dashWidth = 4;
    public float dashSpeed = 20;
    public Transform dashCheck;
    public LayerMask dashMask;
    public bool inDash;
    public bool breakableDash;
    public AudioSource buildingUp_Sound;
    public AudioSource dash_Sound;
    public AudioSource dashCrash_Sound;
    [SerializeField] GameObject dashWallBreakCheck;

    //for Lama Shoot
    [Header("Lama Shoot Stats:")]
    public float bulletSpeed = 50;
    [SerializeField] Rigidbody bulletType;
    private Quaternion restingPosition;
    [Header("Shoot Sounds:")]
    [SerializeField] AudioSource[] arrayOfGun_Sounds;

    Camera Cam;
    Vector3 pos = new Vector3(960, 600, 0);
    Vector3 reticlePosition;


    //test if sliding
    public bool isSliding
    {
        get
        {
            Debug.DrawRay(transform.position + new Vector3(rayDistanceCorner, 0, rayDistanceCorner), Vector3.down * rayLength, Color.red);
            
            if ((Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 0f, SlideMasks, QueryTriggerInteraction.Ignore)/* || (Physics.Raycast(transform.position + new Vector3(0.4f,0,0), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, -0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, 0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(-0.4f, 0, 0), Vector3.down, out slopeHit, 1f))*/))
            {
                //Debug.Log("Treffer");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(rayDistance, 0, 0), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer1");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(rayDistanceCorner, 0, rayDistanceCorner), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore)) //Ecke
            {
                //Debug.Log("Treffer1 Ecke");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(0, 0, -rayDistance), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer2");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(rayDistanceCorner, 0, -rayDistanceCorner), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore)) //Ecke
            {
                //Debug.Log("Treffer1 Ecke");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(0, 0, rayDistance), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer3");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(-rayDistanceCorner, 0, rayDistanceCorner), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore)) //Ecke
            {
                //Debug.Log("Treffer1 Ecke");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(-rayDistance, 0, 0), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer4");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else if (Physics.Raycast(transform.position + new Vector3(-rayDistanceCorner, 0, -rayDistanceCorner), Vector3.down, out slopeHit, rayLength, SlideMasks, QueryTriggerInteraction.Ignore)) //Ecke
            {
                //Debug.Log("Treffer1 Ecke");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit && Vector3.Angle(hitPointNormal, Vector3.up) < fallOnSlopes;
            }
            else
            {
                return false;
            }
        }
    }

    public bool isOnSteps
    {
        
        get
        {
            Debug.DrawRay(transform.position, gameObject.transform.forward * 0.7f, Color.blue);
            bool test = false;
            for (int i = 0; i < 10; i++)
            {
                Debug.DrawRay(transform.position, (gameObject.transform.forward + Vector3.down/i) * 0.7f, Color.blue);
                if ((Physics.Raycast(transform.position, (gameObject.transform.forward + Vector3.down / i )*0.7f, out RaycastHit slopeHit, 0f, SlideMasks, QueryTriggerInteraction.Ignore)/* || (Physics.Raycast(transform.position + new Vector3(0.4f,0,0), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, -0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, 0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(-0.4f, 0, 0), Vector3.down, out slopeHit, 1f))*/))
                {
                    test = Vector3.Angle(hitPointNormal, Vector3.up) < controller.slopeLimit;
                }
                else
                {
                    test = false;
                }
                
            }
            return test;
        }
    }

    //test if player is high enough for double Jump to crane
    public bool isTooCloseToGround
    {
        get
        {
            
            Debug.DrawRay(transform.position, Vector3.down *10, Color.blue);
            if ((Physics.Raycast(transform.position, Vector3.down *10, 1f, SlideMasks, QueryTriggerInteraction.Ignore)))
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void Awake()
    {
        playerInput = new PlayerInput();
        SetGliding();
    }

    public void SetGliding()
    {
        if (PlayerPrefs.GetInt("glidingSettings") > 0)
        {
            holdForGliding = true;
        }
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

    void Start()
    {
        Cam = cam.gameObject.GetComponent<Camera>();
        stateController = GetComponent<StateController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            GetComponent<CharacterController>().stepOffset = 1f;
            onBridge = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            GetComponent<CharacterController>().stepOffset = 0.5f;
            onBridge = false;
        }
    }

    /*
    void Update()
    {
        //crane juice
        craneSound.SetActive(stateController.crane);
        craneVFX.SetActive(stateController.crane);

        if (GetComponent<StateController>().human)
        {
            //if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && !isGrounded && !(Physics.CheckSphere(groundCheck.position, groundDistance * 7, groundMask, QueryTriggerInteraction.Ignore)) && !GetComponent<StateController>().isChanging) // double jump change you into crane
            if ((input.jumpTriggerd) && !isGrounded && !isCoyoteGrounded && !CheckForGroundContact() && !GetComponent<StateController>().isChanging) // double jump change you into crane
            {

                if (stateController.availableCrane)
                {
                    GetComponent<StateController>().isChanging = true;
                    stateController.ball = false;
                    stateController.human = false;
                    stateController.frog = false;
                    stateController.crane = true;
                    stateController.capricorn = false;
                    stateController.lama = false;
                    StartCoroutine(GetComponent<StateController>().changeModell(2));
                }

            }
            if (holdForGliding && input.jumpValue == 0 && stateController.crane)
            {
                GetComponent<StateController>().isChanging = true;
                stateController.ball = false;
                stateController.human = true;
                stateController.frog = false;
                stateController.crane = false;
                stateController.capricorn = false;
                stateController.lama = false;
                StartCoroutine(GetComponent<StateController>().changeModell(1));
            }
        }
    }*/

    private void FixedUpdate()
    {
        if (willSlideOnSlopes && isSliding && !isOnSteps && !onBridge)
        {
            Vector3 moveDir = new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
            controller.Move(moveDir.normalized * humanSpeed * 0.5f * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ReadyForJumpChange();
        bool test = isOnSteps;
        //crane juice
        craneSound.SetActive(stateController.crane);
        craneVFX.SetActive(stateController.crane);

        if (GetComponent<StateController>().human)
        {
            //if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && !isGrounded && !(Physics.CheckSphere(groundCheck.position, groundDistance * 7, groundMask, QueryTriggerInteraction.Ignore)) && !GetComponent<StateController>().isChanging) // double jump change you into crane
            if (/*input.jumpValue == 1 && jumpChange*/input.jumpTriggerd && !Physics.Raycast(transform.position, Vector3.down, 3f, groundMask, QueryTriggerInteraction.Ignore) && !isCoyoteGrounded && !GetComponent<StateController>().isChanging) // double jump change you into crane
            {

                if (stateController.availableCrane)
                {
                    GetComponent<StateController>().isChanging = true;
                    stateController.ball = false;
                    stateController.human = false;
                    stateController.frog = false;
                    stateController.crane = true;
                    stateController.capricorn = false;
                    stateController.lama = false;
                    StartCoroutine(GetComponent<StateController>().changeModell(2));
                }

            }
            if (holdForGliding && input.jumpValue == 0 && stateController.crane)
            {
                GetComponent<StateController>().isChanging = true;
                stateController.ball = false;
                stateController.human = true;
                stateController.frog = false;
                stateController.crane = false;
                stateController.capricorn = false;
                stateController.lama = false;
                StartCoroutine(GetComponent<StateController>().changeModell(1));
            }
        }

        // animation
        if ((input.moveValue.x != 0 || input.moveValue.y != 0) && isCoyoteGrounded)
        {
            walking = true;
            runningDust.gameObject.SetActive(true);
        }
        else
        {
            walking = false;
            runningDust.gameObject.SetActive(false);
            idle = true;
        }

        //Camera and Movement
        float horizontal = input.moveValue.x;
        float vertical = input.moveValue.y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= .1f || isSliding)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //Debug.Log(hitPointNormal);
            //Debug.Log(moveDir);
            //if sliding the move direction will be overwriten
            if (willSlideOnSlopes && isSliding && !isOnSteps && !onBridge)
            {
                //moveDir = new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed; //wird ausgef�hrt in Fixedupdate
                //controller.Move(moveDir.normalized * humanSpeed*0.5f * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            

        }


        movingDownwards = transform.position.y +.1 < oldtransform.y;
        oldtransform = transform.position;

        //Check Ground
        //controller = GetComponent<CharacterController>();

        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance * 2, groundMask, QueryTriggerInteraction.Ignore);
        if (!CheckForGroundContact() && velocity.y < -10 && movingDownwards)
        {
            falling = true; //animation

            FindObjectOfType<CloseQuarterCamera>().isFalling = falling; // be sure that any other paper avatar in the scene does not have this script attached!
        }
        else if(forcedFalling)
        {
            falling = true; //animation
            input.enabled = false;
            FindObjectOfType<CloseQuarterCamera>().isFalling = falling; // be sure that any other paper avatar in the scene does not have this script attached!
        }
        else
        {
            falling = false;
            FindObjectOfType<CloseQuarterCamera>().isFalling = falling; // be sure that any other paper avatar in the scene does not have this script attached!
        }

        if (coyoteTime)
        {
            if(Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                isCoyoteGrounded = true;
            }
            else 
            {
                if (jumping)
                {
                    isCoyoteGrounded = false;
                }
                //if the Player doesn�t touch the ground isCoyoteGrounded will be set to false after the Coyot Delay ends.
                Invoke("CoyoteTime", coyoteTimeDelay);
            }
        }
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance *1.5f, groundMask, QueryTriggerInteraction.Ignore);
        if (Physics.CheckSphere(groundCheck.position, groundDistance * 3, groundMask, QueryTriggerInteraction.Ignore) && velocity.y < 0.5 && !onBridge) //is falling
        {
            controller.stepOffset = 0.5f;
            //falling = false; //animation
            jumping = false; //animation
                                //controller.slopeLimit = 45.0f;
            velocity.y = -4f;
        }
        else
        {
            //falling = true; //animation
        }

        




        //States
        if (GetComponent<StateController>().human)
        {
            if (pad != null)
            {
                pad.SetMotorSpeeds(0f, 0f);
            }
            //Move 
            float speedInput = Math.Abs(horizontal) + Math.Abs(vertical); //add up Move Input
            speed = speedInput * humanSpeed; // jan you had *6 hier, so input 1 == 6 all the time
            //speed clamp f�r schr�ges laufen
            if (speed > humanSpeed)
                speed = humanSpeed;
            animationSpeed = speedInput;

            //Cam
            //thirdPersonCam.SetActive(true);
            craneCam.SetActive(false);
            FindObjectOfType<HealthSystem>().EnableCameras();


            //Jump
            if (isCoyoteGrounded)
            {
                //jumpingDust.gameObject.SetActive(false);
                jumping = false;
            }
            if (input.jumpTriggerd && isCoyoteGrounded)
            {
                controller.stepOffset = 0;
                jumping = true; //animation
                //controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpingDust.Play();
                jumpSound.Play();
            }
            
            

            //Gravity
            if (velocity.y >= gravityClamp)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            controller.Move(velocity * Time.deltaTime);
            //controller.slopeLimit = 45f;
        }
        if (GetComponent<StateController>().frog)
        {
            //Move 
            speed = 50f;

            //Jump
            if (input.jumpValue != 0 && CheckForGroundContact())
            {
                jumping = true; //animation
                controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight * 10 * -2f * gravity); //high jump
            }

            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        if (GetComponent<StateController>().crane)
        {
            //Move
            if (CheckForGroundContact()  && !GetComponent<StateController>().isChanging || controller.isGrounded)
            {
                speed = 0f; //walk speed

                //change back to human on ground
                StartCoroutine(GetComponent<StateController>().changeModell(1));
                stateController.ball = false;
                stateController.human = true;
                stateController.frog = false;
                stateController.crane = false;
                stateController.capricorn = false;
                stateController.lama = false;
                //StartCoroutine(GetComponent<StateController>().changeModell(1));

                
            }
            else
            {
                speed = 6f; //fly speed
            }

            //Cam
            thirdPersonCam.SetActive(false);
            craneCam.SetActive(true);

            
            //No Player Input -> fly forward
            if (input.moveValue.x == 0 && input.moveValue.y == 0)
            {
                //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // fly towards camera
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + transform.eulerAngles.y; // fly towards avatar forward
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            

            //Jump
            if (input.jumpValue != 0 && CheckForGroundContact())
            {
                //controller.slopeLimit = 0f; //low climp
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);  //low jump
            }
            else if(input.jumpTriggerd && !CheckForGroundContact() && !GetComponent<StateController>().isChanging && !holdForGliding)
            {
                //change back to human by pressing jump mid air
                GetComponent<StateController>().isChanging = true;
                
                stateController.ball = false;
                stateController.human = true;
                stateController.frog = false;
                stateController.crane = false;
                stateController.capricorn = false;
                stateController.lama = false;
                StartCoroutine(GetComponent<StateController>().changeModell(1));

                
            }
            if (holdForGliding && input.jumpValue == 0)
            {
                //only if hold For gliding is true
                //change back to human by releasing jump button mid air
                GetComponent<StateController>().isChanging = true;
                stateController.ball = false;
                stateController.human = true;
                stateController.frog = false;
                stateController.crane = false;
                stateController.capricorn = false;
                stateController.lama = false;
                StartCoroutine(GetComponent<StateController>().changeModell(1));

                
            }

            //Gravity
            //float curve = 1 ^ Time.deltaTime;
            velocity.y = gravity / (flyCurve * Time.deltaTime); //low gravity

            controller.Move(velocity * Time.deltaTime);

            //velocity.y += gravity/10 * Time.deltaTime; //low gravity
            //controller.Move(velocity * Time.deltaTime);
            
        }
        if (GetComponent<StateController>().capricorn)
        {
            //Move 
            speed = 2;
            /*
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            */

            //Jump
            /*
            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isGrounded)
            {
                controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);
            }
            */

            if (input.jumpValue != 0 & !isInCooldown)
            {
                action = true;
                Invoke("EndOfAction", 1.2f);
                //Debug.Log("fire");
                StartCoroutine("CapricornDash");
                isInCooldown = true;
                StartCoroutine("Cooldown");
            }


            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        if (GetComponent<StateController>().lama)
        {
            Ray ray = Cam.ScreenPointToRay(pos + new Vector3(0, 30, 0));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                reticlePosition = hit.point;
            }

            //Move 
            speed = 4f;

            //Jump
            if (input.jumpValue != 0 && CheckForGroundContact())
            {
                controller.slopeLimit = 45f;
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);
            }

            if (input.jumpValue != 0 & !isInCooldown)
            {
                action = true;
                //action = false;
                Debug.Log("fire");
                transform.LookAt(reticlePosition);
                Invoke("LamaShoot", 0.5f);
                Invoke("EndOfAction", 0.6f);
                isInCooldown = true;
                StartCoroutine("Cooldown");
            }

            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        if (GetComponent<StateController>().jesus)
        {
            //Move 
            speed = 5;
            /*
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            */

            //Jump
            if (input.jumpValue != 0 && CheckForGroundContact())
            {
                controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);
            }
            

            if (input.jumpValue != 0 & !isInCooldown)
            {
                action = true;
                Invoke("EndOfAction", 1.2f);
                Debug.Log("fire");
                StartCoroutine("CapricornDash");
                isInCooldown = true;
                StartCoroutine("Cooldown");
            }


            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

    }


    // Felix:
    public bool CheckForGroundContact()
    {
        float _groundCheckRadius = GetComponent<CharacterController>().radius;

        // cast a tiny sphere downwards to check for groundcontact the size of the footprint: 
        RaycastHit hit;
        if (Physics.SphereCast(groundCheck.position, _groundCheckRadius * 0.9f, -groundCheck.up, out hit, 2f, groundMask))
        {
            //Gizmos.DrawWireSphere(groundCheck.position, _groundCheckRadius);
            return true;
        }
        return false;
    }

    private void EndOfAction()
    {
        action = false;
        
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isInCooldown = false;

    }

    private void ReadyForJumpChange()
    {
        if (input.jumpTriggerd && !controller.isGrounded)
        {
            jumpChange = true;
        }
        else if (controller.isGrounded)
        {
            jumpChange = false;
        }

    }

    private void CoyoteTime()
    {
        RaycastHit hit;
        isCoyoteGrounded = Physics.SphereCast(groundCheck.position, groundDistance * 0.7f, -groundCheck.up, out hit, 0.2f, groundMask, QueryTriggerInteraction.Ignore);
        //vorher Physics.CheckSphere(groundCheck.position, groundDistance * 0.5f, groundMask, QueryTriggerInteraction.Ignore);
    }

    // Felix Test:

    /// <summary>
    /// execute Dash of Capricorn
    /// </summary>
    /// <returns></returns>
    IEnumerator CapricornDash()
    {
        inDash = true;
        // Turn off player input while dashing:
        GetComponent<InputHandler>().enabled = false;
        //rumble
        pad = Gamepad.current;
        if (pad != null && input.controlType == InputHandler.ControlType.Controller)
        {
            pad.SetMotorSpeeds(0f, 0.3f);
        }

        buildingUp_Sound.Play();
        buildingup.Play();

        // change capricorn Rotation towards cam:
        for (int i = 0; i < 15; i++)
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return new WaitForSeconds(0.01f);
        }

        
        //dash_Sound.Play();

        //yield return new WaitForSeconds(buildingUp_Sound.clip.length);
        yield return new WaitForSeconds(chargeTime);
        
        
        dashWallBreakCheck.SetActive(true);
        
        dash.Play();
        dash02.Play();
        GetComponent<Rigidbody>(); // why?
        breakableDash = true;


        for (int i = 0; i < 15; i++)
        {
            speed = dashSpeed;
            
            float targetAngle = cam.eulerAngles.y; // dash forward from CAMERA:
            //float targetAngle = transform.eulerAngles.y; // dash forward from AVATARS looking direction:
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (speed) * Time.deltaTime);

            // wait a splitsecond so this loop actually covers a certain distance:
            yield return new WaitForSeconds(.001f);

            // create a small bubble-explosion at impact point:
            Collider[] allObjects = Physics.OverlapSphere(transform.position, 1);
            foreach (Collider j in allObjects)
            {
                Rigidbody rig = j.GetComponent<Rigidbody>();
                if (rig != null && rig.gameObject != this.gameObject)
                {
                    rig.AddExplosionForce(1.0f, transform.position, 1, 1f, ForceMode.Impulse);
                    dashCrash_Sound.Play();
                    
                    if (pad != null && input.controlType == InputHandler.ControlType.Controller)
                    {
                        pad.SetMotorSpeeds(0.7f, 0.7f);
                    }

                    if (_impactEffect != null) // this can be thrown out when susi has imported the impact effect!
                    {
                        _impactEffect.Play();
                    }
                }
            }


            /* // unsure why this is currently not working:
            // Stop when you hit anything:
            if (Physics.CheckSphere(dashCheck.position, 0.15f, dashMask))
            {
                //dash_Sound.Stop();
                dashCrash_Sound.Play();
                yield break;
            }*/
        }
        
        speed = 2;
        inDash = false;
        breakableDash = false;
        dashWallBreakCheck.SetActive(false);
        // Turn on player input while dashing:
        GetComponent<InputHandler>().enabled = true;

        //stop rumble
        yield return new WaitForSeconds(0.5f);
        pad.SetMotorSpeeds(0, 0);
    }

    /*
    /// <summary>
    /// execute Dash of Capricorn
    /// </summary>
    /// <returns></returns>
    IEnumerator CapricornDash()
    {
        //change capricorn Rotation towards cam
        for (int i = 0; i < 15; i++)
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return new WaitForSeconds(0.01f);
        }
        

        buildingUp_Sound.Play();
        buildingup.Play();

        dash_Sound.Play(); // warum spielt der dash shound hier wo der aufbau stattfindet?! beide sounds spielen dann parallel!

        yield return new WaitForSeconds(chargeTime);
        inDash = true;
        dash.Play();
        dash02.Play();

        //speed = capricornSpeed;
        GetComponent<Rigidbody>();
        for (int i = 0; i < dashWidth *5; i++) //was passiert hier? dashWitdth * 5 in einer for loop?! benutzt du hier die vermeindliche breite des dashcorridors * 5 um die distanz zu ermitteln?
        {
            if(Physics.CheckSphere(dashCheck.position, 0.3f, dashMask)) //if we hit a Ground obj we stop the dash
            {
                dashCrash_Sound.Play();
                //Debug.Log("stopped");
                yield break;
            }
            speed = dashSpeed; // wof�r ist dann die CapricornSpeed variables?!
            float horizontal = input.moveValue.x;
            float vertical = input.moveValue.y;
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; // this is unused, or not?! what does this do?
            //float targetAngle = cam.eulerAngles.y; // dash forward from CAMERA:
            float targetAngle = transform.eulerAngles.y; // dash forward from AVATARS looking direction:
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (speed * 3) * Time.deltaTime); // warum wird hier speed multipliziert? warum nicht einfach den speed benutzen der eingestellt ist?!
            yield return new WaitForSeconds(.001f);
            //Debug.Log("lauf");

            //extra push power
            Collider[] allObjects = Physics.OverlapSphere(transform.position, 3);   //all Objects in explosion Range
            foreach (Collider j in allObjects)
            {
                Rigidbody rig = j.GetComponent<Rigidbody>();
                if (rig != null && rig.gameObject != this.gameObject)
                {
                    //Debug.Log(rig.gameObject.name);
                    rig.AddExplosionForce(1f, transform.position, 1, 1f, ForceMode.Impulse); // force was 1
                    dashCrash_Sound.Play();
                }
            }
        }
        
        speed = 2;
        inDash = false;
    }
    */
    void LamaShoot()
    {
        /*
        if (arrayOfGun_Sounds.Length == 0)
        {
            //Debug.Log("trying to play fire sound, why tho");
            AudioSource gunSound = arrayOfGun_Sounds[UnityEngine.Random.Range(0, arrayOfGun_Sounds.Length)];
            gunSound.pitch = UnityEngine.Random.Range(1, 2);
            gunSound.Play();
        }
        */
        //restingPosition = transform.rotation; // needed to reset gun
        //transform.Rotate(new Vector3(1, 1, 0), UnityEngine.Random.Range(-8f, 8f)); // spray randomly
        //transform.LookAt(reticlePosition);


        Rigidbody bulletClone = (Rigidbody)Instantiate(bulletType, transform.position + (transform.forward * 2), transform.rotation);
        bulletClone.velocity = transform.forward * bulletSpeed;
        //transform.rotation = restingPosition; // reset gun
    }
}

// replaced all isGrounded with the CheckForGroundContact() function and now everything seems to work much better on my end
// previously you were checking for isGrounded two seperate times in the same update(?) unsure if this was on purpose, so
// i just commented the old stuff and replaced all isGroundeds as said...