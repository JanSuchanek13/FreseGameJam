using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
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
    bool isGrounded;

    //for Animation
    public bool idle;
    public bool walking;
    public bool falling;

    //for Lama Shoot
    [Header("Lama Shoot Stats:")]
    public float bulletSpeed = 10;
    [SerializeField] Rigidbody bulletType;
    private Quaternion restingPosition;
    [Header("Shoot Sounds:")]
    [SerializeField] AudioSource[] arrayOfGun_Sounds;

    Camera Cam;
    Vector3 pos = new Vector3(960, 600, 0);
    Vector3 reticlePosition;

    void Start()
    {
        Cam = cam.gameObject.GetComponent<Camera>();
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

        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);


           
        }

        //Check Ground
        //controller = GetComponent<CharacterController>();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance * 8, groundMask);
        if (!isGrounded)
        {
            falling = true; //animation
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0.5) //is falling
        {
            falling = false; //animation
            controller.slopeLimit = 45.0f;
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
                controller.slopeLimit = 45f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        if (GetComponent<StateController>().frog)
        {
            //Move 
            speed = 6f;

            //Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
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
            }
            else
            {
                speed = 6f; //fly speed
            }

            //Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                controller.slopeLimit = 50f; //low climp
                velocity.y = Mathf.Sqrt(jumpHeight/150 * -2f * gravity);  //low jump
            }

            //Gravity
            //float curve = 1 ^ Time.deltaTime;
            velocity.y = gravity/(flyCurve * Time.deltaTime); //low gravity
            
            controller.Move(velocity * Time.deltaTime);

            //velocity.y += gravity/10 * Time.deltaTime; //low gravity
            //controller.Move(velocity * Time.deltaTime);
        }
        if (GetComponent<StateController>().capricorn)
        {
            //Move 
            speed = capricornSpeed;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            //Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                controller.slopeLimit = 100f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        if (GetComponent<StateController>().lama)
        {
            Ray ray = Cam.ScreenPointToRay(pos);
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
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("fire");
                LamaShoot();
            }
            //Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        
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
        transform.LookAt(reticlePosition);


        Rigidbody bulletClone = (Rigidbody)Instantiate(bulletType, transform.position + (transform.forward * 2), transform.rotation);
        bulletClone.velocity = transform.forward * bulletSpeed;
        //transform.rotation = restingPosition; // reset gun
    }
}
