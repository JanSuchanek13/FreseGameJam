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
        //aktivieren sobald Gentle Controller bools hat für walking,falling,...
        
        animator.SetBool("isWalking", Movement.walking);
        walking_Sound.enabled = Movement.walking;

        animator.SetBool("isFalling", Movement.falling);
        falling_Sound.enabled = Movement.falling;

        animator.SetBool("Action", Movement.action);

        animator.SetBool("isJumping", Movement.jumping);
        




    }
}
