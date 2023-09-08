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
        [SerializeField] private StateController stateController;
        private Animator animator;
        private bool randomIdleIsRunning;

        bool test;

        private void Awake()
        {
            animator = this.GetComponent<Animator>();
            //StartCoroutine("SetRandomIdle");
        }

        private void FixedUpdate()
        {
            GroundCheck();
            SetVelocity();
            JumpCheck();
            FallCheck();
            FoldUpCheck();
            if (animator.GetFloat("xVelocity") == 0 && !randomIdleIsRunning)
            {
                StartCoroutine("SetRandomIdle");
            }
            else if(animator.GetFloat("xVelocity") != 0)
            {
                randomIdleIsRunning = false;
                StopCoroutine("SetRandomIdle");
            }
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

        private void FoldUpCheck()
        {
            // use this if we cant use transition to self = false in Animator
            /*
            if (stateController.isChanging && !test)
            {
                Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                animator.SetTrigger("TurnToCrane");
                test = true;
                Invoke("ResetTransitionTest", 0.5f);
            }
            */
            if (stateController.isChanging)
            {
                animator.SetTrigger("TurnToCrane");
            }
                
            //animator.SetBool("isFolding", stateController.isChanging);
        }

        private void ResetTransitionTest()
        {
            test = false;
        }

        IEnumerator SetRandomIdle()
        {
            randomIdleIsRunning = true;
            animator.SetFloat("randomIdle", 1);
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

