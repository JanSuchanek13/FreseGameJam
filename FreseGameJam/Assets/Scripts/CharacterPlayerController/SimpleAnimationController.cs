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
            StartCoroutine("SetRandomIdle");
        }

        private void FixedUpdate()
        {
            GroundCheck();
            SetVelocity();
            JumpCheck();
            FallCheck();
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
            animator.SetBool("isJumping", !thirdPersonMovement.isGrounded);
        }

        private void FallCheck()
        {
            animator.SetBool("isFalling", thirdPersonMovement.forcedFalling);
        }

        IEnumerator SetRandomIdle()
        {
            float duration = 1;
            float value = 0;
            while (true)
            {
                yield return new WaitForSeconds(5f);
                float currentAnimation = animator.GetFloat("randomIdle");
                float nextAnimation = Random.Range(1, 4);
                float elapsed = 0.0f;
                while (elapsed < duration)
                {
                    value = Mathf.Lerp(currentAnimation, nextAnimation, elapsed / duration);
                    elapsed += Time.deltaTime;
                    yield return null;
                    animator.SetFloat("randomIdle", value);
                }
                value = nextAnimation;
                
                
            }
        }
    }
}

