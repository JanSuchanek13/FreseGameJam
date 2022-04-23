using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    ThirdPersonMovement Movement;

    [SerializeField] AudioSource walking_Sound;
    [SerializeField] AudioSource falling_Sound;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);

        Movement = GetComponentInParent<ThirdPersonMovement>();
        Debug.Log(Movement);
    }

    // Update is called once per frame
    void Update()
    {
        
        animator.SetBool("isWalking", Movement.walking);
        walking_Sound.enabled = Movement.walking;

        animator.SetBool("isFalling", Movement.falling);
        falling_Sound.enabled = Movement.falling;

        if (Movement.jumping != null)
        {
            animator.SetBool("isJumping", Movement.jumping);
        }
        

        
    }
}
