using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupGentleController : MonoBehaviour
{
    [RequireComponent(typeof(Rigidbody))]
    public class GentleController : MonoBehaviour
    {
        Vector3 directionMove;

        [Header("MOVEMENT")]
        [Tooltip("The force that we apply to the movement")]
        public float moveSpeed = 5;
        [Tooltip("The upper limit of the Move Velocity")]
        public float maxMoveVelocity = 10;
        [Tooltip("Var for calculate Mathf.SmoothDampAngle")]
        float turnSmoothVelocity = 1;
        [Tooltip("Var for calculate Mathf.SmoothDampAngle")]
        public float turnSmoothTime = 0.1f;

        [Header("Step Offset and Stick to Wall")]
        public bool onWall;
        public float stepheight;
        public float stepSmooth;
        public LayerMask stickMasks;

        [Header("JUMP")]
        [Tooltip("The force that we apply to the Jump")]
        public float jumpForce = 100;
        [Tooltip("The upper limit of the Jump Velocity")]
        public Vector2 maxJumpVelocity = new Vector2(-10, 5);
        [Tooltip("Layer on which the Player can jump")]
        public LayerMask jumpRayLayer;
        [Tooltip("Is the Player standing on a jumpRayLayer")]
        public bool isGrounded;
        [Tooltip("List of Raycast checking for the Ground")]
        public List<RayData> jumpRayList = new List<RayData>();

        [Header("GRAVITY")]
        [Tooltip("Curve that controls the gravity")]
        public AnimationCurve gravityCurve;
        [Tooltip("Multiplier for the gravityCurve")]
        public float gravityStrengthMultiplier = 1;
        [Tooltip("Speed in which the gravityCurve is played")]
        public float gravityCurveSpeed = 1;
        [Tooltip("current Time in the gravityCurve")]
        [HideInInspector]
        public float gravityCurveTime;
        [Tooltip("Lenght of the gravityCurve")]
        [HideInInspector]
        public float gravityCurveLenght;

        [Header("DRAG")]
        [Tooltip("Drag we apply on Ground")]
        public float baseDrag = 5;
        [Tooltip("Drag we apply in air")]
        public float airDrag = 1;

        [Header("Cooldown")]
        [Tooltip("Time the Cooldown needs to reset")]
        float cooldownTime = 3;
        [Tooltip("Is the Controller in the Cooldown phase")]
        bool isInCooldown = false;

        [Header("Animation")]
        public bool walking;
        public bool falling;
        public bool jumping;
        public bool action;


        [Header("REFERENCES")]
        [Tooltip("Reference of the Rigidbody")]
        [HideInInspector]
        public Rigidbody myRigidbody;
        [Tooltip("Reference to the PlayerInput Action Mapping")]
        private PlayerInput playerInput;

        public Vector3 directionGizmo;

        public void Awake()
        {
            Setup();
            playerInput = new PlayerInput();
        }

        /// <summary>
        /// enable Player input
        /// </summary>
        private void OnEnable()
        {
            OrigamiEvents.onStateChange_Event += GetComponent<OrigamiController>().ChangeState;
            playerInput.Enable();
        }

        /// <summary>
        /// disable Player input
        /// </summary>
        private void OnDisable()
        {
            OrigamiEvents.onStateChange_Event -= GetComponent<OrigamiController>().ChangeState;
            playerInput.Disable();
        }

        /// <summary>
        /// Set diffrent Var. at the beginning of the Game
        /// </summary>
        public void Setup()
        {
            // Rigidbody Setup
            myRigidbody = this.GetComponent<Rigidbody>();
            myRigidbody.drag = baseDrag;

            // Gravity Setup
            gravityCurveTime = 0;
            gravityCurveLenght = gravityCurve.keys[gravityCurve.length - 1].time;
        }

        public void Update()
        {
            GroundCheckRay(3);
            SetAnimationBools();
            GetPlayerInput();
            DontStickOnWalls();
            CalculateGravity();

        }

        /// <summary>
        /// We get the Input of the Player
        /// </summary>
        public void GetPlayerInput()
        {
            //code for Movement Mareske ________________________________________________________
            /*
            direction = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z) - Camera.main.transform.position;
            direction = direction.normalized;

            // we get the movemnent of the player
            Vector3 movement = new Vector3((direction.x  * moveSpeed) * Input.GetAxis("Horizontal"), myRigidbody.velocity.y, (direction.z  * moveSpeed)* Input.GetAxis("Vertical"));
            myRigidbody.velocity = movement;
            */



            /*
            //code for Movement Old Third Person ________________________________________________________
            //Camera and Movement
            float horizontal = playerInput.CharacterControls.Move.ReadValue<Vector2>().x;
            float vertical = playerInput.CharacterControls.Move.ReadValue<Vector2>().y;
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= .1f)
            {

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                myRigidbody.velocity = moveDir * moveSpeed * Time.deltaTime;
                //controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            //____________________________________________________________________________________________________
            */

            //Movement
            if (!onWall)
            {
                directionMove += playerInput.CharacterControls.Move.ReadValue<Vector2>().x * new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * moveSpeed;
                directionMove += playerInput.CharacterControls.Move.ReadValue<Vector2>().y * new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * moveSpeed;
                myRigidbody.velocity = new Vector3(directionMove.x, myRigidbody.velocity.y, directionMove.z);
                directionMove = Vector3.zero;
            }


            // we get the jump input
            if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isGrounded)
            {
                //Ability for Capricorn
                if (GetComponent<OrigamiController>().curState == OrigamiState.Capricorn)
                {
                    StartCoroutine("CapricornDash");
                    isInCooldown = true;
                    StartCoroutine("Cooldown");
                }
                else
                    myRigidbody.AddForce(Vector3.up * jumpForce);
            }
        }



        /// <summary>
        /// Ray Check if Player is grounded
        /// </summary>
        void GroundCheckRay(int lengthMultiplier)
        {
            RaycastHit hit;
            foreach (RayData ray in jumpRayList)
            {
                if ((Physics.Raycast(transform.position + ray.position, ray.direction, out hit, ray.length * lengthMultiplier, jumpRayLayer)))
                {
                    if (!isGrounded)
                        OnLanding();

                    return;
                }
            }

            if (isGrounded)
                OnLiftoff();
        }



        /// <summary>
        /// gets called when standing on the Ground
        /// </summary>
        public void OnLanding()
        {
            myRigidbody.drag = baseDrag;

            isGrounded = true;
        }

        /// <summary>
        /// gets called when hanging mid Air
        /// </summary>
        public void OnLiftoff()
        {
            myRigidbody.drag = airDrag;
            gravityCurveTime = 0;

            isGrounded = false;
        }

        /// <summary>
        /// Calculate Gravity over time with gravityCurve
        /// </summary>
        public void CalculateGravity()
        {
            if (isGrounded)
                return;

            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y - gravityCurve.Evaluate(gravityCurveTime) * gravityStrengthMultiplier, myRigidbody.velocity.z);
            gravityCurveTime = gravityCurveTime + (Time.deltaTime * gravityCurveSpeed);

            // if gravityTimer is higher than the Lenght of the gravityCurve, we set it to the last Value in gravityCurve
            if (gravityCurveTime > gravityCurveLenght)
                gravityCurveTime = gravityCurveLenght;

        }

        void DontStickOnWalls()
        {
            //step in front of Controller
            Vector3 RayPositionTop = new Vector3(transform.position.x, transform.position.y + stepheight, transform.position.z);
            Vector3 RayPositionBottom = new Vector3(transform.position.x, transform.position.y + 0.021f, transform.position.z);
            RaycastHit hitLower;
            Debug.DrawRay(RayPositionBottom, transform.TransformDirection(Vector3.forward), Color.blue);
            Debug.DrawRay(RayPositionTop, transform.TransformDirection(Vector3.forward), Color.blue);
            if (playerInput.CharacterControls.Move.ReadValue<Vector2>().x != 0 || playerInput.CharacterControls.Move.ReadValue<Vector2>().y != 0)
            {
                if (Physics.Raycast(RayPositionBottom, transform.TransformDirection(Vector3.forward), out hitLower, 0.4f))
                {
                    RaycastHit hitUpper;
                    if (!Physics.Raycast(RayPositionTop, transform.TransformDirection(Vector3.forward), out hitUpper, 0.7f, stickMasks, QueryTriggerInteraction.Ignore))
                    {
                        myRigidbody.position -= new Vector3(0, -stepSmooth, 0);
                    }
                }
                if (Physics.Raycast(RayPositionBottom, transform.TransformDirection(1.5f, 0, 1), out hitLower, 0.4f))
                {
                    RaycastHit hitUpper;
                    if (!Physics.Raycast(RayPositionTop, transform.TransformDirection(1.5f, 0, 1), out hitUpper, 0.7f, stickMasks, QueryTriggerInteraction.Ignore))
                    {
                        myRigidbody.position -= new Vector3(0, -stepSmooth, 0);
                    }
                }
                if (Physics.Raycast(RayPositionBottom, transform.TransformDirection(-1.5f, 0, 1), out hitLower, 0.4f))
                {
                    RaycastHit hitUpper;
                    if (!Physics.Raycast(RayPositionTop, transform.TransformDirection(-1.5f, 0, 1), out hitUpper, 0.7f, stickMasks, QueryTriggerInteraction.Ignore))
                    {
                        myRigidbody.position -= new Vector3(0, -stepSmooth, 0);
                    }
                }
            }


            //wall in front of Controller
            RayPositionTop = new Vector3(transform.position.x, transform.position.y + 1.9f, transform.position.z);
            Vector3 RayPositionMiddle = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            RayPositionBottom = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            Vector3 RayVelocityDirection = new Vector3(myRigidbody.velocity.x, 0, myRigidbody.velocity.z);
            if (myRigidbody != null)
            {
                Debug.DrawRay(RayPositionTop, RayVelocityDirection, Color.red);
                Debug.DrawRay(RayPositionBottom, RayVelocityDirection, Color.red);
                if (Physics.Raycast(RayPositionTop, RayVelocityDirection, 0.6f) || Physics.Raycast(RayPositionBottom, RayVelocityDirection, 0.6f) || Physics.Raycast(RayPositionMiddle, RayVelocityDirection, 0.3f))
                {
                    //onWall = true;
                    Debug.Log("hit");
                    myRigidbody.velocity = new Vector3(0, myRigidbody.velocity.y, 0);
                }
                else
                    onWall = false;

            }


        }

        public void SetAnimationBools()
        {
            //walking
            float horizontal = playerInput.CharacterControls.Move.ReadValue<Vector2>().x;
            float vertical = playerInput.CharacterControls.Move.ReadValue<Vector2>().y;
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            walking = direction.magnitude >= .1f;

            //falling
            falling = !isGrounded && gravityCurveTime > gravityCurveLenght / 1.5;

            //action
            jumping = playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && isGrounded;
            action = playerInput.CharacterControls.Jump.ReadValue<float>() != 0 && !isInCooldown;
        }

        public void FixedUpdate()
        {
            LimitVelocity();
            Turn();
        }

        public void Turn()
        {
            float horizontal = playerInput.CharacterControls.Move.ReadValue<Vector2>().x;
            float vertical = playerInput.CharacterControls.Move.ReadValue<Vector2>().y;
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= .1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }

        /// <summary>
        /// We limit the velocity of the rigidbody
        /// </summary>
        public void LimitVelocity()
        {
            Vector3 tempVelocity = myRigidbody.velocity;

            if (tempVelocity.x > maxMoveVelocity)
                tempVelocity.x = maxMoveVelocity;
            if (tempVelocity.x < -maxMoveVelocity)
                tempVelocity.x = -maxMoveVelocity;

            if (tempVelocity.z > maxMoveVelocity)
                tempVelocity.z = maxMoveVelocity;
            if (tempVelocity.z < -maxMoveVelocity)
                tempVelocity.z = -maxMoveVelocity;

            if (tempVelocity.y > maxJumpVelocity.y)
                tempVelocity.y = maxJumpVelocity.y;
            if (tempVelocity.y < maxJumpVelocity.x)
                tempVelocity.y = maxJumpVelocity.x;



            myRigidbody.velocity = tempVelocity;
        }

        /// <summary>
        /// draws Gizmos if selected
        /// </summary>
        public void OnDrawGizmosSelected()
        {
            if (isGrounded)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            foreach (RayData ray in jumpRayList)
            {
                Gizmos.DrawRay(transform.position + ray.position, ray.direction * ray.length);
            }

            Gizmos.color = Color.black;
            Vector3 RayPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Ray directionRay = new Ray(RayPosition, directionGizmo);
            Gizmos.DrawRay(directionRay);

            if (myRigidbody != null)
            {
                Gizmos.color = Color.green;
                Ray velocityRay = new Ray(RayPosition, myRigidbody.velocity);
                Gizmos.DrawRay(velocityRay);
            }



        }

        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(cooldownTime);
            isInCooldown = false;

        }

        /// <summary>
        /// if player is in Capricorn state he can performe a dash
        /// </summary>
        /// <returns></returns>
        IEnumerator CapricornDash()
        {
            if (!isInCooldown)
            {
                Debug.Log("dash");
                yield return new WaitForSeconds(.4f);
                for (int i = 0; i < 50; i++)
                {
                    /*
                    float horizontal = playerInput.CharacterControls.Move.ReadValue<Vector2>().x;
                    float vertical = playerInput.CharacterControls.Move.ReadValue<Vector2>().y;
                    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    //transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    //move Rigidbody
                    myRigidbody.velocity = moveDir.normalized * (moveSpeed * 200) * Time.deltaTime;
                    yield return new WaitForSeconds(.005f);
                    */

                    directionMove += new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
                    myRigidbody.velocity = new Vector3(directionMove.x, myRigidbody.velocity.y, directionMove.z) * 100;
                    directionMove = Vector3.zero;
                    yield return new WaitForSeconds(.0005f);

                    //extra push power
                    Collider[] allObjects = Physics.OverlapSphere(transform.position, 3);   //all Objects in explosion Range
                    foreach (Collider j in allObjects)
                    {
                        Rigidbody rig = j.GetComponent<Rigidbody>();
                        if (rig != null && rig != myRigidbody)
                        {
                            rig.AddExplosionForce(1, transform.position, 1, 1f, ForceMode.Impulse);
                        }
                    }

                }
            }
        }
    }





    /// <summary>
    ///  class with variables for the ground check Raycast
    /// </summary>
    [System.Serializable]
    public class RayData
    {
        public Vector3 position;
        public Vector3 direction;
        public float length;
    }
}
