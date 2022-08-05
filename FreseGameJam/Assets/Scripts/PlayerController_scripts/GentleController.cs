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
        public AnimationCurve gravityCurve;
        private float time;
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

        [Header("REFERENCES")]
        private Rigidbody myRigidbody;

        public void Awake()
        {
            myRigidbody = this.GetComponent<Rigidbody>();
        }

        public void Update()
        {
            GroundCheckRay();
            GetPlayerInput();
        }

        /// <summary>
        /// We get the Input of the Player
        /// </summary>
        public void GetPlayerInput()
        {
            // we get the movemnent of the player
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);
            myRigidbody.AddForce(movement, ForceMode.Acceleration);

            // we get the jump input
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
                    isGrounded = true;
                    return;
                }
            }

            isGrounded = false;
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

