using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gentleforge
{
    [RequireComponent(typeof(Rigidbody))]
    public class GentleController : MonoBehaviour
    {
        [Header("MOVEMENT")]
        public AnimationCurve jumpCurve;
        [Tooltip("The force that we apply to the movement")]
        public float moveSpeed = 5;
        public float maxMoveVelocity = 10;

        [Header("JUMP")]
        public float jumpForce = 100;
        public Vector2 maxJumpVelocity = new Vector2(-10, 5);

        [Header("REFERENCES")]
        private Rigidbody myRigidbody;

        public void Awake()
        {
            myRigidbody = this.GetComponent<Rigidbody>();
        }

        public void Update()
        {
            GetPlayerInput();
        }

        /// <summary>
        /// We get the Input of the Player
        /// </summary>
        public void GetPlayerInput()
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);
            myRigidbody.AddForce(movement, ForceMode.Acceleration);

            if (Input.GetKeyDown(KeyCode.Space))
                myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        public void FixedUpdate()
        {
            LimitVelocity();
        }

        /// <summary>
        /// We kimit the velocity of the rigidbody
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
    }
}

