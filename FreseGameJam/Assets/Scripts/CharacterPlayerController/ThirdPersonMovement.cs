using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;



    public CharacterController controller;
    private StateController stateController;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    public Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public int flyCurve = 3000;
    public float capricornSpeed = 25;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;
    public bool onBridge;
    public bool movingDownwards;
    public bool forcedFalling;
    Vector3 oldtransform;


    //Slide Down
    private bool willSlideOnSlopes = true;
    private float slopeSpeed = 8f;
    private Vector3 hitPointNormal;
    public LayerMask SlideMasks;
    private float rayDistance = 0.31f;

    //for Animation
    public bool idle;
    public bool walking;
    public bool falling;
    public bool jumping;
    public bool action;

    //for VFX
    public ParticleSystem dash;
    public ParticleSystem dash02;
    public ParticleSystem buildingup;
    

    //cooldown
    bool isInCooldown;
    public float cooldownTime = 0.5f;

    //coyote time
    [Header("CoyoteTime:")]
    public bool coyoteTime = true;
    public float coyoteTimeDelay = 0.8f;
    public bool isCoyoteGrounded;

    //hold for gliding
    [Header("hold for Gliding:")]
    public bool holdForGliding;


    //for capricorn Dash
    [Header("Capricorn Dash Stats:")]
    public float chargeTime = 0.8f;
    public float dashWidth = 4;
    public float dashSpeed = 25;
    public bool inDash;
    public AudioSource buildingUp_Sound;
    public AudioSource dash_Sound;
    public AudioSource dashCrash_Sound;

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
            Debug.DrawRay(transform.position + new Vector3(0, 0, rayDistance), Vector3.down, Color.red);
            
            if ((Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 1.2f, SlideMasks, QueryTriggerInteraction.Ignore)/* || (Physics.Raycast(transform.position + new Vector3(0.4f,0,0), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, -0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, 0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(-0.4f, 0, 0), Vector3.down, out slopeHit, 1f))*/))
            {
                //Debug.Log("Treffer");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(rayDistance, 0, 0), Vector3.down, out slopeHit, 1.2f, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer1");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(0, 0, -rayDistance), Vector3.down, out slopeHit, 1.2f, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer2");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(0, 0, rayDistance), Vector3.down, out slopeHit, 1.2f, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer3");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(-rayDistance, 0, 0), Vector3.down, out slopeHit, 1.2f, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer4");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else
            {
                return false;
            }
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
            GetComponent<CharacterController>().stepOffset = 2f;
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


    void Update()
    {
        if (GetComponent<StateController>().human)
        {
            //if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && !isGrounded && !(Physics.CheckSphere(groundCheck.position, groundDistance * 7, groundMask, QueryTriggerInteraction.Ignore)) && !GetComponent<StateController>().isChanging) // double jump change you into crane
            if ((playerInput.CharacterControls.Jump.triggered) && !isGrounded && !isCoyoteGrounded && !CheckForGroundContact() && !GetComponent<StateController>().isChanging) // double jump change you into crane
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
            if (holdForGliding && playerInput.CharacterControls.Jump.ReadValue<float>() == 0 && stateController.crane)
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // animation
        if (playerInput.CharacterControls.Move.ReadValue<Vector2>().x != 0 || playerInput.CharacterControls.Move.ReadValue<Vector2>().y != 0)
        {
            walking = true;
        }
        else
        {
            walking = false;
            idle = true;
        }

        //Camera and Movement
        float horizontal = playerInput.CharacterControls.Move.ReadValue<Vector2>().x;
        float vertical = playerInput.CharacterControls.Move.ReadValue<Vector2>().y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= .1f || isSliding)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //if sliding the move direction will be overwriten
            if (willSlideOnSlopes && isSliding && !onBridge)
            {
                moveDir = new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
            }

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }


        movingDownwards = transform.position.y +.1 < oldtransform.y;
        oldtransform = transform.position;

        //Check Ground
        //controller = GetComponent<CharacterController>();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance * 2, groundMask);
        if (!isGrounded && velocity.y < -15 && movingDownwards || forcedFalling)
        {
            falling = true; //animation
        }
        else
        {
            falling = false;
        }

        if (coyoteTime)
        {
            if(Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
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
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0.5) //is falling
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
            //Move 
            speed = 6f;

            //Jump
            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isCoyoteGrounded)
            {
                controller.stepOffset = 0;
                jumping = true; //animation
                //controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                
            }
            

            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            controller.slopeLimit = 45f;
        }
        if (GetComponent<StateController>().frog)
        {
            //Move 
            speed = 50f;

            //Jump
            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isGrounded)
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
            if (isGrounded && velocity.y < 0 && !GetComponent<StateController>().isChanging)
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

            //No Player Input -> fly forward
            if (playerInput.CharacterControls.Move.ReadValue<Vector2>().x == 0 && playerInput.CharacterControls.Move.ReadValue<Vector2>().y == 0)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            //Jump
            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isGrounded)
            {
                controller.slopeLimit = 0f; //low climp
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);  //low jump
            }
            else if(playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && !isGrounded && !GetComponent<StateController>().isChanging && !holdForGliding)
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
            if (holdForGliding && playerInput.CharacterControls.Jump.ReadValue<float>() == 0)
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

            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 & !isInCooldown)
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
            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isGrounded)
            {
                controller.slopeLimit = 45f;
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);
            }

            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 & !isInCooldown)
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
            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isGrounded)
            {
                controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);
            }
            

            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 & !isInCooldown)
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
    bool CheckForGroundContact()
    {
        //Debug.Log(" ground contact called");
        // cast a tiny sphere downwards to check for groundcontact the size of the footprint: 
        RaycastHit hit;
        if(Physics.SphereCast(groundCheck.position, 0.22f, -groundCheck.up, out hit, 2f, groundMask))
        {
            Debug.Log("true");
            return true;
        }
        return false;
        
        // returns either TRUE if a ground was detected or FALSE if no ground was detected.
        /*if (hit.collider == null)
        {
            Debug.DrawRay(groundCheck.position, -groundCheck.up, Color.green);
            return true;
        }else
        {
            return false;
        }*//*
        Gizmos.DrawWireSphere(transform.position, range);

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward * range, out hit, range, layerMask))
        {
            Gizmos.color = Color.green;
            Vector3 sphereCastMidpoint = transform.position + (transform.forward * hit.distance);
            Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawLine(transform.position, sphereCastMidpoint, Color.green);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector3 sphereCastMidpoint = transform.position + (transform.forward * (range - sphereCastRadius));
            Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
            Debug.DrawLine(transform.position, sphereCastMidpoint, Color.red);
        }*/
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


    private void CoyoteTime()
    {
        isCoyoteGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
    }

    /// <summary>
    /// execute Dash of Capricorn
    /// </summary>
    /// <returns></returns>
    IEnumerator CapricornDash()
    {
        buildingUp_Sound.Play();
        buildingup.Play();

        dash_Sound.Play();

        yield return new WaitForSeconds(chargeTime);
        inDash = true;
        dash.Play();
        dash02.Play();
        
        //speed = capricornSpeed;
        GetComponent<Rigidbody>();
        for (int i = 0; i < dashWidth *5; i++)
        {
            speed = dashSpeed;
            float horizontal = playerInput.CharacterControls.Move.ReadValue<Vector2>().x;
            float vertical = playerInput.CharacterControls.Move.ReadValue<Vector2>().y;
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            float targetAngle = cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (speed * 3) * Time.deltaTime);
            yield return new WaitForSeconds(.005f);
            //Debug.Log("lauf");

            //extra push power
            Collider[] allObjects = Physics.OverlapSphere(transform.position, 3);   //all Objects in explosion Range
            foreach (Collider j in allObjects)
            {
                Rigidbody rig = j.GetComponent<Rigidbody>();
                if (rig != null && rig.gameObject != this.gameObject)
                {
                    Debug.Log(rig.gameObject.name);
                    rig.AddExplosionForce(1, transform.position, 1, 1f, ForceMode.Impulse);
                    dashCrash_Sound.Play();
                }
            }
        }
        
        speed = 2;
        inDash = false;
    }

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