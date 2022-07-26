using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
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

    //cooldown
    bool isInCooldown;
    public float cooldownTime = 3;


    //for capricorn Dash
    bool dashing = true;

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
            
            if ((Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 1f, SlideMasks, QueryTriggerInteraction.Ignore)/* || (Physics.Raycast(transform.position + new Vector3(0.4f,0,0), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, -0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(0, 0, 0.4f), Vector3.down, out slopeHit, 1f)) || (Physics.Raycast(transform.position + new Vector3(-0.4f, 0, 0), Vector3.down, out slopeHit, 1f))*/))
            {
                //Debug.Log("Treffer");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(rayDistance, 0, 0), Vector3.down, out slopeHit, 1f, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer1");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(0, 0, -rayDistance), Vector3.down, out slopeHit, 1f, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer2");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(0, 0, rayDistance), Vector3.down, out slopeHit, 1f, SlideMasks, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Treffer3");
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else if (Physics.Raycast(transform.position + new Vector3(-rayDistance, 0, 0), Vector3.down, out slopeHit, 1f, SlideMasks, QueryTriggerInteraction.Ignore))
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


    // Update is called once per frame
    void Update()
    {

        // animation
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            walking = true;
        }
        else
        {
            walking = false;
            idle = true;
        }

        //Camera and Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
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
        if (!isGrounded && velocity.y < -15 && movingDownwards)
        {
            falling = true; //animation
        }
        else
        {
            falling = false;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0.5) //is falling
        {
            controller.stepOffset = 0.5f;
            falling = false; //animation
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
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                controller.stepOffset = 0;
                jumping = true; //animation
                //controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                
            }
            if (Input.GetButtonDown("Jump") && !isGrounded && !(Physics.Raycast(transform.position, Vector3.down * 10, 1f, SlideMasks, QueryTriggerInteraction.Ignore))) // double jump change you into crane
            {
                
                if (stateController.availableCrane)
                {
                    stateController.ball = false;
                    stateController.human = false;
                    stateController.frog = false;
                    stateController.crane = true;
                    stateController.capricorn = false;
                    stateController.lama = false;
                    StartCoroutine(GetComponent<StateController>().changeModell(2));
                }
                
            }

            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            controller.slopeLimit = 45f;
        }
        if (GetComponent<StateController>().frog)
        {
            //Move 
            speed = 2f;

            //Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
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
            if (isGrounded && velocity.y < 0)
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
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            //Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                controller.slopeLimit = 0f; //low climp
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);  //low jump
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
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);
            }

            if (Input.GetButtonDown("Fire1") & !isInCooldown)
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
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                controller.slopeLimit = 45f;
                velocity.y = Mathf.Sqrt(jumpHeight / 150 * -2f * gravity);
            }

            if (Input.GetButtonDown("Fire1") & !isInCooldown)
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

    IEnumerator CapricornDash()
    {
        yield return new WaitForSeconds(.8f);
        //speed = capricornSpeed;
        GetComponent<Rigidbody>();
        for (int i = 0; i < 50; i++)
        {
            speed = capricornSpeed;
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (speed * 3) * Time.deltaTime);
            yield return new WaitForSeconds(.005f);
            //Debug.Log("lauf");

            //extra push power
            Collider[] allObjects = Physics.OverlapSphere(transform.position, 3);   //all Objects in explosion Range


            foreach (Collider j in allObjects)
            {
                Rigidbody rig = j.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(1, transform.position, 1, 1f, ForceMode.Impulse);
                }
            }
        }

        speed = 2;

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
