using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioSource walking_Sound;
    [SerializeField] AudioSource falling_Sound;

    [Header("REFERENCES")]
    [Tooltip("Reference of the Animator")]
    Animator animator;
    [Tooltip("Reference of the Controller")]
    Gentleforge.GentleController Movement;

    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //Debug.Log(animator);

        Movement = GetComponentInParent<Gentleforge.GentleController>();
        //Debug.Log(Movement);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isGrounded", Movement.isGrounded);
        walking_Sound.enabled = Movement.walking;
        if(Mathf.Abs( Movement.myRigidbody.velocity.x) > Mathf.Abs(Movement.myRigidbody.velocity.z))
            animator.SetFloat("moveVelocity", Mathf.Abs(Movement.myRigidbody.velocity.x));
        else
            animator.SetFloat("moveVelocity", Mathf.Abs(Movement.myRigidbody.velocity.z));
        falling_Sound.enabled = Movement.falling;
        animator.SetFloat("jumpVelocity", Movement.myRigidbody.velocity.y);
        animator.SetFloat("gravityCurveProgress", Movement.gravityCurveTime);
    }
}
