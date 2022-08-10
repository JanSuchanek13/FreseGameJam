using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gentleforge
{
    [RequireComponent(typeof(Rigidbody))]
    public class GentleController : MonoBehaviour
    {
        [Header("MOVEMENT")]
        [Tooltip("The force that we apply to the movement")]
        public float moveSpeed = 5;
        [Tooltip("The upper limit of the Move Velocity")]
        public float maxMoveVelocity = 10;

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
        private float gravityCurveTime;
        [Tooltip("Lenght of the gravityCurve")]
        private float gravityCurveLenght;

        [Header("DRAG")]
        [Tooltip("Drag we apply on Ground")]
        public float baseDrag = 5;
        [Tooltip("Drag we apply in air")]
        public float airDrag = 1;

        [Header("REFERENCES")]
        [Tooltip("Reference of the Rigidbody")]
        private Rigidbody myRigidbody;

        public Vector3 direction;

        public void Awake()
        {
            Setup();
        }

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
            GroundCheckRay();
            GetPlayerInput();
            CalculateGravity();
        }

        /// <summary>
        /// We get the Input of the Player
        /// </summary>
        public void GetPlayerInput()
        {
            direction = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z) - Camera.main.transform.position;
            direction = direction.normalized;

            // we get the movemnent of the player
            Vector3 movement = new Vector3((direction.x  * moveSpeed) * Input.GetAxis("Horizontal"), myRigidbody.velocity.y, (direction.z  * moveSpeed)* Input.GetAxis("Vertical"));
            myRigidbody.velocity = movement;
            
            

            // we get the jump input
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                myRigidbody.AddForce(Vector3.up * jumpForce);
        }

        /// <summary>
        /// Ray Check if Player is grounded
        /// </summary>
        void GroundCheckRay()
        {
            RaycastHit hit;
            foreach (RayData ray in jumpRayList)
            {
                if ((Physics.Raycast(transform.position + ray.position, ray.direction, out hit, ray.length, jumpRayLayer)))
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

            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y -gravityCurve.Evaluate(gravityCurveTime) * gravityStrengthMultiplier, myRigidbody.velocity.z);
            gravityCurveTime = gravityCurveTime + (Time.deltaTime * gravityCurveSpeed);
            
            // if gravityTimer is higher than the Lenght of the gravityCurve, we set it to the last Value in gravityCurve
            if (gravityCurveTime > gravityCurveLenght)
                gravityCurveTime = gravityCurveLenght;
            
        }

        public void FixedUpdate()
        {
            LimitVelocity();
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
            Ray directionRay = new Ray(RayPosition, direction);
            Gizmos.DrawRay(directionRay);

            if (myRigidbody != null)
            {
                Gizmos.color = Color.green;
                Ray velocityRay = new Ray(RayPosition, myRigidbody.velocity);
                Gizmos.DrawRay(velocityRay);
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

