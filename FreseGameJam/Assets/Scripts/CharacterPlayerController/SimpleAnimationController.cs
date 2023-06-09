using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loneflower
{
    [RequireComponent(typeof(Animator))]
    public class SimpleAnimationController : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private CharacterController myController;
        [SerializeField] private ThirdPersonMovement thirdPersonMovement;
        private Animator animator;

        private void Awake()
        {
            animator = this.GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            GroundCheck();
            SetVelocity();
            JumpCheck();
        }

        /// <summary>
        /// We set the velocity on the Animation Controller
        /// </summary>
        private void SetVelocity()
        {
            //Debug.Log(myController.velocity);
            animator.SetFloat("xVelocity", thirdPersonMovement.speed);
            animator.SetFloat("yVelocity", myController.velocity.y);
        }

        /// <summary>
        /// We check if we are grounded
        /// </summary>
        private void GroundCheck()
        {
            animator.SetBool("isGrounded", myController.isGrounded);
        }

        private void JumpCheck()
        {
            animator.SetBool("isJumping", !thirdPersonMovement.isCoyoteGrounded);
        }
    }
}

