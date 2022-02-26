using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    ThirdPersonMovement Movement;

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
        animator.SetBool("isFalling", Movement.falling);
        
    }
}
