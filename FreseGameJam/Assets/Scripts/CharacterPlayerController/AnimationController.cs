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
        //Debug.Log(animator);

        Movement = GetComponentInParent<ThirdPersonMovement>();
        //Debug.Log(Movement);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (GetComponentInParent<StateController>().crane)
        {
            Debug.Log("crane");
            transform.rotation = Quaternion.EulerAngles(Vector3.zero);
        }
        */
        //Debug.Log(gameObject);
        //transform.rotation = Quaternion.EulerAngles(Vector3.zero);

        animator.SetBool("isWalking", Movement.walking);
        walking_Sound.enabled = Movement.walking;

        animator.SetBool("isFalling", Movement.falling);
        falling_Sound.enabled = Movement.falling;

        animator.SetBool("Action", Movement.action);

        animator.SetBool("isJumping", Movement.jumping);

        if (GetComponentInParent<StateController>().frog)
        {
            animator.SetBool("isJumping", Movement.jumping);
        }

        
        
    }
}
